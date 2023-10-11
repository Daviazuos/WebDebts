
namespace MicroServices.WebDebts.Domain.Models
{
    public class User : ModelBase
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Document { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
