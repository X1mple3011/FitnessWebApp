using System;
using System.Linq;
using System.Web.Mvc;
using FitnessWebApp.Models;
using System.Data.Entity;
using System.Collections.Generic;

namespace FitnessWebApp.Controllers
{
    public class ExerciseController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Exercise
        public ActionResult Index()
        {
            try
            {
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

        // GET: Exercise/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var exercise = db.Exercises
                    .Include(e => e.TrainingDays)
                    .FirstOrDefault(e => e.ExerciseId == id);

                if (exercise == null) return HttpNotFound();
                return View(exercise);
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tải chi tiết bài tập");
                return RedirectToAction("Index");
            }
        }

        // GET: Exercise/Create
        public ActionResult Create()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                return View();
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi truy cập trang tạo bài tập");
                return RedirectToAction("Index");
            }
        }

        // POST: Exercise/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Exercise exercise)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            // Kiểm tra tên bài tập đã tồn tại chưa
                            if (db.Exercises.Any(e => e.Name == exercise.Name))
                            {
                                ModelState.AddModelError("Name", "Tên bài tập đã tồn tại");
                                return View(exercise);
                            }

                            db.Exercises.Add(exercise);
                            db.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("Index");
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                return View(exercise);
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo bài tập");
                return View(exercise);
            }
        }

        // GET: Exercise/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var exercise = db.Exercises.Find(id);
                if (exercise == null) return HttpNotFound();
                return View(exercise);
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi truy cập trang chỉnh sửa bài tập");
                return RedirectToAction("Index");
            }
        }

        // POST: Exercise/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Exercise exercise)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            // Kiểm tra tên bài tập đã tồn tại chưa (trừ bài tập hiện tại)
                            if (db.Exercises.Any(e => e.Name == exercise.Name && e.ExerciseId != exercise.ExerciseId))
                            {
                                ModelState.AddModelError("Name", "Tên bài tập đã tồn tại");
                                return View(exercise);
                            }

                            db.Entry(exercise).State = EntityState.Modified;
                            db.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("Index");
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                return View(exercise);
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi chỉnh sửa bài tập");
                return View(exercise);
            }
        }

        // GET: Exercise/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var exercise = db.Exercises.Find(id);
                if (exercise == null) return HttpNotFound();
                return View(exercise);
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi truy cập trang xóa bài tập");
                return RedirectToAction("Index");
            }
        }

        // POST: Exercise/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var exercise = db.Exercises.Find(id);
                        if (exercise != null)
                        {
                            // Kiểm tra xem bài tập có đang được sử dụng trong lộ trình nào không
                            if (db.TrainingDayExercises.Any(tde => tde.ExerciseId == id))
                            {
                                ModelState.AddModelError("", "Không thể xóa bài tập này vì đang được sử dụng trong lộ trình tập luyện");
                                return View("Delete", exercise);
                            }

                            db.Exercises.Remove(exercise);
                            db.SaveChanges();
                            transaction.Commit();
                        }
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi xóa bài tập");
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