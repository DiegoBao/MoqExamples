using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqTutorials
{
    public interface IService
    {
        void Execute(int value);

        bool IsValid();

        int Result { get; set; }
    }
}
