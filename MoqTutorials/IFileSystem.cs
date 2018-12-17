using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MoqTutorial
{
    public interface IFileSystem
    {
        string GetCurrentDirectory();

        bool FileExist(string path);

        StreamWriter CreateFile(string path);

        void DeleteFile(string path);
    }
}
