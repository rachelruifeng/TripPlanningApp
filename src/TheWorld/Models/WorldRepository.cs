using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    using Microsoft.Data.Entity;
    using Microsoft.Extensions.Logging;

    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;

        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger; 
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            try
            {
                return this._context.Trips.OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                this._logger.LogError("Could not get trips from database", ex);
                return null;
            }
        }

        public IEnumerable<Trip> GettAllTripsWithStops()
        {
            try
            {
                return this._context.Trips.Include(t => t.Stops).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                this._logger.LogError("Could not get trips with stops from database", ex);
                return null;
            }
        }
    }
}
