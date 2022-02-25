﻿using MicroServices.WebDebts.Domain.Models.Enum;
using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetWalletByIdResponse
    {
        public Guid Id {  get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime? FinishAt { get; set; }
        public DateTime StartAt { get; set; }
        public WalletStatus WalletStatus { get; set; }
        public int UpdatedValue { get; set; }
    }
}
