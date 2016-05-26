using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Controllers.Api
{
    using System.Net;

    using AutoMapper;

    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.Logging;

    using TheWorld.Models;
    using TheWorld.ViewModels;

    [Route("api/trips")]
    public class TripController : Controller
    {
        private IWorldRepository _repository;

        private ILogger<TripController> _logger;

        public TripController(IWorldRepository repository, ILogger<TripController> logger)
        {
            _repository = repository;
            _logger = logger; 
        }

        [HttpGet("")]
        public JsonResult Get()
        {
            var result = Mapper.Map<IEnumerable<TripViewModel>>(this._repository.GettAllTripsWithStops());
            return Json(result);
        }

        [HttpPost("")]
        public JsonResult Post([FromBody]TripViewModel newTripViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTrip = Mapper.Map<Trip>(newTripViewModel);

                    // Save to the Database 
                    this._logger.LogInformation("Attempting to save a new trip");
                    this._repository.AddTrip(newTrip);

                    if (this._repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<TripViewModel>(newTrip));
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("Failed to save new trip", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState});
        }
    }
}
