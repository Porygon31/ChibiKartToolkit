using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ChibiKartToolkit
{
    public static class Extract
    {
        public static void ExtractPAK(string inputpak)
        {
            int extractFileCount = 0;

            using (BinaryReader br = new BinaryReader(File.Open(inputpak, FileMode.Open)))
            {
                string correctSignature = "NKZIP";
                string fileSignature = Encoding.UTF8.GetString(br.ReadBytes(5));
                if (fileSignature != correctSignature)
                {
                    MessageBox.Show("Incorrect PAK File");
                    return;
                }

                // Move to file count
                br.BaseStream.Position = 0x24;
                uint fileCount = br.ReadUInt32();

                for (int i = 0; i < fileCount; i++)
                {
                    long startOfFileData = br.BaseStream.Position + 0x108;

                    int fileLength = br.ReadInt32();

                    int charCount = 0;
                    string fileNameDir = "";
                    long startOfFileName = br.BaseStream.Position;

                    while (br.ReadByte() != 0)
                        charCount++;

                    br.BaseStream.Position = startOfFileName;
                    fileNameDir = Encoding.UTF8.GetString(br.ReadBytes(charCount));

                    string extractedFilename = Path.GetFileName(fileNameDir);
                    string extractedDIR = Path.GetDirectoryName(fileNameDir).Replace("./", "");

                    string exportDir = Path.Combine("Export", Path.GetFileNameWithoutExtension(inputpak), extractedDIR);
                    if (!Directory.Exists(exportDir))
                        Directory.CreateDirectory(exportDir);

                    br.BaseStream.Position = startOfFileData;
                    byte[] fileData = br.ReadBytes(fileLength);
                    File.WriteAllBytes(Path.Combine(exportDir, extractedFilename), fileData);

                    extractFileCount++;
                }

                if (extractFileCount == fileCount)
                {
                    MessageBox.Show($"Extracted all {fileCount} Files Successfully");
                }
                else
                {
                    MessageBox.Show($"Extracted: {extractFileCount}\nExpected: {fileCount}");
                }
            }
        }
    }
}