using _1640WebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _1640WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }
        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
        public IActionResult Index()
        {
            
            var departmentUsers = _context.Departments
            .Select(d => new {
                DepartmentId = d.Id,
                DepartmentName = d.Name,
                UserCount = _context.Users.Count(u => u.DepartmentId == d.Id),
                CoordinatorCount = _context.Users
                .Join(_context.UserRoles,
                    u => u.Id,
                    ur => ur.UserId,
                    (u, ur) => new { User = u, UserRole = ur })
                .Join(_context.Roles,
                    ur => ur.UserRole.RoleId,
                    r => r.Id,
                    (ur, r) => new { User = ur.User, RoleName = r.Name })
                .Count(ur => ur.User.DepartmentId == d.Id && ur.RoleName == "Coordinator"),
                StaffCount = _context.Users
                .Join(_context.UserRoles,
                    u => u.Id,
                    ur => ur.UserId,
                    (u, ur) => new { User = u, UserRole = ur })
                .Join(_context.Roles,
                    ur => ur.UserRole.RoleId,
                    r => r.Id,
                    (ur, r) => new { User = ur.User, RoleName = r.Name })
                .Count(ur => ur.User.DepartmentId == d.Id && ur.RoleName == "Staff"),
                IdeaCount = _context.Ideas
                .Count(i => i.DepartmentId == d.Id)
            })
            .ToList<object>();

            return View(departmentUsers);
        }

        [HttpGet] 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);   
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var department = _context.Departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id,Name")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
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

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //public IActionResult Detail(int id)
        //{
        //    var departmentUsers = _context.Departments
        //    .Where(d => d.Id == id)
        //    .Select(d => new {
        //        DepartmentId = d.Id,
        //        DepartmentName = d.Name,
        //        UserCount = _context.Users.Count(u => u.DepartmentId == d.Id),
        //        CoordinatorCount = _context.Users
        //            .Join(_context.UserRoles,
        //                u => u.Id,
        //                ur => ur.UserId,
        //                (u, ur) => new { User = u, UserRole = ur })
        //            .Join(_context.Roles,
        //                ur => ur.UserRole.RoleId,
        //                r => r.Id,
        //                (ur, r) => new { User = ur.User, RoleName = r.Name })
        //            .Count(ur => ur.User.DepartmentId == d.Id && ur.RoleName == "Coordinator"),
        //        StaffCount = _context.Users
        //            .Join(_context.UserRoles,
        //                u => u.Id,
        //                ur => ur.UserId,
        //                (u, ur) => new { User = u, UserRole = ur })
        //            .Join(_context.Roles,
        //                ur => ur.UserRole.RoleId,
        //                r => r.Id,
        //                (ur, r) => new { User = ur.User, RoleName = r.Name })
        //            .Count(ur => ur.User.DepartmentId == d.Id && ur.RoleName == "Staff"),
        //        IdeaCount = _context.Ideas
        //            .Count(i => i.DepartmentId == d.Id)
        //    })
        //    .ToList<object>();

        //    return View(departmentUsers.Cast<object>().ToList());
        //}

        //public IActionResult Detail(int id)
        //{
        //    var department = _context.Departments
        //        .Where(d => d.Id == id)
        //        .Select(d => new {
        //            DepartmentId = d.Id,
        //            DepartmentName = d.Name,
        //            UserCount = _context.Users.Count(u => u.DepartmentId == d.Id),
        //            CoordinatorCount = _context.Users
        //                .Join(_context.UserRoles,
        //                    u => u.Id,
        //                    ur => ur.UserId,
        //                    (u, ur) => new { User = u, UserRole = ur })
        //                .Join(_context.Roles,
        //                    ur => ur.UserRole.RoleId,
        //                    r => r.Id,
        //                    (ur, r) => new { User = ur.User, RoleName = r.Name })
        //                .Count(ur => ur.User.DepartmentId == d.Id && ur.RoleName == "Coordinator"),
        //            StaffCount = _context.Users
        //                .Join(_context.UserRoles,
        //                    u => u.Id,
        //                    ur => ur.UserId,
        //                    (u, ur) => new { User = u, UserRole = ur })
        //                .Join(_context.Roles,
        //                    ur => ur.UserRole.RoleId,
        //                    r => r.Id,
        //                    (ur, r) => new { User = ur.User, RoleName = r.Name })
        //                .Count(ur => ur.User.DepartmentId == d.Id && ur.RoleName == "Staff"),
        //            IdeaCount = _context.Ideas.Count(i => i.DepartmentId == d.Id)
        //        })
        //    .ToList<object>();
        //    //.SingleOrDefault();


        //    var coordinators = _context.Users
        //    .Join(_context.UserRoles,
        //        u => u.Id,
        //        ur => ur.UserId,
        //        (u, ur) => new { User = u, UserRole = ur })
        //    .Join(_context.Roles,
        //        ur => ur.UserRole.RoleId,
        //        r => r.Id,
        //        (ur, r) => new { User = ur.User, RoleName = r.Name })
        //    .Where(ur => ur.RoleName == "Coordinator" && ur.User.DepartmentId == id)
        //    .Select(u => new {
        //        UserNumber = u.User.StaffNumber,
        //        UserName = u.User.Fullname_,
        //        UserEmail = u.User.Email,
        //        UserPhoneNumber = u.User.PhoneNumber,
        //    })
        //    .ToList();

        //    if (department == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["Coordinators"] = coordinators;

        //    return View(department.Cast<object>().ToList());
        //    //return View(department);
        //}

        public IActionResult Detail(int id)
        {
            var coordinators = _context.Users
            .Join(_context.UserRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => new { User = u, UserRole = ur })
            .Join(_context.Roles,
                ur => ur.UserRole.RoleId,
                r => r.Id,
                (ur, r) => new { User = ur.User, RoleName = r.Name })
            .Where(ur => ur.User.DepartmentId == id && ur.RoleName == "Coordinator")
            .Select(ur => new {
                UserNumber = ur.User.StaffNumber,
                UserName = ur.User.Fullname_,
                UserEmail = ur.User.Email,
                UserPhoneNumber = ur.User.PhoneNumber,
            })
            .ToList();

            var staffs = _context.Users
            .Join(_context.UserRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => new { User = u, UserRole = ur })
            .Join(_context.Roles,
                ur => ur.UserRole.RoleId,
                r => r.Id,
                (ur, r) => new { User = ur.User, RoleName = r.Name })
            .Where(ur => ur.User.DepartmentId == id && ur.RoleName == "Staff")
            .Select(ur => new {
                UserNumber = ur.User.StaffNumber,
                UserName = ur.User.Fullname_,
                UserEmail = ur.User.Email,
                UserPhoneNumber = ur.User.PhoneNumber,
            })
            .ToList();

            var staffIdeaCount = _context.Ideas
            .Join(_context.Users,
                i => i.User.Id,
                u => u.Id,
                (i, u) => new { Idea = i, User = u })
            .Join(_context.UserRoles,
                iu => iu.User.Id,
                ur => ur.UserId,
                (iu, ur) => new { Idea = iu.Idea, UserRole = ur })
            .Join(_context.Roles,
                iur => iur.UserRole.RoleId,
                r => r.Id,
                (iur, r) => new { Idea = iur.Idea, RoleName = r.Name })
            .Where(iur => iur.Idea.DepartmentId == id && iur.RoleName == "Staff")
            .Count();

            var coordinatorIdeaCount = _context.Ideas
            .Join(_context.Users,
                i => i.User.Id,
                u => u.Id,
                (i, u) => new { Idea = i, User = u })
            .Join(_context.UserRoles,
                iu => iu.User.Id,
                ur => ur.UserId,
                (iu, ur) => new { Idea = iu.Idea, UserRole = ur })
            .Join(_context.Roles,
                iur => iur.UserRole.RoleId,
                r => r.Id,
                (iur, r) => new { Idea = iur.Idea, RoleName = r.Name })
            .Where(iur => iur.Idea.DepartmentId == id && iur.RoleName == "Coordinator")
            .Count();


            var department = _context.Departments
                .Where(d => d.Id == id)
                .Select(d => new {
                    DepartmentId = d.Id,
                    DepartmentName = d.Name,
                    UserCount = _context.Users.Count(u => u.DepartmentId == d.Id),
                    CoordinatorCount = coordinators.Count,
                    StaffCount = staffs.Count,
                    StaffIdeaCount= staffIdeaCount,
                    CoordinatorIdeaCount = coordinatorIdeaCount,
                    IdeaCount = _context.Ideas.Count(i => i.DepartmentId == d.Id)
                })
                //.SingleOrDefault();
                .ToList<object>();

           
            if (department == null)
            {
                return NotFound();
            }

            ViewData["Coordinators"] = coordinators.Cast<dynamic>().ToList();
            ViewData["Staffs"] = staffs.Cast<dynamic>().ToList();
            ViewData["StaffIdeaCount"] = staffIdeaCount;
            ViewData["CoordinatorIdeaCount"] = coordinatorIdeaCount;


            //return View(department);
            return View(department.Cast<object>().ToList());
        }


    }
}
