using _1640WebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _1640WebApp.Controllers
{
    [Authorize(Roles = "Staff, Admin, Manager, Coordinator")]

    public class IdeaController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public IdeaController(ApplicationDbContext context = null, UserManager<ApplicationUser> userManager = null)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Ideas
        public async Task<IActionResult> Index()
        {
            var ideas = _context.Ideas.Include(i => i.Catogories).Include(i => i.Submission).Include(i => i.User);

            return View(await ideas.ToListAsync());
        }

        // GET: ViewIdeas
        public async Task<IActionResult> ViewIdeas()
        {

            var applicationDbContext = _context.Ideas.Include(i => i.Submission).Include(i => i.User);
            return View(await applicationDbContext.ToListAsync());
        }

        



        // GET: Ideas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ideas == null)
            {
                return NotFound();
            }

            var idea = await _context.Ideas
                .Include(i => i.Submission)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (idea == null)
            {
                return NotFound();
            }

            return View(idea);
        }

        // GET: Ideas/Create
        public IActionResult Create(int submissionId)
        {
            ViewBag.SubmissionId = submissionId;
            ViewBag.Categories = _context.Catogorys.ToList();
   
            return View();
        }

        // POST: Ideas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int submissionId,Idea idea, IFormCollection form)
        {
            var categoryIds = form["categories"].ToString().Split(",");
            idea.Catogories = new List<Catogory>(); // khởi tạo list categories trước khi thêm vào           
            foreach (var categoryId in categoryIds)
            {
                if (int.TryParse(categoryId, out int categoryIdInt))
                {
                    var category = _context.Catogorys.Find(categoryIdInt);
                    if (category != null)
                    {
                        idea.Catogories.Add(category);
                    }
                }
            }
            // Đăng bài ẩn danh
            // Xác định trạng thái của checkbox và gán vào thuộc tính `Anonymous` của `idea`
            idea.Anonymous = Request.Form["Anonymous"].Count > 0;

            var newIdea = new Idea { SubmissionId = submissionId };
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            
            idea.UserId = userId;
                                
            _context.Add(idea);

            var submission = await _context.Submissions.FindAsync(idea.SubmissionId);

            if (submission == null)
            {
                // Trả về thông báo lỗi nếu Submission không tồn tại.
                return NotFound();
            }
            else if (submission.IsClosed)
            {
                // Trả về thông báo lỗi nếu Submission đã bị đóng.
                return BadRequest("Cannot add new ideas to a closed submission.");
            }

            // Thêm Idea mới vào Submission nếu Submission chưa bị đóng.
            submission.Ideas.Add(idea);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));         
        }



        // GET: Ideas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ideas == null)
            {
                return NotFound();
            }

            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound();
            }
            ViewData["SubmissionId"] = new SelectList(_context.Submissions, "Id", "Id", idea.SubmissionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", idea.UserId);
            return View(idea);
        }

        // POST: Ideas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,UserId,SubmissionId")] Idea idea)
        {
            if (id != idea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(idea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdeaExists(idea.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubmissionId"] = new SelectList(_context.Submissions, "Id", "Id", idea.SubmissionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", idea.UserId);
            return View(idea);
        }

        // GET: Ideas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ideas == null)
            {
                return NotFound();
            }

            var idea = await _context.Ideas
                .Include(i => i.Submission)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (idea == null)
            {
                return NotFound();
            }

            return View(idea);
        }

        // POST: Ideas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Ideas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ideas'  is null.");
            }
            var idea = await _context.Ideas.FindAsync(id);
            if (idea != null)
            {
                _context.Ideas.Remove(idea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IdeaExists(int id)
        {
            return _context.Ideas.Any(e => e.Id == id);
        }
    }
}
