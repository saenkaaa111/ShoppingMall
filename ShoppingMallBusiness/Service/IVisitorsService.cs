using ShoppingMall.Business;
using ShoppingMall.Data;

namespace ShoppingMallBusiness
{
    public interface IVisitorsService
    {
        Task<int> GetCurrentVisitorCountAsync();
        Task<GatewayLogModel> RegisterEntryAsync(Gateway gate);
        Task<GatewayLogModel?> RegisterExitAsync(Gateway gate);
        Task<List<VisitorStatisticsModel>> GetVisitorStatisticsAsync(DateTime startDate, DateTime endDate);
    }
}