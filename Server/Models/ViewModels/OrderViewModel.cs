namespace Server.Models.ViewModels
{
    public class OrderViewModel
    {
		public int SalesOrderID { get; set; }

		public string SalesOrderNumber { get; set; }

		public string PurchaseOrderNumber { get; set; }

		public System.DateTime OrderDate { get; set; }

		public System.DateTime ShipDate { get; set; }
		
		public bool IsOnlineOrder { get; set; }

		public double SubTotal { get; set; }

		public double TotalDue { get; set; }

		public System.Collections.Generic.List<ProductViewModel> Products { get; set; } = new System.Collections.Generic.List<ProductViewModel>();
		
		public CustomerViewModel Customer { get; set; }

		public CreditCardViewModel CreditCard { get; set; }
	}
}
