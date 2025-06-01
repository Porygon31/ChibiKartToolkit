using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace ChibiKartToolkit
{
    public partial class ResponsesEditor : Form
    {
        public ResponsesEditor()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            File.ReadAllText("responses.xml");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Fichiers XML (*.xml)|*.xml|Tous les fichiers (*.*)|*.*";
                saveFileDialog.Title = "Enregistrer responses";
                saveFileDialog.FileName = $"responses.xml";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText("responses.xml", txtBoxResponsesXML.Text);
                        MessageBox.Show("Responses enregistrés avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de l'enregistrement : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
