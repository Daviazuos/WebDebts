namespace MicroServices.WebDebts.Application.Models.CardModels
{
    public class GetMonthlyCardSummariesRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool WithNoDebts { get; set; }
        public bool? IsPaid { get; set; }
    }
}
