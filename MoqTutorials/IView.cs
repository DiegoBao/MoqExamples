using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoqTutorials
{
    public interface IView
    {
        DialogResult ShowDialog();

        DialogResult DialogResult { get; set; }

        event EventHandler HandleEnter;
        event EventHandler HandleCancel;

        string Input { get; set; }

        void Close();
    }
}
