using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;

namespace ChibiKartToolkit
{
    public partial class Form1 : Form
    {
        private const string response = "responses.xml";
        private TcpListener listener;
        private Thread clientThread;
        private HttpListener httplistener;
        private Thread httplistenerThread;
        private Dictionary<string, byte[]> responses;

        public Form1()
        {
            InitializeComponent();
            _cts = new CancellationTokenSource();
        }

        private CancellationTokenSource _cts;

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cts?.Cancel();

            if (clientThread != null && clientThread.IsAlive)
            {
                clientThread.Join(TimeSpan.FromSeconds(5));
            }

            if (httplistenerThread != null && httplistenerThread.IsAlive)
            {
                httplistenerThread.Join(TimeSpan.FromSeconds(5));
            }
        }



        private void btnStartServer_Click(object sender, EventArgs e)
        {
            clientThread = new Thread(StartServer)
            {
                IsBackground = true
            };
            clientThread.Start();
        }

        private void StartServer()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, 50017);
                listener.Start();

                //Log that the server has started
                Invoke(new Action(() => AddMessage("Server started and listening on port 50017")));

                while (!_cts.Token.IsCancellationRequested)
                {
                    if (listener.Pending())
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Thread clientHandlerThread = new Thread(() => HandleClient(client, _cts.Token))
                        {
                            IsBackground = true
                        };
                        clientHandlerThread.Start();
                    }
                    else
                    {
                        Thread.Sleep(100); // Add a small delay to prevent busy waiting
                    }
                }
            }
            // Handle close of the app
            catch (OperationCanceledException)
            {

                Invoke(new Action(() => AddMessage("Server stopped due to cancellation request")));
            }
            //Handle any exceptions that occur
            catch (Exception ex)
            {
                Invoke(new Action(() => AddMessage("Server error: " + ex.Message)));
            }
        }

        private void btnStartWebServer_Click(object sender, EventArgs e)
        {
            httplistener = new HttpListener();
            httplistener.Prefixes.Add("http://127.0.0.1:80/");
            httplistener.Start();

            httplistenerThread = new Thread(HTTPListenForRequests)
            {
                IsBackground = true
            };
            httplistenerThread.Start();

            Invoke(new Action(() => AddMessage("HTTP Server Started Listening at 127.0.0.1:80")));
        }
        private void HTTPListenForRequests()
        {
            while (!_cts.Token.IsCancellationRequested && httplistener.IsListening)
            {
                try
                {
                    HttpListenerContext context = httplistener.GetContext();
                    HttpListenerRequest request = context.Request;

                    string requestInfo = $"{DateTime.Now}: {request.HttpMethod} {request.RawUrl}";
                    Invoke(new Action(() => AddMessage("Request: " + requestInfo)));
                    _cts.Token.ThrowIfCancellationRequested();
                    if (request.Url.AbsolutePath == "/static/Launcher/config/update.xml")
                    {
                        string filePath = "update.xml";
                        if (System.IO.File.Exists(filePath))
                        {
                            HttpListenerResponse response = context.Response;
                            response.ContentType = "application/xml";
                            byte[] buffer = System.IO.File.ReadAllBytes(filePath);
                            response.ContentLength64 = buffer.Length;
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                            response.OutputStream.Close();
                        }
                        else
                        {
                            context.Response.StatusCode = 404;
                            context.Response.StatusDescription = "Not Found";
                            context.Response.OutputStream.Close();
                        }
                    }
                    else
                    {
                        string responseString = "<html><body><h1>Hello, World</h1></body></html>";
                        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                        HttpListenerResponse response = context.Response;
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                        response.OutputStream.Close();
                    }
                }
                catch (OperationCanceledException)
                {
                    // Handle cancellation
                    break;
                }
                catch (Exception)
                {
                    // Handle exception silently
                }
            }
        }

        private void LoadResponses()
        {
            responses = new Dictionary<string, byte[]>();
            responses.Clear();

            XDocument xml = XDocument.Load(response);

            foreach (var responseElement in xml.Descendants("Response"))
            {
                var trigger = responseElement.Element("Trigger").Value;
                var data = responseElement.Element("Data").Value.Replace(" ", "");
                var dataBytes = Enumerable.Range(0, data.Length / 2)
                    .Select(i => Convert.ToByte(data.Substring(i * 2, 2), 16))
                    .ToArray();
                responses.Add(trigger, dataBytes);
            }
        }

        private byte[] DispatchPacket(byte[] data)
        {
            if (data.Length < 3)
                return null;

            byte cmd = data[2]; // 3rd byte = Command
            switch (cmd)
            {
                case 0xA6: // Login Request
                    return new byte[] { 0x04, 0x00, 0x12, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                case 0x0F: // Show Garage
                    return new byte[] { 0x04, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                //case 0x11: // Show Menu
                //    return new byte[] { 0x04, 0x00, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                //case 0x12: // Lobby OK
                //    return new byte[] { 0x04, 0x00, 0x12, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                default:
                    return null;
            }
        }

        private void HandleClient(TcpClient client, CancellationToken cancellationToken)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];

                    while (client.Connected && !cancellationToken.IsCancellationRequested)
                    {
                        if (!stream.DataAvailable)
                        {
                            Thread.Sleep(10);
                            continue;
                        }

                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0) break;

                        byte[] receivedPacket = buffer.Take(bytesRead).ToArray();
                        string hexRequest = BitConverter.ToString(receivedPacket).Replace("-", " ");
                        Invoke(new Action(() => AddMessage("[>] Received:  " + hexRequest)));

                        byte[] response = DispatchPacket(receivedPacket);
                        if (response != null)
                        {
                            stream.Write(response, 0, response.Length);
                            string hexResponse = BitConverter.ToString(response).Replace("-", " ");
                            Invoke(new Action(() => AddMessage("[<] Responded: " + hexResponse)));
                        }
                        else
                        {
                            Invoke(new Action(() => AddMessage("[!] Unknown CMD: 0x" +
                                (receivedPacket.Length > 2 ? receivedPacket[2].ToString("X2") : "??"))));
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Invoke(new Action(() => AddMessage("Client handler stopped due to cancellation request")));
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => AddMessage("Client error: " + ex.Message)));
            }
            finally
            {
                client.Close();
                Invoke(new Action(() => AddMessage("[-] Client disconnected")));
            }
        }

        private void AddMessage(string message)
        {
            lbxMessages.Items.Add(message);
            lbxMessages.TopIndex = lbxMessages.Items.Count - 1;
            lbxMessages.SelectedIndex = lbxMessages.Items.Count - 1;
        }

        // Gestionnaire du clic sur le bouton "Enregistrer les messages"
        private void btnSaveMessages_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Fichiers texte (*.txt)|*.txt|Tous les fichiers (*.*)|*.*";
                saveFileDialog.Title = "Enregistrer les messages";
                saveFileDialog.FileName = $"messages_{DateTime.Now:dd_MM_HH_mm_ss}.txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var lines = lbxMessages.Items.Cast<object>().Select(item => item.ToString());
                        System.IO.File.WriteAllLines(saveFileDialog.FileName, lines, Encoding.UTF8);
                        MessageBox.Show("Messages enregistrés avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de l'enregistrement : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DecryptStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Misc.DecryptNetworkini("ly#u5");
        }

        private void PAKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdInputPAK.ShowDialog() == DialogResult.OK)
            {
                Extract.ExtractPAK(ofdInputPAK.FileName);
            }
        }

        private void XORTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string inputString = "ly#u5";
            Misc.XorStringAndWriteToFile(inputString, "OutputXOR.txt");
        }


        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Chibi Kart Toolkit{Environment.NewLine}" +
                $"Our goal is to create a server emulator for Chibi Kart/Kart'n'Crazy{Environment.NewLine}" +
                $"Forked by @Porygon31 on the Tool Created by @yuviapp{Environment.NewLine}{Environment.NewLine}Ver:0.02");
        }

        private void ToolStripMenuItemInspectXML_Click(object sender, EventArgs e)
        {
            Packet packetForm = new();
            packetForm.Show();
        }

        private void BtnLaunchChibiKart_Click(object sender, EventArgs e)
        {
            const string FileName = "T:\\OGPlanet\\Chibi Kart\\KnC.exe";
            Process process = Process.Start(FileName);
            Invoke(new Action(() => AddMessage("")));
        }

        public void linkLabel1_ShowResponsesXML(object sender, LinkLabelLinkClickedEventArgs e)
        {

            ResponsesEditor responsesEditor = new ResponsesEditor();
            responsesEditor.Show();
        }

        private void btnServerSettings_Click(object sender, EventArgs e)
        {
            ServerSettings serverSettingsForm = new();
            serverSettingsForm.Show();
        }
    }

}
