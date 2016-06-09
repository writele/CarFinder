using CarFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CarFinder.Controllers
{
    [RoutePrefix("api/Cars")]
    public class CarController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //[Route("Years")]
        public async Task<List<string>> GetYears()
        {
            return await db.GetYears();
        }

        /// <summary>
        /// Get all car makes according to a specified year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<List<string>> GetMakes(string year)
        {
            return await db.GetMakes(year);
        }

        // localhost/api/Car?year=2014&make=BMW
        public async Task<List<string>> GetModels(string year, string make)
        {
            return await db.GetModels(year, make);
        }

        // localhost/api/car?year=2014&Make=Kia&Model=Soul
        public async Task<List<string>> GetTrims(string year, string make, string model)
        {
            return await db.GetTrims(year, make, model);
        }

        // localhost/api/car?year=2014&Make=Kia&Model=Soul&Trim=4dr%20Wagon%20(1.6L%204cyl%206A)
        public async Task<List<Cars>> GetCarsByYearMakeModelTrim(string year, string make, string model, string trim)
        {
            return await db.GetCarsByYearMakeModelTrim(year, make, model, trim);
        }


    }
}
