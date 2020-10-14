﻿using MicroServices.WebDebts.Domain.Models.Enum;
using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class DebtsAppModel
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfInstallments { get; set; }
        public DebtType DebtType { get; set; }
    }
}
