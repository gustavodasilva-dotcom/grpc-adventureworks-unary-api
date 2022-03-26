using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Repositories.Models
{
    public class OrderRepository : Config.Connection
    {
        public async Task<Server.Models.Entities.SalesOrderHeader> GetSalesOrderHeaderAsync(int salesOrderID)
        {
            #region SQL

            var query = $@"SELECT * FROM Sales.SalesOrderHeader (NOLOCK) WHERE SalesOrderID = {salesOrderID};";

            #endregion

            try
            {
                return await ExecutarSelectAsync<Server.Models.Entities.SalesOrderHeader>(query);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"The following error occurred: {e.Message}");

                return null;
            }
        }

        public async Task<IEnumerable<Server.Models.Entities.SalesOrderDetail>> GetSalesOrderDetailsAsync(int salesOrderID)
        {
            #region SQL

            var query = $@"SELECT * FROM Sales.SalesOrderDetail (NOLOCK) WHERE SalesOrderID = {salesOrderID};";

            #endregion

            try
            {
                return await ExecutarSelectListaAsync<Server.Models.Entities.SalesOrderDetail>(query);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"The following error occurred: {e.Message}");

                return null;
            }
        }

        public async Task<Server.Models.Entities.Customer> GetCustomerAsync(int customerID)
        {
            #region SQL

            var query = $@"SELECT * FROM Sales.Customer (NOLOCK) WHERE CustomerID = {customerID};";

            #endregion

            try
            {
                return await ExecutarSelectAsync<Server.Models.Entities.Customer>(query);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"The following error occurred: {e.Message}");

                return null;
            }
        }

        public async Task<Server.Models.Entities.Person> GetPersonAsync(int businessEntityID)
        {
            #region SQL

            var query = $@"SELECT * FROM Person.Person (NOLOCK) WHERE BusinessEntityID = {businessEntityID};";

            #endregion

            try
            {
                return await ExecutarSelectAsync<Server.Models.Entities.Person>(query);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"The following error occurred: {e.Message}");

                return null;
            }
        }

        public async Task<Server.Models.Entities.CreditCard> GetCreditCardAsync(int creditCardID)
        {
            #region SQL

            var query = $@"SELECT * FROM Sales.CreditCard (NOLOCK) WHERE CreditCardID = {creditCardID};";

            #endregion

            try
            {
                return await ExecutarSelectAsync<Server.Models.Entities.CreditCard>(query);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"The following error occurred: {e.Message}");

                return null;
            }
        }

        public async Task<IEnumerable<Server.Models.Entities.Product>> GetProductsAsync(int salesOrderID)
        {
            var query = new System.Text.StringBuilder();

            #region SQL 

            query.Append(@"SELECT P.*");
            query.Append(@" FROM Sales.SalesOrderDetail SD (NOLOCK)");
            query.Append(@" INNER JOIN Production.Product P (NOLOCK)");
            query.Append(@" ON SD.ProductID = P.ProductID");
            query.Append($@" WHERE SD.SalesOrderID = {salesOrderID};");

            #endregion

            try
            {
                return await ExecutarSelectListaAsync<Server.Models.Entities.Product>(query.ToString());
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"The following error occurred: {e.Message}");

                return null;
            }
        }
    }
}
