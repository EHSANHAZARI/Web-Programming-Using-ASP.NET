using F2021A6MH.Models;
using System.Web.Mvc;

namespace F2021A6MH.Controllers
{
    [Authorize()]
    public class LoadDataController : Controller
    {
        // Reference to the manager object
        Manager m = new Manager();

        // GET: LoadData
        public ActionResult Index()
        {
            var result = new LoadDataResponseViewModel();
            return View(result);
        }

        public ActionResult LoadInitialData()
        {
            var result = new LoadDataResponseViewModel();

            if (m.LoadData())
            {
                result.Success = true;
                result.Message = "data has been loaded";
            }
            else
            {
                result.Success = false;
                result.Message = "data exists already";
            }

            return View(nameof(Index), result);
        }

        public ActionResult Remove()
        {
            var result = new LoadDataResponseViewModel();
            if (m.RemoveData())
            {
                result.Success = true;
                result.Message = "data has been removed";
            }
            else
            {
                result.Success = false;
                result.Message = "could not remove data";
            }

            return View(nameof(Index), result);
        }

        public ActionResult RemoveDatabase()
        {
            if (m.RemoveDatabase())
            {
                return Content("database has been removed");
            }
            else
            {
                return Content("could not remove database");
            }
        }
    }
}