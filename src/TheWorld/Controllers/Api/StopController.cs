using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Controllers.Api
{
    using System.Net;

    using AutoMapper;

    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.Logging;

    using TheWorld.Models;
    using TheWorld.ViewModels;

    [Route("api/trips/{tripName}/stops")]
    public class StopController :Controller
    {
        private IWorldRepository _repository;

        private ILogger _logger;

        public StopController(IWorldRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger; 
        }

        [HttpGet("")]
        public JsonResult Get(string tripName)
        {
            try
            {
                var results = this._repository.GetTripByName(tripName);
                if (results == null)
                {
                    return Json(null); 
                }
                return Json(Mapper.Map<IEnumerable<StopViewModel>>(results.Stops));
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Failed to get stops for trip {tripName}", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error occurred finding trip name.");
            }
        }
    }
}
