using Newtonsoft.Json;
using Nik.MineSweeping.Models;
using System.Web.Mvc;

namespace Nik.MineSweeping.Controllers
{
    /// <summary>
    /// https://github.com/xxx407410849/MinesSweeper
    /// </summary>
    public class MineController : Controller
    {
        // GET: Mine
        public ActionResult Index()
        {
            //var mine = new Mine(5, 5, 10);
            //mine.Start().Click(UserAction.LeftClick, new MineCell
            //{
            //    X = 3,
            //    Y = 6
            //});
            //ViewBag.Mine = JsonConvert.SerializeObject(mine.Map);
            return View();
        }

        public ActionResult Manage()
        {
            //var mine = new Mine(5, 5, 10);
            //mine.Start().Click(UserAction.LeftClick, new MineCell
            //{
            //    X = 3,
            //    Y = 6
            //});
            //ViewBag.Mine = JsonConvert.SerializeObject(mine.Map);
            return View();
        }
    }
}