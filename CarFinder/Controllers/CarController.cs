using CarFinder.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CarFinder.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Car")]
    public class CarController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        ////////////// CALLING SQL STORED PROCEDURES /////////////

        /// <summary>
        /// Get list of all years in car database.
        /// </summary>
        /// <returns></returns>
        [Route("Years")]
        public async Task<List<string>> GetYears()
        {
            return await db.GetYears();
        }

        /// <summary>
        /// Get all car makes according to a specified year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        /// 
        [Route("{year}/Makes")]
        public async Task<List<string>> GetMakes(string year)
        {
            return await db.GetMakes(year);
        }

        /// <summary>
        /// Get all models for a specified year and car make.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <returns></returns>
        [Route("{year}/{make}/Models")]
        public async Task<List<string>> GetModels(string year, string make)
        {
            return await db.GetModels(year, make);
        }

        /// <summary>
        /// Get all trims for a specified car according to year, make, and model.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("{year}/{make}/{model}/Trims")]
        public async Task<List<string>> GetTrims(string year, string make, string model)
        {
            return await db.GetTrims(year, make, model);
        }

        /// <summary>
        /// Get all details on specified car according to year, make, model, and trim.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="trim"></param>
        /// <returns></returns>
        public async Task<List<Cars>> GetCarByYearMakeModelTrim(string year, string make, string model, string trim)
        {
            return await db.GetCarByYearMakeModelTrim(year, make, model, trim);
        }

        /// <summary>
        /// Get all cars according to specified year, make, and model.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<Cars>> GetCarsByYearMakeModel(string year, string make, string model)
        {
            return await db.GetCarsByYearMakeModel(year, make, model);
        }

        /// <summary>
        /// Get all cars according to specified year and make.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <returns></returns>
        public async Task<List<Cars>> GetCarsByYearMake(string year, string make)
        {
            return await db.GetCarsByYearMake(year, make);
        }

        /// <summary>
        /// Get all cars for a given year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<List<Cars>> GetCarsByYear(string year)
        {
            return await db.GetCarsByYear(year);
        }


        ///////////// RETURNING DATA TO VIEW ///////////////
        // api/Car/getCar?year=2014&make=Kia&Model=Soul&Trim=4dr%20Wagon%20(1.6L%204cyl%206A)
        /// <summary>
        /// Get all car details, image, and recall info for given year, make, model, and trim.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="trim"></param>
        /// <returns></returns>
        [Route("getCar")]
        public async Task<IHttpActionResult> getCarData(string year = "", string make = "", string model = "", string trim = "")
        {
            HttpResponseMessage response;
            var content = "";

            //This is a call to "your" personal API to get a car. 
            //You may need to change the method name  
            var singleCar = await GetCarByYearMakeModelTrim(year, make, model, trim);
            var car = new carViewModel
            {
                Car = singleCar,
                Recalls = content,
                Image = ""

            };

            //Get recall Data

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.nhtsa.gov/");
                try
                {
                    response = await client.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + year + "/make/"
                        + make + "/model/" + model + "?format=json");
                    content = await response.Content.ReadAsStringAsync();

                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }

            car.Recalls = content;


            //////////////////////////////   My Bing Search   //////////////////////////////////////////////////////////

            string query = year + " " + make + " " + model + " " + trim;

            string rootUri = "https://api.datamarket.azure.com/Bing/Search";

            var bingContainer = new Bing.BingSearchContainer(new Uri(rootUri));

            var accountKey = ConfigurationManager.AppSettings["searchKey"]; ;

            bingContainer.Credentials = new NetworkCredential(accountKey, accountKey);


            var imageQuery = bingContainer.Image(query, null, null, null, null, null, null);

            var imageResults = imageQuery.Execute().ToList();


            car.Image = imageResults.First().MediaUrl;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            return Ok(car);

        }

    }
}
