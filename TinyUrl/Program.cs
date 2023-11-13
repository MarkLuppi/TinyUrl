
using TinyUrlWorkbench.Channel;
using ClientViewManager = TinyUrlWorkbench.Client.CommandLineHandler;
using AppClient = TinyUrlWorkbench.Client.AppClient;
using TinyUrlWorkbench.Services.TinyUrl.Service;


Console.WriteLine("Hello, TinyUrlWorkbench!  Type 'h' for help");

//create the mock channel and business  service for each remote service, and load the client side plugin for the business service
var channel = Channel.ForAddress("https://mocklocalhost:8001/tiny-url-server");
var service = channel.CreateService<TinyUrlService>();
var client = new AppClient(service).ClientPlugInForService;

/* Create a client view manager, to manage command line input.
 * 
     When you run the app, the manager enters a read loop.  Enter "h" in the command window to see the available commands. They're listed below
     This application has the following one-letter commands available:
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
                    c -  get the 'clicked' count for the short url (number of launches; you will be prompted for the short url)
*/

await new ClientViewManager().Create(client);
