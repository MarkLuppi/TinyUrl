using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyUrlWorkbench.Data;


namespace TinyUrlWorkbench.Services.TinyUrl.Service
{
    public class TinyUrlContext : ApiContext
    {
        public DbSet<TinyUrls> Urls { get; set; }
    }
}
