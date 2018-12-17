using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqTutorials
{
    public class JustAClass
    {
        private readonly IService service;

        public JustAClass(IService service)
        {
            this.service = service;
        }

        public bool ExecuteService(int value)
        {
            service.Execute(value);

            return service.IsValid();
        }

        public bool Execute2(int value)
        {
            service.Execute(value);
         
            return service.Result != 0;
        }
    }
}
