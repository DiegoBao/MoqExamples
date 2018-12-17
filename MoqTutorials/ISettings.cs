using System;
using System.Collections.Generic;
using System.Text;

namespace MoqTutorial
{
    public interface ISettings
    {
        void Load();

        string ExportPath { get; set; }
    }
}
