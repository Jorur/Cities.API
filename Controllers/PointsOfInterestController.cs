using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            try
            {
                throw new Exception("Exception example");
                var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    _logger.LogInformation($"City with id: {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }
                return Ok(city.PointsOfInterest);
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id: {cityId}.", e);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            
            var pointOfInterest = city.PointsOfInterest
                    .FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterest == null)    
            {
                return NotFound();
            }
            return Ok(pointOfInterest);
        }
        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointofInterest(int cityId, PointsOfInterestForCreationDto pointsOfInterest)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            //demo purposes - to be improved
            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
                c => c.PointsOfInterest).Max(p => p.Id);
            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointsOfInterest.Name,
                Description = pointsOfInterest.Description
            };
            city.PointsOfInterest.Add(finalPointOfInterest);
            
            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId = cityId,
                pointOfInterestId = finalPointOfInterest.Id
            }, finalPointOfInterest);
        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointsOfInterestForUpdateDto pointsOfInterest)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            
            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }
            
            pointOfInterestFromStore.Name = pointsOfInterest.Name;
            pointOfInterestFromStore.Description = pointsOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
            JsonPatchDocument<PointsOfInterestForUpdateDto> patchDocument)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if(city == null)
            {
                return NotFound();
            }
            
            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointsOfInterestForUpdateDto()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };
            
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }
            
            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
           var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
           if (city == null)
           {
               return NotFound();
           }
           
           var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
           
           if(pointOfInterestFromStore == null)
           {
               return NotFound();
           }

           city.PointsOfInterest.Remove(pointOfInterestFromStore);
           return NoContent();
        }
    }
}
