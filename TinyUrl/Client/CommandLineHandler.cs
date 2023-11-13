
using TinyUrlWorkbench.Services.Interfaces;

namespace TinyUrlWorkbench.Client
{
    public class CommandLineHandler
    {
        public Task Create(IClient client)
        {
            Dictionary<char, Delegate> proc = client.Procs;     

            return Task.Run(() =>
            {
                while (true)
                {
                    var key = Console.ReadKey(true).KeyChar;
                    try
                    {
                        proc[key].DynamicInvoke();
                    }
                    catch { }
                }
            });
        }

       
    }
}
