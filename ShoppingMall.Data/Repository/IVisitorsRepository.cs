namespace ShoppingMall.Data
{
    public interface IVisitorsRepository
    {
        Task<int> GetCurrentVisitorCountAsync();
        Task<Visitor> RegisterEntryAsync(Gateway gate);
        Task<Visitor?> RegisterExitAsync(Gateway gate);
        Task<List<Visitor>> GetVisitorByDateAsync(DateTime date);
    }
}