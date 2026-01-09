namespace ChibiKartToolkit
{
    partial class ServerSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelGSIp = new Label();
            txtBoxGameServerIP = new TextBox();
            groupBox1 = new GroupBox();
            gameServerPort = new NumericUpDown();
            btnGameServerSave = new Button();
            labelGSPort = new Label();
            groupBox2 = new GroupBox();
            webServerPort = new NumericUpDown();
            btnWebServerSave = new Button();
            label1 = new Label();
            label2 = new Label();
            txtWebServerIP = new TextBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gameServerPort).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webServerPort).BeginInit();
            SuspendLayout();
            // 
            // labelGSIp
            // 
            labelGSIp.AutoSize = true;
            labelGSIp.Location = new Point(22, 57);
            labelGSIp.Name = "labelGSIp";
            labelGSIp.Size = new Size(89, 15);
            labelGSIp.TabIndex = 0;
            labelGSIp.Text = "Game Server IP:";
            // 
            // txtBoxGameServerIP
            // 
            txtBoxGameServerIP.Location = new Point(117, 54);
            txtBoxGameServerIP.Name = "txtBoxGameServerIP";
            txtBoxGameServerIP.Size = new Size(186, 23);
            txtBoxGameServerIP.TabIndex = 4;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(gameServerPort);
            groupBox1.Controls.Add(btnGameServerSave);
            groupBox1.Controls.Add(labelGSPort);
            groupBox1.Controls.Add(labelGSIp);
            groupBox1.Controls.Add(txtBoxGameServerIP);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(338, 166);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // gameServerPort
            // 
            gameServerPort.Location = new Point(143, 88);
            gameServerPort.Name = "gameServerPort";
            gameServerPort.Size = new Size(55, 23);
            gameServerPort.TabIndex = 8;
            // 
            // btnGameServerSave
            // 
            btnGameServerSave.Location = new Point(143, 122);
            btnGameServerSave.Name = "btnGameServerSave";
            btnGameServerSave.Size = new Size(115, 25);
            btnGameServerSave.TabIndex = 7;
            btnGameServerSave.Text = "Save";
            btnGameServerSave.UseVisualStyleBackColor = true;
            btnGameServerSave.Click += btnGameServerSave_Click_1;
            // 
            // labelGSPort
            // 
            labelGSPort.AutoSize = true;
            labelGSPort.Location = new Point(22, 96);
            labelGSPort.Name = "labelGSPort";
            labelGSPort.Size = new Size(101, 15);
            labelGSPort.TabIndex = 5;
            labelGSPort.Text = "Game Server Port:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(webServerPort);
            groupBox2.Controls.Add(btnWebServerSave);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(txtWebServerIP);
            groupBox2.Location = new Point(371, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(333, 166);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "groupBox2";
            // 
            // webServerPort
            // 
            webServerPort.Location = new Point(145, 88);
            webServerPort.Name = "webServerPort";
            webServerPort.Size = new Size(55, 23);
            webServerPort.TabIndex = 9;
            // 
            // btnWebServerSave
            // 
            btnWebServerSave.Location = new Point(154, 122);
            btnWebServerSave.Name = "btnWebServerSave";
            btnWebServerSave.Size = new Size(101, 25);
            btnWebServerSave.TabIndex = 8;
            btnWebServerSave.Text = "Save";
            btnWebServerSave.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(22, 96);
            label1.Name = "label1";
            label1.Size = new Size(94, 15);
            label1.TabIndex = 5;
            label1.Text = "Web Server Port:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(22, 57);
            label2.Name = "label2";
            label2.Size = new Size(82, 15);
            label2.TabIndex = 0;
            label2.Text = "Web Server IP:";
            // 
            // txtWebServerIP
            // 
            txtWebServerIP.Location = new Point(117, 54);
            txtWebServerIP.Name = "txtWebServerIP";
            txtWebServerIP.Size = new Size(138, 23);
            txtWebServerIP.TabIndex = 4;
            // 
            // ServerSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(792, 228);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "ServerSettings";
            Text = "ServerSettings";
            Load += ServerSettings_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gameServerPort).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)webServerPort).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label labelGSIp;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtBoxGameServerIP;
        private TextBox txtWebServerIP;
        private TextBox textBox4;
        private GroupBox groupBox1;
        private Label labelGSPort;
        private GroupBox groupBox2;
        private Label label1;
        private NumericUpDown gameServerPort;
        private Button btnGameServerSave;
        private Button btnWebServerSave;
        private NumericUpDown webServerPort;
    }
}