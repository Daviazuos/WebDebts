﻿using System;

namespace MicroServices.WebDebts.Domain.Models
{
    public class BaseDebt
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
