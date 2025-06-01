namespace ChibiKartToolkit
{
    partial class ResponsesEditor
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
            txtBoxResponsesXML = new TextBox();
            btnSave = new Button();
            lbl = new Label();
            SuspendLayout();
            // 
            // txtBoxResponsesXML
            // 
            txtBoxResponsesXML.AcceptsReturn = true;
            txtBoxResponsesXML.AcceptsTab = true;
            txtBoxResponsesXML.Location = new Point(21, 55);
            txtBoxResponsesXML.Multiline = true;
            txtBoxResponsesXML.Name = "txtBoxResponsesXML";
            txtBoxResponsesXML.Size = new Size(1033, 443);
            txtBoxResponsesXML.TabIndex = 0;
            txtBoxResponsesXML.TextChanged += textBox1_TextChanged;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnSave.Location = new Point(418, 520);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(256, 61);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // lbl
            // 
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lbl.Location = new Point(418, 9);
            lbl.Name = "lbl";
            lbl.Size = new Size(118, 21);
            lbl.TabIndex = 2;
            lbl.Text = "responses.xml";
            // 
            // ResponsesEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1094, 593);
            Controls.Add(lbl);
            Controls.Add(btnSave);
            Controls.Add(txtBoxResponsesXML);
            Name = "ResponsesEditor";
            Text = "Responses Editor";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtBoxResponsesXML;
        private Button btnSave;
        private Label lbl;
    }
}