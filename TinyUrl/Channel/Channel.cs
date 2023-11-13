
using MockServer;
using System.Collections.Concurrent;

namespace TinyUrlWorkbench.Channel
{
    
    public class Channel
    {
        public  CancellationTokenSource Cts { get; set; }
        public  BlockingCollection<ServiceRequest> ConnectionRequest { get; set; }
        public BlockingCollection<ServiceResponse> ConnectionResponse { get; set; }
        public string Url{ get; set; }
        public Server Server { get; set; }
        public static Channel ForAddress(string url)
        {
         
            return new Channel(url);
        }

        Channel (string apiUrl)
        {
            Url = apiUrl;  //not used for anything in this Mock Channel
            ConfigureMockServerEnvironment();
            CreateRequestQueue();
            CreateResponseQueue();
        }

        public T CreateService<T>() {
            return Activator.CreateInstance<T>();
        }

        public void CreateRequestQueue()
        {
            Cts = new CancellationTokenSource();
            ConnectionRequest = new BlockingCollection<ServiceRequest>();
        }

        public void CreateResponseQueue()
        {
            Cts = new CancellationTokenSource();
            ConnectionRequest = new BlockingCollection<ServiceRequest>();      
        }

        void ConfigureMockServerEnvironment()
        {
            Server = new Server();
            Server.Initialize();
        }

        public  void TinyUrlAppRequest(ServiceRequest request)
        {
            using (var bc = ConnectionRequest)
            {
                var ct = Cts;
                bool success = false;
                var index = 0;
               
                do
                {
                    // Cancellation causes OCE. We know how to handle it.
                    try
                    {
                        // A shorter timeout causes more failures.
                        success = bc.TryAdd(request, 2, ct.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Add loop canceled.");
                        // Let other threads know we're done in case
                        // they aren't monitoring the cancellation token.
                        bc.CompleteAdding();
                        break;
                    }

                    if (success)
                    {
                        Console.WriteLine(" Add:{0}", request);
                        index++;
                    }
                    else
                    {
                        Console.Write(" AddBlocked:{0} Count = {1}", request.ToString(), bc.Count);
                        // Don't increment nextItem. Try again on next iteration.

                    }
                    Thread.Sleep(2000);
                } while (true);
            }

            // No lock required here because only one producer.
            Console.WriteLine("Request Handled");
        }
    }

    public class ServiceRequest
    {
        int Id;
        public string [] RequestPayload { get; set; }
    }

    public class ServiceResponse
    {
        int requestId;
        public string [] ResponsePayload { get; set; }
    }


}

