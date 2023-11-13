using Microsoft.EntityFrameworkCore;
using TinyUrlWorkbench.Services.TinyUrl.Service;
using static TinyUrlWorkbench.Services.TinyUrl.Service.TinyUrlService;

namespace TinyUrlWorkbench.Data
{

    public class ApiContext : DbContext
    {
        protected override void OnConfiguring
       (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "TinyUrlDB");
        }
  
    }
}

public class DataRepository : IDataRepository
{
    public static TinyUrlContext Context { get; set; }
    public DataRepository()
    {
        Context = new TinyUrlContext();
        AddLongUrl("www.amazon.com");
        AddLongUrl("www.google.com");
    }

    public void AddLongUrl(string url)
    {
        var entry = new TinyUrls
        {
            LongUrl = new LongUrl() { UrlResource = url }
        };
        Context.Urls.Add(entry);
        Context.SaveChanges();
    }

    public List<TinyUrls> GetLongUrls()
    {
        using (var context = new TinyUrlContext())
        {
            var list = context.Urls
                .Include(a => a.LongUrl)
                .ToList();
            return list;
        }
    }
}
