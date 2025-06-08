using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnessWebApp.Models;
using System.Data.Entity;

namespace FitnessWebApp.Controllers
{
    public class BlogController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Blog
        public ActionResult Index()
        {
            try
            {
                var posts = db.BlogPosts
                    .Include(p => p.User)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToList();
                return View(posts);
            }
            catch (Exception)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tải danh sách bài viết");
                return View(new List<BlogPost>());
            }
        }

        // GET: Blog/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var post = db.BlogPosts
                    .Include("User")
                    .Include("Comments")
                    .Include("Comments.User")
                    .Include("Likes")
                    .FirstOrDefault(p => p.BlogPostId == id);

                if (post == null)
                {
                    return HttpNotFound();
                }

                return View(post);
            }
            catch (Exception)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tải chi tiết bài viết");
                return RedirectToAction("Index");
            }
        }

        // GET: Blog/Create
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
            catch (Exception)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi truy cập trang tạo bài viết");
                return RedirectToAction("Index");
            }
        }

        // POST: Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogPost post)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            post.UserId = (int)Session["UserId"];
                            post.CreatedAt = DateTime.Now;
                            
                            db.BlogPosts.Add(post);
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

                return View(post);
            }
            catch (Exception)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo bài viết");
                return View(post);
            }
        }

        // POST: Blog/Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(BlogComment comment)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (ModelState.IsValid)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            comment.UserId = (int)Session["UserId"];
                            comment.CreatedAt = DateTime.Now;
                            db.BlogComments.Add(comment);
                            db.SaveChanges();
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                return RedirectToAction("Details", new { id = comment.BlogPostId });
            }
            catch (Exception)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm bình luận");
                return RedirectToAction("Details", new { id = comment.BlogPostId });
            }
        }

        // POST: Blog/Like
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Like(int id)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        int userId = (int)Session["UserId"];
                        var existingLike = db.BlogLikes
                            .FirstOrDefault(l => l.BlogPostId == id && l.UserId == userId);

                        if (existingLike != null)
                        {
                            db.BlogLikes.Remove(existingLike);
                        }
                        else
                        {
                            var like = new BlogLike
                            {
                                BlogPostId = id,
                                UserId = userId,
                                CreatedAt = DateTime.Now
                            };
                            db.BlogLikes.Add(like);
                        }

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi thực hiện thao tác like");
                return RedirectToAction("Details", new { id = id });
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