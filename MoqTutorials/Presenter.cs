using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoqTutorials
{
    public class Presenter
    {
        private readonly IView view;

        public Presenter(IView view)
        {
            this.view = view;
        }

        public string Name { get; set; }
        
        public bool Show()
        {
            try
            {
                SubscribeEvents();

                return view.ShowDialog() == DialogResult.OK;
            }
            finally
            {
                UnsubscribeEvents();
            }
        }

        private void UnsubscribeEvents()
        {
            view.HandleEnter -= HandleEnter;
            view.HandleCancel -= HandleCancel;
        }

        private void HandleCancel(object sender, EventArgs e)
        {
            view.DialogResult = DialogResult.Cancel;
        }

        private void HandleEnter(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(view.Input))
            {
                // Invalid value... show messages or whatever
            }
            else
            {
                this.Name = view.Input;
                view.DialogResult = DialogResult.OK;
                view.Close();
            }
        }

        private void SubscribeEvents()
        {
            view.HandleEnter += HandleEnter;
            view.HandleCancel += HandleCancel;
        }
    }
}
