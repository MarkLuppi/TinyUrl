using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

using TinyUrlWorkbench.Channel;
using TinyUrlWorkbench.Services.Interfaces;
using TinyUrlWorkbench.Services.TinyUrl.Service;

namespace TinyUrlWorkbench.Client
{
    public class AppClient 
    {
        public CancellationTokenSource Cts { get; set; }
        public TinyUrlService Service { get; set; }


        public IClient ClientPlugInForService { get; set; }

        public AppClient (TinyUrlService service)
        {        
            Service = service;
            ClientPlugInForService = Service.Client;

            Initialize(new CancellationTokenSource());

        }


        public void Initialize(CancellationTokenSource token)
        {
            Cts = token;
            ClientPlugInForService.Initialize();
        }

        // Wait for the tasks to complete execution
        // Task.WaitAll(appClient);

        ServiceRequest GetResponse(BlockingCollection<ServiceRequest> bc)
        {
            ServiceRequest nextItem = null;
            while (true)
            {
              
                try
                {
                    if (bc.TryTake(out nextItem, 0, Cts.Token))
                    {
                        nextItem = bc.Take();
                        
                        Console.WriteLine(" Take:{0}", nextItem);
                    }
                }

                catch (OperationCanceledException)
                {
                    Console.WriteLine("Taking canceled.");
                    break;
                }

                // Slow down consumer just a little to cause
                // collection to fill up faster, and lead to "AddBlocked"
                Thread.Sleep(2000);
            }
            return nextItem;
       
        }


    }
}

