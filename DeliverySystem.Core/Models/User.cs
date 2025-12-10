namespace DeliverySystem.Core.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public int? RelatedEntityId { get; set; }
    }
}