using System;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Models
{
    public class FilterDebtsCategoriesRequest
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public Guid? CardId { get; set; }
    }
}
