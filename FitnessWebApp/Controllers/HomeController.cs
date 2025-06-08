using FitnessWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnessWebApp.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            try
            {
                // Lấy danh sách bài tập mới nhất
                var latestExercises = db.Exercises
                    .OrderByDescending(e => e.ExerciseId)
                    .Take(6)
                    .ToList();

                // Lấy bài viết blog mới nhất
                var latestPosts = db.BlogPosts
                    .Include(p => p.User)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(3)
                    .ToList();

                ViewBag.LatestExercises = latestExercises;
                ViewBag.LatestPosts = latestPosts;

                return View();
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tải trang chủ");
                return View();
            }
        }

        public ActionResult TrainingPlans()
        {
            try
            {
                // Lấy danh sách lộ trình tập luyện
                var trainingPlans = db.TrainingPlans
                    .Include(tp => tp.User)
                    .OrderByDescending(tp => tp.CreatedAt)
                    .ToList();

                return View(trainingPlans);
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tải danh sách lộ trình tập luyện");
                return View(new List<TrainingPlan>());
            }
        }

        public ActionResult Exercises()
        {
            try
            {
                // Lấy danh sách bài tập
                var exercises = db.Exercises
                    .OrderBy(e => e.Name)
                    .ToList();

                return View(exercises);
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tải danh sách bài tập");
                return View(new List<Exercise>());
            }
        }

        public ActionResult ExerciseDetails(int id)
        {
            try
            {
                var exercise = db.Exercises
                    .Include(e => e.TrainingDays)
                    .FirstOrDefault(e => e.ExerciseId == id);

                if (exercise == null)
                {
                    return HttpNotFound();
                }

                return View(exercise);
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tải chi tiết bài tập");
                return RedirectToAction("Exercises");
            }
        }

        public ActionResult Blog()
        {
            try
            {
                // Lấy danh sách bài viết blog
                var posts = db.BlogPosts
                    .Include(p => p.User)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToList();

                return View(posts);
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tải danh sách bài viết");
                return View(new List<BlogPost>());
            }
        }

        public ActionResult CalculateBMI()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                int userId = (int)Session["UserId"];
                var user = db.Users.Find(userId);

                if (user == null)
                {
                    return HttpNotFound();
                }

                if (user.Height == null || user.Weight == null)
                {
                    return RedirectToAction("UpdateProfile", "Account");
                }

                decimal height = (decimal)user.Height / 100; // Chuyển cm sang m
                decimal weight = (decimal)user.Weight;
                decimal bmi = weight / (height * height);

                string category = "";
                if (bmi < 18.5m)
                {
                    category = "Thiếu cân";
                }
                else if (bmi < 25m)
                {
                    category = "Bình thường";
                }
                else if (bmi < 30m)
                {
                    category = "Thừa cân";
                }
                else
                {
                    category = "Béo phì";
                }

                ViewBag.BMI = bmi.ToString("F1");
                ViewBag.Category = category;

                return View();
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tính chỉ số BMI");
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}