namespace Kalendario.Core.Entities
{
    public class Customer : AccountEntity
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Warning { get; set; }
    }
}