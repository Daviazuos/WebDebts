using System;

namespace MicroServices.WebDebts.Domain.Models
{
    public class ResponsibleParty : ModelBase
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public User User { get; set; }
    }
}
