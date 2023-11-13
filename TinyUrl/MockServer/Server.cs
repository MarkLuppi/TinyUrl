using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockServer
{
    public class Server
    {
        public DataRepository Repo {  get; set; }
        public void Initialize() {
            InitializeMockDB();

        }

        private void InitializeMockDB()
        {
            Repo = new DataRepository();
        }
    }
}
