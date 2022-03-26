using System.Configuration;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var target = ConfigurationManager.AppSettings["target"];

            var channel = new Grpc.Core.Channel(target, Grpc.Core.ChannelCredentials.Insecure);

            try
            {
                channel.ConnectAsync().ContinueWith(t =>
                {
                    if (t.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                        System.Console.WriteLine($"Connected successufully at the target {target}.");
                });

                System.Console.WriteLine("Please, inform the order ID:");
                var userInput = System.Console.ReadLine();

                if (int.TryParse(userInput, out int orderID))
                {
                    var request = new AdventureWorks.OrderRequest(new()
                    {
                        SalesOrderId = orderID
                    });

                    var client = new AdventureWorks.OrderService.OrderServiceClient(channel);
                    var response = client.Get(request);

                    if (response.Order != null)
                        System.Console.WriteLine($"Order info: {response}");
                    else
                        System.Console.WriteLine($"The order {orderID} does not exists.");
                }
                else
                    System.Console.WriteLine($"The {userInput} is not numeric.");

                System.Console.ReadKey();
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"The following error occurred: {e.Message}");
            }
        }
    }
}
