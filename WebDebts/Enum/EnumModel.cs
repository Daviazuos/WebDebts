using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServices.WebDebts.Domain.Enum
{
    public enum Status
    {
        Paid = 0,
        NotPaid = 1
    }

    public enum DebtStatus
    {
        Open = 0,
        Closed = 1
    }

    public enum DebtType
    {
        Installment = 0,
        Fixed = 1,
        Simple = 2
    }
}
