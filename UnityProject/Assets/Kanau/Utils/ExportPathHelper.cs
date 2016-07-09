using System.IO;

namespace Assets.Kanau.Utils {
    public class ExportPathHelper {
        public static readonly ExportPathHelper Instance;

        static ExportPathHelper() {
            Instance = new ExportPathHelper();
        }
        private ExportPathHelper() { }

        public void UpdateTargetFilePath(string targetFilePath) {
            this.targetFilePath = targetFilePath;
        }
        private string targetFilePath;

        public string Extension
        {
            get
            {
                var ext = Path.GetExtension(targetFilePath);
                if (ext.StartsWith(".")) {
                    return ext.Remove(0, 1);
                } else {
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

        public string ToFilePath(string filepath) {
            return Path.Combine(RootPath, filepath);
        }



        public string ToImagePath(string name) {
            var subdir = ExportSettings.Instance.destination.imageDirectory;
            return ToSubDirectoryPath(subdir, name);
        }
        public string ToModelPath(string name) {
            var subdir = ExportSettings.Instance.destination.modelDirectory;
            return ToSubDirectoryPath(subdir, name);
        }

        string ToSubDirectoryPath(string subdir, string name) {
            var dir = Path.Combine(RootPath, subdir);
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            return Path.Combine(dir, name);
        }
    }
}