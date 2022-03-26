namespace Server.Models.Entities
{
    public class Customer
    {
        public int CustomerID { get; set; }

        public int PersonID { get; set; }

        public int StoreID { get; set; }

        public int TerritoryID { get; set; }

        public string AccountNumber { get; set; }

        public System.Guid rowguid { get; set; }

        public System.DateTime ModifiedDate { get; set; }
    }
}
