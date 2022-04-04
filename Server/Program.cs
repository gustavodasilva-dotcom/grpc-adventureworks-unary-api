using System.Configuration;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Main

            var _port = int.Parse(ConfigurationManager.AppSettings["port"]);
            var _host = ConfigurationManager.AppSettings["host"];

            Grpc.Core.Server server = null;

            try
            {
                if (server == null)
                {
                    server = new Grpc.Core.Server()
                    {
                        Services = { AdventureWorks.OrderService.BindService(new Services.OrderServiceImpl()) },
                        Ports = { new Grpc.Core.ServerPort(_host, _port, Grpc.Core.ServerCredentials.Insecure) }
                    };
                }

                server.Start();

                System.Console.WriteLine($"The server is listening at the port {_port}.");
                System.Console.ReadKey();
            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine($"The following error occurred: {e.Message}");
            }
            finally
            {
                if (server != null) server.ShutdownAsync().Wait();
            }

            #endregion
        }
    }
}
