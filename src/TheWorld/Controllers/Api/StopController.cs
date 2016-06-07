using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Controllers.Api
{
    using System.Net;

    using AutoMapper;

    using Microsoft.AspNet.Authorization;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.Logging;

    using TheWorld.Models;
    using TheWorld.Services;
    using TheWorld.ViewModels;

    [Authorize]
    [Route("api/trips/{tripName}/stops")]
    public class StopController : Controller
    {
        private IWorldRepository _repository;

        private ILogger<StopController> _logger;

        private CoordService _coordService;

        public StopController(IWorldRepository repository, ILogger<StopController> logger, CoordService coordService)
        {
            _repository = repository;
            _logger = logger;
            _coordService = coordService;
        }

        [HttpGet("")]
        public JsonResult Get(string tripName)
        {
            try
            {
                var results = this._repository.GetTripByName(tripName, User.Identity.Name);
                if (results == null)
                {
                    return Json(null);
                }
                return Json(Mapper.Map<IEnumerable<StopViewModel>>(results.Stops.OrderBy(s => s.Order)));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get stops for trip {tripName}", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error occurred finding trip name.");
            }
        }

        public async Task<JsonResult> Post(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Map to the Entitiy 
                    var newStop = Mapper.Map<Stop>(vm);
                    // Looking up Geocoordinates
                    var coordResult = await _coordService.LookUp(newStop.Name);

                    if (!coordResult.Success)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        Json(coordResult.Message);
                    }
                    newStop.Latitude = coordResult.Latitude;
                    newStop.Longitude = coordResult.Longitude; 

                    // Save to the Database
                    _repository.AddStop(tripName, User.Identity.Name, newStop);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<StopViewModel>(newStop));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save new stops", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Failed to save new stop.");
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on new stop.");
        }
    }
}
