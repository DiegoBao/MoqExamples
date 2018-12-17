using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MoqTutorial
{
    public class ExportResult
    {
        public bool Result { get; set; }
        public string FullPath { get; set; }
        public string Error { get; set; } = null;
    }

    public class ExportService
    {
        private readonly IFileSystem fileSystem;
        private readonly ISettings settings;

        public ExportService(IFileSystem fileSystem, ISettings settings)
        {
            this.fileSystem = fileSystem;
            this.settings = settings;
        }

        public ExportResult Export(string[] data, string fileName)
        {
            try
            {
                settings.Load();
                string path = settings.ExportPath;

                string fullPath = Path.Combine(path, fileName);

                if (fileSystem.FileExist(fullPath))
                {
                    fileSystem.DeleteFile(fullPath);
                }

                using (var file = fileSystem.CreateFile(fullPath))
                {
                    foreach (var s in data)
                    {
                        file.WriteLine(s);
                    }
                    file.Close();
                }

                return new ExportResult
                {
                    Result = true,
                    FullPath = fullPath
                };
            }
            catch (Exception ex)
            {
                return new ExportResult
                {
                    Result = false,
                    FullPath = null,
                    Error = ex.Message
                };
            }
        }
    }
}
