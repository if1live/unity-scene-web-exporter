using System.IO;
using System.Text;

namespace Assets.Kanau.Utils {
    public class FileHelper
    {
        public static void SaveContentsAsFile(string content, string path)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(content);
            FileStream fs = File.Create(path);
            fs.Write(info, 0, info.Length);
            fs.Close();
        }
    }
}