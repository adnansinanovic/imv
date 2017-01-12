using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Sinantrop.IMV.Sync
{
    public class Compression
    {
        public MemoryStream Zip(MemoryStream plain)
        {
            plain.Seek(0, SeekOrigin.Begin);
            MemoryStream output = new MemoryStream();            
            using (GZipStream zipOutput = new GZipStream(output, CompressionMode.Compress))
            {
                plain.CopyTo(zipOutput);                
            }         
            
            return output;            
        }

        public string Zip(string plainText)
        {
            string result;
            using (MemoryStream output = new MemoryStream())
            {
                using (MemoryStream rawData = new MemoryStream(Encoding.UTF8.GetBytes(plainText)))
                {
                    using (GZipStream zipOutput = new GZipStream(output, CompressionMode.Compress))
                    {
                        rawData.CopyTo(zipOutput);
                    }
                }

                result = Convert.ToBase64String(output.ToArray());
            }

            return result;
        }

        public MemoryStream Unzip(MemoryStream zipped)
        {
            zipped.Seek(0, SeekOrigin.Begin);
            MemoryStream output = new MemoryStream();                                 
            
            using (GZipStream unZipOut = new GZipStream(zipped, CompressionMode.Decompress))
            {
                unZipOut.CopyTo(output);
            }            
            
            return output;
        }


        public string Unzip(string zipped)
        {
            string result;
            using (MemoryStream output = new MemoryStream())
            {
                using (MemoryStream compressed = new MemoryStream(Convert.FromBase64String(zipped)))
                {
                    using (GZipStream unZipOut = new GZipStream(compressed, CompressionMode.Decompress))
                    {
                        unZipOut.CopyTo(output);
                    }
                }

                result = Encoding.UTF8.GetString(output.ToArray());
            }

            return result;
        }
    }
}
