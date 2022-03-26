namespace Server.Models.Entities
{
    public class Product
    {
		public int ProductID { get; set; }

        public string Name { get; set; }

        public string ProductNumber { get; set; }

        public bool MakeFlag { get; set; }

        public bool FinishedGoodsFlag { get; set; }

        public string Color { get; set; }

        public int SafetyStockLevel { get; set; }

        public int ReorderPoint { get; set; }

        public double StandardCost { get; set; }

        public double ListPrice { get; set; }

        public string Size { get; set; }

        public string SizeUnitMeasureCode { get; set; }

        public string WeightUnitMeasureCode { get; set; }

        public double Weight { get; set; }

        public int DaysToManufacture { get; set; }

        public string ProductLine { get; set; }

        public string Class { get; set; }

        public string Style { get; set; }

        public int ProductSubcategoryID { get; set; }

        public int ProductModelID { get; set; }

        public System.DateTime SellStartDate { get; set; }

        public System.DateTime SellEndDate { get; set; }

        public System.DateTime DiscountinuedDate { get; set; }

        public System.Guid rowguid { get; set; }

        public System.DateTime ModifiedDate { get; set; }
    }
}
