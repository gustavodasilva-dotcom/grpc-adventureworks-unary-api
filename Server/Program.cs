using Grpc.Core;
using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;

namespace Server
{
    class Program
    {
        #region Main

        static void Main(string[] args)
        {
            var credentials = SslSecurity();
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
                        Ports = { new ServerPort(_host, _port, credentials) }
                    };
                }

                server.Start();

                Console.WriteLine($"The server is listening at the port {_port}.");
                Console.ReadKey();
            }
            catch (IOException e)
            {
                Console.WriteLine($"The following error occurred: {e.Message}");
            }
            finally
            {
                if (server != null) server.ShutdownAsync().Wait();
            }
        }

        #endregion

        #region SSL

        private static SslServerCredentials SslSecurity()
        {
            SslServerCredentials credentials = null;

            try
            {
                var serverCert = File.ReadAllText(@"Ssl/server.crt");
                var serverKey = File.ReadAllText(@"Ssl/server.key");
                var caCrt = File.ReadAllText(@"Ssl/ca.crt");
                var keyPair = new KeyCertificatePair(serverCert, serverKey);

                if (credentials == null) credentials = new SslServerCredentials(new List<KeyCertificatePair>() { keyPair }, caCrt, true);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            return credentials;
        }

        #endregion
    }
}
