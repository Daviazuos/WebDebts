using System;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetCategoryRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
