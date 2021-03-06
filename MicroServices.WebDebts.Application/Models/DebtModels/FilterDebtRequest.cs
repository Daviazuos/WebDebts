﻿using System;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Models
{
    public class FilterDebtRequest
    {
        public string Name { get; set; }
        public decimal? Value { get; set; }
        public DateTime? Date { get; set; }
        public DebtInstallmentTypeApp? DebtInstallmentType { get; set; }
        public DebtTypeApp? DebtType { get; set; }
    }
}
