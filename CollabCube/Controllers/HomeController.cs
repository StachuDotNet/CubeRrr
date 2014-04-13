using CollabCube.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollabCube.Controllers
{
    public class HomeController : Controller
    {
        CubeContext dbContext;
        public HomeController()
        {
            this.dbContext = new CubeContext();
        }

        public ActionResult Index()
        {
            var model = dbContext.Rooms.Include("Solves").Include("Solves.MovesMade").ToList();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Room(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index");
            }

            if (!dbContext.Rooms.Any(r => r.Name == name))
            {
                var newSolve = new Solve{ IsSolved = false, MovesMade = new List<Move>(), Scramble = "R U R' U'"};
                var newRoom = new Room { Name = name, Solves = new List<Solve>(){newSolve} };
                dbContext.Rooms.Add(newRoom);
                dbContext.Solves.Add(newSolve);
                dbContext.SaveChanges();
            }

            var asdf = dbContext.Rooms.Include("Solves").Include("Solves.MovesMade").ToList();

            var s1 = dbContext.Rooms
                .First(r => r.Name == name);

            var s2 = s1.Solves.FirstOrDefault(s => !s.IsSolved);

            var s3 = s2.MovesMade;

            ViewBag.InitialMoves = s2.Scramble + " " + String.Join(" ", s3.Select(m => m.MoveString));

            //var test = dbContext.

            //ViewBag.movesToDo = dbContext.Rooms.Solves(sb)

            return View("Room", model: name);
        }
    }
}