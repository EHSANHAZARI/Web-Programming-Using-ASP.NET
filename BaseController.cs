using System.Web.Mvc;

namespace F2021A6MH.Controllers
{
    public class BaseController : Controller
    {
        protected readonly Manager manager;

        public BaseController()
        {
            manager = new Manager();
        }

        protected SelectList GetGenreSelectList() => new SelectList(manager.GenreGetAll(), "Name", "Name");
    }
}
