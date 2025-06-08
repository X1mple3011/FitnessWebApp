using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnessWebApp.Models;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity;

namespace FitnessWebApp.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Account/Login
        public ActionResult Login()
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                return View();
            }
            catch (Exception)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi truy cập trang đăng nhập");
                return View();
            }
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin");
                    return View();
                }

                // Mã hóa password
                string hashedPassword = HashPassword(password);

                var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

                if (user != null)
                {
                    Session["UserId"] = user.UserId;
                    Session["Username"] = user.Username;
                    Session["FullName"] = user.FullName;
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                return View();
            }
            catch (Exception)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi đăng nhập");
                return View();
            }
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                return View();
            }
            catch (Exception)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi truy cập trang đăng ký");
                return View();
            }
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            // Kiểm tra username đã tồn tại chưa
                            if (db.Users.Any(u => u.Username == user.Username))
                            {
                                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                                return View(user);
                            }

                            // Mã hóa password
                            user.Password = HashPassword(user.Password);

                            db.Users.Add(user);
                            db.SaveChanges();
                            transaction.Commit();

                            // Tự động đăng nhập sau khi đăng ký
                            Session["UserId"] = user.UserId;
                            Session["Username"] = user.Username;
                            Session["FullName"] = user.FullName;

                            return RedirectToAction("Index", "Home");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                            return View(user);
                        }
                    }
                }
                // Nếu ModelState không hợp lệ, trả về view với lỗi chi tiết
                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                return View(user);
            }
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            try
            {
                Session.Clear();
                Session.Abandon();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                // Log error
                return RedirectToAction("Index", "Home");
            }
        }

        // Hàm mã hóa password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
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