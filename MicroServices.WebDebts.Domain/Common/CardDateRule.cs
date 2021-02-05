using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Common
{
    public class CardRules
    {
        public static Debt CardDateRule(int ClosureDate, int DueDate, Debt debt)
        {
            var dictDates = CreateClosureAndDueDates(ClosureDate, DueDate, debt.Date);

            foreach (var dates in dictDates)
            {
                if (debt.Date < dates.Key)
                {
                    debt.Date = dates.Value;
                    return debt;
                }
            }
            return debt;
        }

        public static Dictionary<DateTime, DateTime> CreateClosureAndDueDates(int ClosureDate, int DueDate, DateTime BuyDate)
        {
            var completeDueDate = new DateTime(BuyDate.Year, BuyDate.Month, DueDate);
            var completeClosureDate = new DateTime(BuyDate.Year, BuyDate.Month, ClosureDate);

            if (ClosureDate > DueDate)
            {
                completeDueDate = completeDueDate.AddMonths(1);
            }

            var closureDates = new Dictionary<DateTime, DateTime>
            {
                { completeClosureDate.AddMonths(-1), completeDueDate.AddMonths(-1) },
                { completeClosureDate, completeDueDate },
                { completeClosureDate.AddMonths(1), completeDueDate.AddMonths(1) }
                 
            };

            return closureDates;
        }
    }    
}