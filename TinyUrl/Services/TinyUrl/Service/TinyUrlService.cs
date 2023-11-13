using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;



using TinyUrlWorkbench.Services.Interfaces;
using System;
using System.Diagnostics;
using TinyUrlWorkbench.Services.TinyUrl.Client.TinyUrlClientPlugin;


namespace TinyUrlWorkbench.Services.TinyUrl.Service
{
    public class TinyUrlService : IService
    {
        public IClient Client { get; set; }

        TinyUrlContext Context { get; set; }
        public TinyUrlService()
        {
            Client = new TinyUrlClientPlugin();
            Client.Service = this;
            Context = DataRepository.Context; 
        }

        public List<TinyUrls> GetUrls()
        {      
            var list = Context.Urls
                .Include(a => a.LongUrl)
                .ToList();
            return list;

        }

        public void AddUrl(string url)
        {
            var client = new TinyUrls
            {
                LongUrl = new LongUrl() { UrlResource = url }
            };
            Context.Urls.Add(client);
            Context.SaveChanges();
        }

        public void AddCustomShortUrl(string url, string shortUrl)
        {
            var longUrl = Context.Urls.Where(link => link.LongUrl.UrlResource == url).FirstOrDefault();
            longUrl.LongUrl.TinyUrls.Add(new ShortUrl() { UrlResource = shortUrl });
            Context.SaveChanges();
        }

        public string AddRandomShortUrl(string url)
        {
            var shortUrl = RandomString(8);
            var longUrl = Context.Urls.Where(link => link.LongUrl.UrlResource == url).FirstOrDefault();
            longUrl.LongUrl.TinyUrls.Add(new ShortUrl() { UrlResource = shortUrl });
            Context.SaveChanges();
            return shortUrl;
        }

        public void DeleteUrl(string url)
        {
            try
            {
                var deleted = Context.Urls.Where(link => link.LongUrl.UrlResource == url).FirstOrDefault();
                Context.Urls.Remove(deleted);
                Context.SaveChanges();
            }
            catch { }
        }


        public List<ShortUrl> GetShortUrls(string url)
        {
            try
            {
                var foundUrl = Context.Urls.Where(link => link.LongUrl.UrlResource == url).FirstOrDefault();
                return foundUrl.LongUrl.TinyUrls;
            }
            catch { }
            return null;
        }

        public interface IDataRepository
        {
            public List<TinyUrls> GetLongUrls();
        }

        public Dictionary<char, Delegate> Procs { get; set; }

       
      
        public static string RandomString(int length)
        {
            Random random = new Random();
           
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public class TinyUrls
    {
        public int Id { get; set; }
        public LongUrl LongUrl { get; set; }
     
    }

    public class LongUrl
    {
        public int Id { get; set; }
        public string UrlResource { get; set; }
        public List<ShortUrl> TinyUrls = new List<ShortUrl>();
        

    }

    public class ShortUrl
    {
        public int Id { get; set; }
        public string UrlResource { get; set; }
        public int clicked = 0;
    } 

   
}

