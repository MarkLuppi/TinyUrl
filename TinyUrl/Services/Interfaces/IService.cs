using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyUrlWorkbench.Services.Interfaces
{
    public interface IService
    {
      IClient Client { get; set; }
    }
}
