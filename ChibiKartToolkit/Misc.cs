using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ChibiKartToolkit
{
    /// <summary>
    /// Provides static methods for performing XOR operations on strings and decrypting Networkini strings.
    /// </summary>
    public static class Misc
    {
        /// <summary>
        /// XORs the input string with each byte value from 0 to 255, appends the results to a file, and displays a message box when finished.
        /// </summary>
        /// <param name="input">The input string to be XOR'd.</param>
        /// <param name="outputPath">The path to the file where the XOR results will be appended.</param>
        public static void XorStringAndWriteToFile(string input, string outputPath)
        {
            StringBuilder result = new StringBuilder();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            for (int i = 0; i <= 255; i++)
            {
                try
                {
                    byte[] xorBytes = new byte[inputBytes.Length];

                    for (int j = 0; j < inputBytes.Length; j++)
                    {
                        xorBytes[j] = (byte)(inputBytes[j] ^ (byte)i);
                    }

                    string xorResult = Encoding.UTF8.GetString(xorBytes);
                    result.AppendLine($"XOR with {i:X2}: {xorResult}");

                    using (StreamWriter sw = new StreamWriter(File.Open(outputPath, FileMode.Append)))
                    {
                        sw.WriteLine($"XOR with {i:X4}: {xorResult}");
                    }
                }
                catch
                {
                    continue;
                }
            }

            MessageBox.Show("Finished XOR Test");
        }

        /// <summary>
        /// Decrypts the input string by subtracting 0x33 from each byte value and displays the decrypted string in a message box.
        /// </summary>
        /// <param name="inputString">The input string to be decrypted.</param>
        public static void DecryptNetworkini(string inputString)
        {
            byte[] stringBytes = Encoding.UTF8.GetBytes(inputString);
            StringBuilder result = new StringBuilder();

            foreach (byte b in stringBytes)
            {
                result.Append((char)(b - 0x33));
            }

            MessageBox.Show(result.ToString());
        }
    }
}