namespace Server.Models.Entities
{
    public class CreditCard
    {
        public int CreditCardID { get; set; }

        public string CardType { get; set; }

        public string CardNumber { get; set; }

        public int ExpMonth { get; set; }

        public int ExpYear { get; set; }

        public System.DateTime ModifiedDate { get; set; }
    }
}
