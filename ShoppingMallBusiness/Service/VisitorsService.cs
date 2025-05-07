using Microsoft.Extensions.Logging;
using ShoppingMall.Business;
using ShoppingMall.Data;

namespace ShoppingMallBusiness
{
    public class VisitorsService : IVisitorsService
    {
        private readonly IVisitorsRepository _repository;
        private readonly ILogger<VisitorsService> _logger;

        public VisitorsService(IVisitorsRepository repository, ILogger<VisitorsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<int> GetCurrentVisitorCountAsync()
        {
            return await _repository.GetCurrentVisitorCountAsync();
        }

        public async Task<GatewayLogModel> RegisterEntryAsync(Gateway gate)
        {
            await _repository.RegisterEntryAsync(gate);
            var count = await _repository.GetCurrentVisitorCountAsync();

            var log = new GatewayLogModel { Gateway = gate, TotalVisitors = count };
            _logger.LogInformation("Visitor entered through {Gateway}. Total visitors: {TotalVisitors}",
                gate, count);

            return log;
        }

        public async Task<GatewayLogModel?> RegisterExitAsync(Gateway gate)
        {
            var visitor = await _repository.RegisterExitAsync(gate);
            if (visitor == null) return null;

            var count = await _repository.GetCurrentVisitorCountAsync();

            var log = new GatewayLogModel { Gateway = gate, TotalVisitors = count };
            _logger.LogInformation("Visitor exited through {Gateway}. Total visitors: {TotalVisitors}",
                gate, count);

            return log;
        }

        public async Task<List<VisitorStatisticsModel>> GetVisitorStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            var result = new List<VisitorStatisticsModel>();

            startDate = DateTime.SpecifyKind(startDate.Date, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate.Date, DateTimeKind.Utc);

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var dayVisitors = await _repository.GetVisitorByDateAsync(date);

                var visitors = new Dictionary<string, int>();

                for (int hour = 0; hour < 24; hour++)
                {
                    var hourKey = $"{hour:00}:00";
                    var hourStart = date.AddHours(hour);
                    var hourEnd = hourStart.AddHours(1);

                    var count = dayVisitors.Count(v =>
                        v.EntryTime < hourEnd &&
                        (!v.ExitTime.HasValue || v.ExitTime >= hourStart));

                    visitors[hourKey] = count;
                }

                result.Add(new VisitorStatisticsModel
                {
                    Date = date,
                    Visitors = visitors
                });
            }

            return result;
        }
    }
}