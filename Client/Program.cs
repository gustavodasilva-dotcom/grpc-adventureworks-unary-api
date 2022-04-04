using System;
using System.Configuration;

namespace Client
{
    class Program
    {
        #region Main

        static void Main(string[] args)
        {
            var target = ConfigurationManager.AppSettings["target"];

            var channel = new Grpc.Core.Channel(target, Grpc.Core.ChannelCredentials.Insecure);

            try
            {
                channel.ConnectAsync().ContinueWith(t =>
                {
                    if (t.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                        Console.WriteLine($"Connected successufully at the target {target}.");
                });

                Console.WriteLine("Please, inform the order ID:");
                var userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int orderID))
                {
                    var request = new AdventureWorks.OrderRequest(new()
                    {
                        SalesOrderId = orderID
                    });

                    var client = new AdventureWorks.OrderService.OrderServiceClient(channel);
                    
                    try
                    {
                        var response = client.Get(request, deadline: DateTime.UtcNow.AddSeconds(1));

                        if (response.Order != null)
                            Console.WriteLine($"Order info: {response}");
                        else
                            Console.WriteLine($"The order {orderID} does not exists.");
                    }
                    catch (Grpc.Core.RpcException e) when (e.StatusCode == Grpc.Core.StatusCode.DeadlineExceeded)
                    {
                        Console.WriteLine(e.Status.Detail);
                    }
                }
                else
                    Console.WriteLine($"The {userInput} is not numeric.");

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine($"The following error occurred: {e.Message}");
            }
            finally
            {
                if (channel != null) channel.ShutdownAsync().Wait();
                Console.WriteLine("Channel shutted down.");
            }
        }

        #endregion
    }
}
