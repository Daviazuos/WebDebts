using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Models.Commom
{
    public class PaginatedList<T>
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public long TotalItems { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
