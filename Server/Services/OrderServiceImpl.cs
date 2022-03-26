using Grpc.Core;
using AdventureWorks;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Google.Protobuf.Collections;

namespace Server.Services
{
    public class OrderServiceImpl : OrderService.OrderServiceBase
    {
        #region Properties

        private readonly Repositories.Models.OrderRepository _orderRepository;

        #endregion

        #region Constructor

        public OrderServiceImpl()
        {
            _orderRepository = new Repositories.Models.OrderRepository();
        }

        #endregion

        #region InternalMethods

        public override async Task<OrderResponse> Get(OrderRequest request, ServerCallContext context)
        {
            var orderReturn = new OrderResponse();

            try
            {
                var order = await GetOrderAsync(request.SalesOrderId);

                if (order == null)
                {
                    System.Console.WriteLine("Some entities weren't found or some other error occurred.");
                    System.Console.WriteLine("Please, check the previous logs generated automatically.");
                    return orderReturn;
                }
                else
                {
                    InstantializeProductsReturn(order.Products, out RepeatedField<Product> products);

                    InstantializeCustomerReturn(order.Customer, out Customer customer);

                    InstantializeCreditCardReturn(order.CreditCard, out CreditCard creditCard);

                    orderReturn.Order = new Order
                    {
                        SalesOrderId = order.SalesOrderID,
                        SalesOrderNumber = string.IsNullOrEmpty(order.SalesOrderNumber) ? string.Empty : order.SalesOrderNumber,
                        PurchaseOrderNumber = string.IsNullOrEmpty(order.PurchaseOrderNumber) ? string.Empty : order.PurchaseOrderNumber,
                        OrderDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(order.OrderDate.ToUniversalTime()),
                        ShipDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(order.OrderDate.ToUniversalTime()),
                        IsOnlineOrder = order.IsOnlineOrder,
                        SubTotal = order.SubTotal,
                        TotalDue = order.TotalDue,
                        Products = { products },
                        Customer = customer,
                        CreditCard = creditCard
                    };

                    return orderReturn;
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"The following error occurred: {e.Message}");

                return orderReturn;
            }
        }

        #endregion

        #region Business

        private async Task<Models.ViewModels.OrderViewModel> GetOrderAsync(int salesOrderID)
        {
            var orderHeader = await _orderRepository.GetSalesOrderHeaderAsync(salesOrderID);

            if (orderHeader != null)
            {
                var orderDetail = await _orderRepository.GetSalesOrderDetailsAsync(orderHeader.SalesOrderID);

                if (orderDetail != null)
                {
                    var customer = await GetCustomerAsync(orderHeader);

                    if (customer != null)
                    {
                        var creditCard = await GetCreditCardAsync(orderHeader.CreditCardID);

                        if (creditCard != null)
                        {
                            var products = await GetProductsAsync(salesOrderID);

                            return new Models.ViewModels.OrderViewModel
                            {
                                SalesOrderID = orderHeader.SalesOrderID,
                                SalesOrderNumber = orderHeader.SalesOrderNumber,
                                PurchaseOrderNumber = orderHeader.PurchaseOrderNumber,
                                OrderDate = orderHeader.OrderDate,
                                ShipDate = orderHeader.ShipDate,
                                IsOnlineOrder = orderHeader.OnlineOrderFlag,
                                SubTotal = orderHeader.SubTotal,
                                TotalDue = orderHeader.TotalDue,
                                Products = products.ToList(),
                                Customer = new Models.ViewModels.CustomerViewModel
                                {
                                    FirstName = customer.FirstName,
                                    MiddleName = customer.MiddleName,
                                    LastName = customer.LastName,
                                    AccountNumber = customer.AccountNumber
                                },
                                CreditCard = new Models.ViewModels.CreditCardViewModel
                                {
                                    CardNumber = creditCard.CardNumber,
                                    ExpMonth = creditCard.ExpMonth,
                                    ExpYear = creditCard.ExpYear
                                }
                            };
                        }
                    }
                }
                else
                    System.Console.WriteLine($"The order {salesOrderID} does not have details.");
            }
            else
                System.Console.WriteLine($"The order {salesOrderID} does not exists.");

            return null;
        }

        private async Task<Models.ViewModels.CustomerViewModel> GetCustomerAsync(Models.Entities.SalesOrderHeader orderHeader)
        {
            var customer = await _orderRepository.GetCustomerAsync(orderHeader.CustomerID);

            if (customer != null)
            {
                var person = await _orderRepository.GetPersonAsync(customer.PersonID);

                if (person != null)
                {
                    return new Models.ViewModels.CustomerViewModel
                    {
                        FirstName = person.FirstName,
                        MiddleName = person.MiddleName,
                        LastName = person.MiddleName,
                        AccountNumber = customer.AccountNumber
                    };
                }
                else
                    System.Console.WriteLine($"The isn't any person's data referred to the customer {customer.PersonID}.");
            }
            else
                System.Console.WriteLine($"The order {orderHeader.SalesOrderID} does not have a customer.");

            return null;
        }

        private async Task<Models.ViewModels.CreditCardViewModel> GetCreditCardAsync(int creditCardID)
        {
            var creditcard = await _orderRepository.GetCreditCardAsync(creditCardID);

            if (creditcard == null)
            {
                System.Console.WriteLine($"The ID {creditCardID} does not correspond to any credit card.");

                return null;
            }
            else
            {
                return new Models.ViewModels.CreditCardViewModel
                {
                    CardNumber = long.Parse(creditcard.CardNumber),
                    ExpMonth = creditcard.ExpMonth,
                    ExpYear = creditcard.ExpYear
                };
            }
        }

        private async Task<System.Collections.Generic.IEnumerable<Models.ViewModels.ProductViewModel>> GetProductsAsync(int salesOrderID)
        {
            var product = await _orderRepository.GetProductsAsync(salesOrderID);

            if (product == null)
            {
                System.Console.WriteLine($"The order {salesOrderID} does not have any products.");

                return null;
            }
            else
            {
                return product.Select(s => new Models.ViewModels.ProductViewModel
                {
                    ProductID = s.ProductID,
                    Name = s.Name,
                    ProductNumber = s.ProductNumber,
                    Color = s.Color,
                    StandardCost = s.StandardCost,
                    ListPrice = s.ListPrice,
                    Size = s.Size,
                    Weight = s.Weight
                });
            }
        }

        #endregion

        #region InstantiatingReturn

        private static RepeatedField<Product> InstantializeProductsReturn(IEnumerable<Models.ViewModels.ProductViewModel> products, out RepeatedField<Product> productsReturn)
        {
            var productReturn = new RepeatedField<Product>();

            foreach (var product in products) productReturn.Add(new Product
            {
                ProductId = product.ProductID,
                Name = string.IsNullOrEmpty(product.Name) ? string.Empty : product.Name,
                ProductNumber = string.IsNullOrEmpty(product.ProductNumber) ? string.Empty : product.ProductNumber,
                Color = string.IsNullOrEmpty(product.Color) ? string.Empty : product.Color,
                StandardCost = product.StandardCost,
                ListPrice = product.ListPrice,
                Size = string.IsNullOrEmpty(product.Size) ? string.Empty : product.Size,
                Weigh = product.Weight
            });

            return productsReturn = productReturn;
        }

        private static Customer InstantializeCustomerReturn(Models.ViewModels.CustomerViewModel customer, out Customer customerReturn)
        {
            return customerReturn = new Customer
            {
                FirstName = string.IsNullOrEmpty(customer.FirstName) ? string.Empty : customer.FirstName,
                MiddleName = string.IsNullOrEmpty(customer.MiddleName) ? string.Empty : customer.MiddleName,
                LastName = string.IsNullOrEmpty(customer.LastName) ? string.Empty : customer.LastName,
                AccountNumber = string.IsNullOrEmpty(customer.AccountNumber) ? string.Empty : customer.AccountNumber
            };
        }

        private static CreditCard InstantializeCreditCardReturn(Models.ViewModels.CreditCardViewModel creditCard, out CreditCard creditCardReturn)
        {
            return creditCardReturn = new CreditCard
            {
                CardNumber = creditCard.CardNumber,
                ExpMonth = creditCard.ExpMonth,
                ExpYear = creditCard.ExpYear
            };
        }

        #endregion
    }
}
