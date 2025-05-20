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
            label1 = new Label();
            textBoxServerPort = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(79, 116);
            label1.Name = "label1";
            label1.Size = new Size(67, 15);
            label1.TabIndex = 0;
            label1.Text = "Server Port:";
            // 
            // textBoxServerPort
            // 
            textBoxServerPort.Location = new Point(161, 113);
            textBoxServerPort.Name = "textBoxServerPort";
            textBoxServerPort.Size = new Size(100, 23);
            textBoxServerPort.TabIndex = 4;
            // 
            // ServerSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(792, 228);
            Controls.Add(textBoxServerPort);
            Controls.Add(label1);
            Name = "ServerSettings";
            Text = "ServerSettings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBoxServerPort;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
    }
}