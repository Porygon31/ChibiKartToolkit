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

            using (BinaryReader binReader = new BinaryReader(File.Open(inputpak, FileMode.Open)))
            {
                // Get Signature and check
                string correctSignature = "NKZIP";
                string fileSignature = Encoding.UTF8.GetString(binReader.ReadBytes(5));
                if (fileSignature != correctSignature)
                {
                    MessageBox.Show("Incorrect PAK File");
                    return;
                }

                // Move to file count
                binReader.BaseStream.Position = 0x24;
                uint fileCount = binReader.ReadUInt32();

                for (int i = 0; i < fileCount; i++)
                {
                    // Calculate Start of Data
                    long startOfFileData = binReader.BaseStream.Position + 0x108;

                    // Loop Getting Files
                    int fileLength = binReader.ReadInt32();

                    // Get FileNameDir Which we need to Save POS to return to
                    int charCount = 0;
                    string fileNameDir = "";
                    long startOfFileName = binReader.BaseStream.Position;

                    while (binReader.ReadByte() != 0)
                        charCount++;

                    binReader.BaseStream.Position = startOfFileName;
                    fileNameDir = Encoding.UTF8.GetString(binReader.ReadBytes(charCount));

                    string extractedFilename = Path.GetFileName(fileNameDir);
                    string extractedDIR = Path.GetDirectoryName(fileNameDir).Replace("./", "");

                    // Create Directory if needed
                    string exportDir = Path.Combine("Export", Path.GetFileNameWithoutExtension(inputpak), extractedDIR);
                    if (!Directory.Exists(exportDir))
                        Directory.CreateDirectory(exportDir);

                    // Now get the Data
                    binReader.BaseStream.Position = startOfFileData;
                    byte[] fileData = binReader.ReadBytes(fileLength);
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