using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
using static ChibiKartToolkit.Misc;
using static ChibiKartToolkit.Extract;

namespace ChibiKartToolkit
{
    public partial class Form1 : Form
    {
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
            catch (OperationCanceledException)
            {
                // Handle cancellation
                Invoke(new Action(() => AddMessage("Server stopped due to cancellation request")));
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => AddMessage("Server error: " + ex.Message)));
            }
        }

        private void LoadResponses()
        {
            responses = new Dictionary<string, byte[]>();
            responses.Clear();

            var xml = XDocument.Load("responses.xml");
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

        private void HandleClient(TcpClient client, CancellationToken cancellationToken)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string request = BitConverter.ToString(buffer, 0, bytesRead).Replace("-", "");
                    string requestPrefix = BitConverter.ToString(buffer, 0, 4).Replace("-", "");

                    cancellationToken.ThrowIfCancellationRequested();

                    Invoke(new Action(() => AddMessage("Received:    " + request)));
                    LoadResponses();

                    if (responses.TryGetValue(requestPrefix, out byte[] responseBytes))
                    {
                        stream.Write(responseBytes, 0, responseBytes.Length);
                        string responseString = BitConverter.ToString(responseBytes).Replace("-", "");
                        Invoke(new Action(() => AddMessage("Responded: " + responseString)));
                    }
                    else
                    {
                        byte[] response = buffer.Take(bytesRead).ToArray();
                        stream.Write(response, 0, response.Length);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation
                Invoke(new Action(() => AddMessage("Client handler stopped due to cancellation request")));
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => AddMessage("Client error: " + ex.Message)));
            }
            finally
            {
                client.Close();
            }
        }

        private void AddMessage(string message)
        {
            lbxMessages.Items.Add(message);
            lbxMessages.TopIndex = lbxMessages.Items.Count - 1;
            lbxMessages.SelectedIndex = lbxMessages.Items.Count - 1;
        }



        private void DecryptStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Misc.DecryptNetworkini("ly#u5");
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

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Chibi Kart Toolkit and Server Emulator{Environment.NewLine}Tool Created by @yuviapp{Environment.NewLine}Ver:0.01");
        }
    }
}
