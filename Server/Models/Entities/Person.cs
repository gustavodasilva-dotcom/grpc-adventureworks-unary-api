namespace Server.Models.Entities
{
    public class Person
    {
        public int BusinessEntityID { get; set; }

        public string PersonType { get; set; }

        public int NameStyle { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Suffix { get; set; }

        public int EmailPromotion { get; set; }

        public string AdditionalContactInfo { get; set; }

        public string Demographics { get; set; }

        public System.Guid rowguid { get; set; }

        public System.DateTime ModifiedDate { get; set; }
    }
}
