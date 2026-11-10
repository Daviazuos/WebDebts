using System;

namespace MicroServices.WebDebts.Domain.Models
{
    public class DebtCategoryMatch
    {
        public string DebtName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public double Similarity { get; set; }
    }
}
