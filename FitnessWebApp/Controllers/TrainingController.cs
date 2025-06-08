using System;
using System.Linq;
using System.Web.Mvc;
using FitnessWebApp.Models;
using System.Data.Entity;
using System.Collections.Generic;

namespace FitnessWebApp.Controllers
{
    public class TrainingController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Training/MyPlan
        public ActionResult MyPlan()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                int userId = (int)Session["UserId"];
                var trainingPlans = db.TrainingPlans
                    .Where(tp => tp.UserId == userId)
                    .OrderByDescending(tp => tp.CreatedAt)
                    .ToList();

                return View(trainingPlans);
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tải kế hoạch tập luyện");
                return View(new List<TrainingPlan>());
            }
        }

        // GET: Training/CreatePlan
        public ActionResult CreatePlan()
        {
            var model = new TrainingPlan();
            // Nếu cần, khởi tạo TrainingDays rỗng
            model.TrainingDays = new List<TrainingDay>();
            return View(model);
        }

        // POST: Training/CreatePlan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePlan(TrainingPlan plan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Tính số ngày tập dựa trên ngày bắt đầu và kết thúc
                    int soNgay = (plan.EndDate - plan.StartDate).Days + 1;
                    if (soNgay <= 0)
                    {
                        ModelState.AddModelError("", "Ngày kết thúc phải sau ngày bắt đầu");
                        return View(plan);
                    }

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            plan.UserId = (int)Session["UserId"];
                            plan.CreatedAt = DateTime.Now;
                            plan.Duration = soNgay;
                            db.TrainingPlans.Add(plan);
                            db.SaveChanges();

                            for (int i = 1; i <= soNgay; i++)
                            {
                                var day = new TrainingDay
                                {
                                    TrainingPlanId = plan.TrainingPlanId,
                                    DayNumber = i
                                };
                                db.TrainingDays.Add(day);
                            }
                            db.SaveChanges();
                            transaction.Commit();

                            return RedirectToAction("PlanDetails", new { id = plan.TrainingPlanId });
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo kế hoạch tập luyện");
            }
            return View(plan);
        }

        // GET: Training/PlanDetails/5
        public ActionResult PlanDetails(int id)
        {
            try
            {
                var plan = db.TrainingPlans
                    .Include("TrainingDays.TrainingDayExercises.Exercise")
                    .Include("TrainingDays")
                    .Include("TrainingDays.TrainingDayExercises")
                    .FirstOrDefault(p => p.TrainingPlanId == id);

                if (plan == null)
                {
                    return HttpNotFound();
                }

                return View(plan);
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tải chi tiết kế hoạch");
                return RedirectToAction("MyPlan");
            }
        }

        // GET: Training/AddExercise/5
        public ActionResult AddExercise(int planId)
        {
            // Chuyển hướng sang chọn ngày tập trước khi thêm bài tập
            var plan = db.TrainingPlans.Include("TrainingDays").FirstOrDefault(p => p.TrainingPlanId == planId);
            if (plan == null || plan.UserId != (int)Session["UserId"]) return new HttpStatusCodeResult(403);
            if (plan.TrainingDays == null || !plan.TrainingDays.Any()) return RedirectToAction("PlanDetails", new { id = planId });
            // Chọn ngày đầu tiên để thêm bài tập (hoặc có thể render view chọn ngày)
            return RedirectToAction("AddExerciseToDay", new { dayId = plan.TrainingDays.First().TrainingDayId, planId = planId });
        }

        // POST: Training/AddExercise
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExercise(TrainingDayExercise trainingDay)
        {
            // Chuyển hướng sang AddExerciseToDay
            return RedirectToAction("AddExerciseToDay", new { dayId = trainingDay.TrainingDayId, planId = trainingDay.TrainingPlanId });
        }

        // GET: Training/DeleteExercise/5
        public ActionResult DeleteExercise(int id)
        {
            var trainingDay = db.TrainingDays.Find(id);
            if (trainingDay == null)
            {
                return HttpNotFound();
            }

            var plan = db.TrainingPlans.Find(trainingDay.TrainingPlanId);
            if (plan.UserId != (int)Session["UserId"])
            {
                return new HttpStatusCodeResult(403);
            }

            db.TrainingDays.Remove(trainingDay);
            db.SaveChanges();

            return RedirectToAction("PlanDetails", new { id = trainingDay.TrainingPlanId });
        }

        public ActionResult DayDetails(int dayId)
        {
            var day = db.TrainingDays
                .Include("TrainingDayExercises.Exercise")
                .FirstOrDefault(d => d.TrainingDayId == dayId);
            if (day == null) return HttpNotFound();
            return View(day);
        }

        [HttpGet]
        public ActionResult AddExerciseToDay(int dayId, int planId)
        {
            ViewBag.Exercises = db.Exercises.ToList();
            return View("AddExercise", new TrainingDayExercise { TrainingDayId = dayId, TrainingPlanId = planId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExerciseToDay(TrainingDayExercise model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingExercise = db.TrainingDayExercises
                        .Any(tde => tde.TrainingDayId == model.TrainingDayId 
                                && tde.ExerciseId == model.ExerciseId);
                                
                    if (existingExercise)
                    {
                        ModelState.AddModelError("", "Bài tập này đã tồn tại trong ngày tập");
                        ViewBag.Exercises = db.Exercises.ToList();
                        return View("AddExercise", model);
                    }

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            // Đảm bảo chỉ gán các trường ID và primitive, KHÔNG gán navigation property
                            var newTde = new TrainingDayExercise
                            {
                                TrainingDayId = model.TrainingDayId,
                                ExerciseId = model.ExerciseId,
                                Sets = model.Sets,
                                Repetitions = model.Repetitions,
                                RestTime = model.RestTime,
                                Notes = model.Notes,
                                TrainingPlanId = model.TrainingPlanId
                            };
                            db.TrainingDayExercises.Add(newTde);
                            db.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("DayDetails", new { dayId = model.TrainingDayId });
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm bài tập");
            }
            ViewBag.Exercises = db.Exercises.ToList();
            return View("AddExercise", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteExerciseFromDay(int id)
        {
            try
            {
                var ex = db.TrainingDayExercises.Find(id);
                if (ex != null)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            int dayId = ex.TrainingDayId;
                            db.TrainingDayExercises.Remove(ex);
                            db.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("DayDetails", new { dayId });
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                // Log error
                return new HttpStatusCodeResult(500, "Có lỗi xảy ra khi xóa bài tập");
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