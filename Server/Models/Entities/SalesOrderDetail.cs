namespace Server.Models.Entities
{
    public class SalesOrderDetail
    {
        public int SalesOrderID { get; set; }

        public int SalesOrderDetailID { get; set; }

        public string CarrierTrackingNumber { get; set; }

        public int OrderQty { get; set; }

        public int ProductID { get; set; }

        public int SpecialOfferID { get; set; }

        public double UnitPrice { get; set; }

        public double UnitPriceDiscount { get; set; }

        public double LineTotal { get; set; }

        public System.Guid rowgruid { get; set; }

        public System.DateTime ModifiedDate { get; set; }
    }
}
