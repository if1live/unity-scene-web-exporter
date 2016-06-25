using System.IO;

namespace Assets.Kanau.Utils
{
    public class ExportPathHelper
    {
        private string targetFilePath;

        public ExportPathHelper(string targetFilePath)
        {
            this.targetFilePath = targetFilePath;
        }

        public string Extension
        {
            get
            {
                var ext = Path.GetExtension(targetFilePath);
                if(ext.StartsWith("."))
                {
                    return ext.Remove(0, 1);
                }
                else
                {
                    return ext;
                }
            }
        }

        public string RootPath
        {
            get { return Path.GetDirectoryName(targetFilePath); }
        }
        public string Prefix
        {
            get { return Path.GetFileNameWithoutExtension(targetFilePath); }
        }

        public string SceneFileName
        {
            get { return string.Format("{0}.{1}", Prefix, Extension); }
        }

        public string SceneFilePath
        {
            get { return Path.Combine(RootPath, SceneFileName); } 
        }

        public string ToFilePath(string filepath)
        {
            return Path.Combine(RootPath, filepath);
        }
    }
}