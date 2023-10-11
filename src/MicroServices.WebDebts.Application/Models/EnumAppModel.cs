using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServices.WebDebts.Application.Models
{
    public class EnumAppModel
    {
        public enum StatusApp
        {
            Paid = 0,
            NotPaid = 1
        }

        public enum DebtStatusApp
        {
            Open = 0,
            Closed = 1
        }

        public enum DebtInstallmentTypeApp
        {
            Installment = 0,
            Fixed = 1,
            Simple = 2
        }

        public enum DebtTypeApp
        {
            Simple = 0,
            Card = 1
        }
    }
}
