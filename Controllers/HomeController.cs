using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ActivityPlanner.Models;

namespace ActivityPlanner.Controllers
{
    public class HomeController : Controller
    {
        private int? UserSession
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }
        
        private MyContext dbContext;

        public HomeController(MyContext context) { dbContext = context; }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.AllUsers.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use!");
                    return View("Index");
                }
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                newUser.Password = hasher.HashPassword(newUser, newUser.Password);
                dbContext.Add(newUser);
                dbContext.SaveChanges();
                UserSession = newUser.UserId;
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser currUser)
        {
            if (ModelState.IsValid)
            {
                User existingUser = dbContext.AllUsers.FirstOrDefault(u => u.Email == currUser.Email);
                if (existingUser == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }
                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(currUser, existingUser.Password, currUser.Password);
                if (result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }
                UserSession = existingUser.UserId;
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            if (UserSession == null) 
            { 
                return RedirectToAction("Index"); 
            }
            ViewBag.User = dbContext.GetUserById((int)UserSession);
            ViewBag.Activities = dbContext.AllActivities
                .Include(x => x.Planner)
                .Include(y => y.Participants)
                // .ThenInclude(z => z.Participant)
                .OrderByDescending(z => z.Date)
                .ToList();
            return View();
        }
    
        [HttpGet("{activityId}")]
        public IActionResult Show(int activityId)
        {
            if (UserSession == null) { return RedirectToAction("Index"); }
            ViewBag.User = (int)UserSession;
            ViewBag.Users = dbContext.GetUserById(activityId);
            
            ActivityEvent Event = dbContext.AllActivities
                .Include(x => x.Planner)
                .Include(y => y.Participants)
                .ThenInclude(z => z.Participant)
                .FirstOrDefault(w => w.ActivityId == activityId);
            ViewBag.ActivityEvent = Event;
            // ViewBag.User = User;
            return View();
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(ActivityEvent newActivity)
        {
            if (UserSession == null) { return RedirectToAction("Index"); }
            if (ModelState.IsValid)
            {   
                Join newJoin = new Join();
                newActivity.PlannerId = (int)UserSession;
                dbContext.Add(newActivity);
                dbContext.SaveChanges();
                return Redirect($"{newActivity.ActivityId}");
            }
            return View("New");
        }

        [HttpGet("delete/{activityId}")]
        public IActionResult Delete(int activityId)
        {
            dbContext.Remove(activityId);
            return RedirectToAction("Dashboard");
        }

        [HttpGet("join/{activityId}")]
        public IActionResult Attend(int activityId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            Join newJoin = new Join();
            newJoin.ActivityId = activityId;
            newJoin.UserId = (int)UserId;
            dbContext.Joiners.Add(newJoin);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("leave/{activityId}")]
        public IActionResult Leave(int activityId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            dbContext.Remove(activityId, (int)UserId);
            return RedirectToAction("Dashboard");
        }
    }
}

