using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ChibiKartToolkit
{
    public partial class ServerSettings : Form
    {
        private const string SettingsFile = @"T:\OGPlanet\Chibi Kart\Network2.ini";

        public ServerSettings()
        {
            InitializeComponent();

            // Réglage borne port (optionnel si déjà fait dans le designer)
            gameServerPort.Minimum = 1;
            gameServerPort.Maximum = 65535;
        }

        private void ServerSettings_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void btnGameServerSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
            MessageBox.Show("Game Server settings saved!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadSettings()
        {
            if (!File.Exists(SettingsFile))
                return;

            var lines = File.ReadAllLines(SettingsFile);

            if (lines.Length >= 1)
                txtBoxGameServerIP.Text = lines[0];

            if (lines.Length >= 2 && int.TryParse(lines[1], out int port))
                gameServerPort.Value = Math.Min(Math.Max(port, 1), 65535);
        }

        private void SaveSettings()
        {
            string ip = txtBoxGameServerIP.Text.Trim();

            // Vérif IP (IPv4 uniquement)
            if (!IPAddress.TryParse(ip, out var address) || address.AddressFamily != AddressFamily.InterNetwork)
            {
                MessageBox.Show("Seules les adresses IPv4 sont acceptées.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBoxGameServerIP.Focus();
                return;
            }

            int port = (int)gameServerPort.Value;

            // Vérif port
            if (port < 1 || port > 65535)
            {
                MessageBox.Show("Le port doit être compris entre 1 et 65535.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                gameServerPort.Focus();
                return;
            }

            var lines = new[]
            {
                ip,
                port.ToString()
            };
            File.WriteAllLines(SettingsFile, lines);
        }

        private void btnGameServerSave_Click_1(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
