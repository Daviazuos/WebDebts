using System;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Models.DebtModels
{
    public class AddDebtFromAppRequest
    {
        public string notification { get; set; }
    }
}
