using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ShoppingMall.Data
{
    public class VisitorsRepository : IVisitorsRepository
    {
        private readonly VisitorsContext _context;
        private readonly ILogger<VisitorsRepository> _logger;

        public VisitorsRepository(VisitorsContext context, ILogger<VisitorsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> GetCurrentVisitorCountAsync()
        {
            try
            {
                return await _context.Visitors
                .Where(v => v.ExitTime == null)
                .CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error get visitors. Error: {ex.Message}", ex.Message);
                throw new Exception($"Error get visitors. Error: {ex.Message}");                
            }

            
        }

        public async Task<Visitor> RegisterEntryAsync(Gateway gate)
        {
            var visitor = new Visitor
            {
                Id = Guid.NewGuid(),
                EntryTime = DateTime.UtcNow,
                EntryGate = gate
            };
            try
            {
                await _context.Visitors.AddAsync(visitor);
                await _context.SaveChangesAsync();
                return visitor;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error register entry. Error: {ex.Message}", ex.Message);
                throw new Exception($"Error register entry. Error: {ex.Message}");
            }            
        }

        public async Task<Visitor?> RegisterExitAsync(Gateway gate)
        {
            var visitor = await _context.Visitors
                .Where(v => v.ExitTime == null)
                .OrderBy(v => v.EntryTime)
                .FirstOrDefaultAsync();

            if (visitor == null)
                return visitor;

            visitor.ExitTime = DateTime.UtcNow;
            visitor.ExitGate = gate;

            try
            {
                await _context.SaveChangesAsync();
                return visitor;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error register exit. Error: {ex.Message}", ex.Message);
                throw new Exception($"Error register exit. Error: {ex.Message}");
            }            
        }

        public async Task<List<Visitor>> GetVisitorByDateAsync(DateTime date)
        {
            var dayVisitors = await _context.Visitors
                    .Where(v => v.EntryTime.Date == date || (v.ExitTime.HasValue && v.ExitTime.Value.Date == date))
                    .ToListAsync(); 

            return dayVisitors;
        }
    }
}
