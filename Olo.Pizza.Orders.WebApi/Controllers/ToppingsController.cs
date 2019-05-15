using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Olo.Pizza.Orders.WebApi.Models;
using Olo.Pizza.Orders.WebApi.Services;

namespace Olo.Pizza.Orders.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToppingsController : ControllerBase
    {
        private IPopularityService _popularityService;

        public ToppingsController(IPopularityService popularityService)
        {
            _popularityService = popularityService;
        }
        
        [HttpGet("getpopular/{top?}")]
        public async Task<IEnumerable<PopularityModel>> GetPopular(int top = 20)
        {
            var result = new PopularityModel[] { };

            try
            {
                result = await _popularityService.Query(top);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);

                //NOTE: discuss with business whether to throw 500 or handle differently
            }

            return result;
        }
    }
}
