namespace Server.Models.ViewModels
{
    public class ProductViewModel
    {
		public int ProductID { get; set; }
		
		public string Name { get; set; }

		public string ProductNumber { get; set; }

		public string Color { get; set; }

		public double StandardCost { get; set; }

		public double ListPrice { get; set; }

		public string Size { get; set; }

		public double Weight { get; set; }
	}
}
