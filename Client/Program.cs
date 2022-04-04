using System;
using System.IO;
using System.Configuration;
using Grpc.Core;

namespace Client
{
    class Program
    {
        #region Main

        static void Main(string[] args)
        {
            var credentials = SslSecurity();
            var host = ConfigurationManager.AppSettings["host"];
            var port = int.Parse(ConfigurationManager.AppSettings["port"]);
            var channel = new Channel(host, port, credentials);

            try
            {
                channel.ConnectAsync().ContinueWith(t =>
                {
                    if (t.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                        Console.WriteLine($"Connected successufully at the target {host}:{port}.");
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
                        var response = client.Get(request, deadline: DateTime.UtcNow.AddSeconds(3));

                        if (response.Order != null)
                            Console.WriteLine($"Order info: {response}");
                        else
                            Console.WriteLine($"The order {orderID} does not exists.");
                    }
                    catch (RpcException e) when (e.StatusCode == StatusCode.DeadlineExceeded)
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

        #region SSL

        private static SslCredentials SslSecurity()
        {
            SslCredentials channelCredentials = null;

            try
            {
                var clientCert = File.ReadAllText(@"Ssl/client.crt");
                var clientKey = File.ReadAllText(@"Ssl/client.key");
                var caCrt = File.ReadAllText(@"Ssl/ca.crt");

                if (channelCredentials == null) channelCredentials = new SslCredentials(caCrt, new KeyCertificatePair(clientCert, clientKey));
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            return channelCredentials;
        }

        #endregion
    }
}
