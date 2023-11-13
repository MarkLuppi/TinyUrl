using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TinyUrlWorkbench.Services.TinyUrl.Service;

namespace TinyUrlWorkbench.Services.TinyUrl.Client.TinyUrlClientPlugin
{
    //The logic for the client side relative to this service
    public class TinyUrlClientPlugin : Interfaces.IClient
    {
        public Dictionary<char, Delegate> Procs { get; set; }

        public string HelpText { get; set; }


        public Interfaces.IService Service { get; set; }

        public void Initialize()
        {
            HelpText = @"
             This application has the following one-letter commands available:
                    x -  Cancel and exit
                    h -  shows this help text 
                    g -  get the long urls entered so far
                    a -  add a long url (you will be prompted for the long url string)
                    d -  delete a long url (you will be prompted for the long url string)
                    l -  launch a long url (you will be prompted for it)

            These don't work yet:
                    u -  add a custom short url (you will be prompted for the short url, and then its long url)
                    n -  add a generated short url (you will be prompted for only the long url)
                    t -  get all short urls for a long url (you will be prompted for the  long url)
                    r -  remove a short url (you will be prompted for the short url, and then its long url)      
                    y -  launch a short url (you will be prompted for it)
                    c -  get the 'clicked' count for the short url (number of launches; you will be prompted for the short url)";

            Procs = new Dictionary<char, Delegate>();
            Procs['h'] = () => Console.WriteLine("Help: " + HelpText);
            Procs['x'] = () => { Console.Write("Exiting..."); System.Environment.Exit(1); };

            Procs['g'] = () =>
                {
                    Console.WriteLine("Getting long urls...");
                    var urls = (Service as TinyUrlService).GetUrls();
                    foreach (var url in urls)
                    {
                        Console.WriteLine("Long url: " + url.LongUrl.UrlResource);
                    }
                };

            Procs['l'] = () =>
            {
                Console.WriteLine("Launching long url...");
                Console.Write("Enter the long url: ");
                var url = Console.ReadLine();
                OpenUrl(url);
            };

            Procs['a'] = () => { Console.WriteLine("Adding long urls..."); Console.Write("Enter url: "); var url = Console.ReadLine(); (Service as TinyUrlService).AddUrl(url); };
            Procs['d'] = () => { Console.WriteLine("Deleting long url..."); Console.Write("Enter url: "); var url = Console.ReadLine(); (Service as TinyUrlService).DeleteUrl(url); };

            Procs['u'] = () => { Console.WriteLine("Adding short customized url..."); Console.Write("Enter long url: "); var url = Console.ReadLine(); Console.Write("Enter short url: "); var shortUrl = Console.ReadLine(); (Service as TinyUrlService).AddCustomShortUrl(url, shortUrl); };
            Procs['n'] = () => { Console.WriteLine("Adding short generated url..."); Console.Write("Enter long url: "); var url = Console.ReadLine(); var shortUrl = (Service as TinyUrlService).AddRandomShortUrl(url);  Console.WriteLine("short url: " + shortUrl); } ;
            Procs['t'] = () => { Console.WriteLine("Getting short urls..."); Console.Write("Enter long url: "); var url = Console.ReadLine(); var shortUrls = (Service as TinyUrlService).GetShortUrls(url); foreach (var shortUrl in shortUrls) Console.WriteLine("short url: " + shortUrl); };

        }

        public void OpenUrl(string url)
            {
                try
                {
                    Process.Start(url);
                }
                catch
                {
                    // hack because of this: https://github.com/dotnet/corefx/issues/10361
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        url = url.Replace(" & ", "^&");
                        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        Process.Start("xdg-open", url);
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        Process.Start("open", url);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

    }

