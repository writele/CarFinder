using System.Collections.Generic;
using System.Threading.Tasks;
using CarFinder.Models;

namespace CarFinder.Controllers
{
    internal class carViewModel
    {
        public List<Cars> Car { get; set; }
        public string Image { get; set; }
        public string Recalls { get; set; }
    }
}