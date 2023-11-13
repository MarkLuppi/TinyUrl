using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyUrlWorkbench.Services.TinyUrl.Service;

namespace TinyUrlWorkbench.Services.Interfaces
{
    public interface IClient
    {
        public Dictionary<char, Delegate> Procs { get; set; }
        public string HelpText { get; set; }
        public void Initialize();
        public IService Service { get; set; }
    }
}
