namespace ShoppingMall.Business
{
    public class VisitorStatisticsModel
    {
        public DateTime Date { get; set; }
        public Dictionary<string, int> Visitors { get; set; } = new();
    }
}
