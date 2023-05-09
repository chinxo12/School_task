
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school.Data;
using school.Models;
using X.PagedList;

namespace school.Controllers
{
    public class UserController : Controller
    {
        private readonly MyDbContext _context;

        public UserController(MyDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {

            return View(_context.Users);


        }


        public IActionResult Create()
        {
            ViewBag.Class = _context.Classes;
            return View();
        }


        [HttpPost]

        public IActionResult Create(int classId, User user)
        {


            var role = _context.Roles.Find(3);
            user.Role = role;
            user.RoleId = 3;
            bool check = true;
            int count = 0;
            int countF = 0;
            user.CreatedDate = DateTime.Now;
            Class classs = _context.Classes.Find(classId);
            if (classs.ClassId == classId)
            {
                user.Class = classs;
            }
            else
            {
                return NotFound();
            }
            Faculty faculty = _context.Faculties.Find(classs.FacultyId);
            School school = _context.Schools.Find(faculty.SchoolId);

            // kiểm tra xem số lượng hiện tại của School.
            var result = _context.Users
                    .Join(_context.Classes, u => u.ClassId, c => c.ClassId, (u, c) => new { User = u, Class = c })
                    .Join(_context.Faculties, uc => uc.Class.FacultyId, f => f.FacultyId, (uc, f) => new { UserClass = uc, Faculty = f })
                    .GroupBy(fc => fc.Faculty.SchoolId)
                    .Select(g => new { SchoolId = g.Key, Count = g.Count() })
                    .ToList();

            foreach (var item in result)
            {
                if (item.SchoolId == school.SchoolId)
                {
                    count = item.Count;
                }
            }
            // Kiểm tra số lượng hiện tại của Khoa
            var result1 = from u in _context.Users
                          join c in _context.Classes on u.ClassId equals c.ClassId
                          group u by c.FacultyId into g
                          select new { FacultyId = g.Key, Count = g.Count() };
            foreach (var item in result1)
            {
                if (item.FacultyId == faculty.FacultyId)
                {
                    countF = item.Count;
                }
            }

           
            

            if (count > school.Capacity || countF > faculty.Capacity)
            {
                check = false;
            }
            if (check)
            {
                user.Password = "123";
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }


           
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Class = _context.Classes;
            User user = _context.Users.Find(id);
            Class c = _context.Classes.Find(user.ClassId);
            user.Class = c;
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, User user)
        {
            try
            {
                User updateUser = _context.Users.Find(id);
                Class _class = _context.Classes.Find(user.ClassId);
                int countF = 0;
                var result1 = from u in _context.Users
                              group u by u.ClassId into g
                              select new { ClassId = g.Key, Count = g.Count() };
                foreach (var item in result1)
                {
                    if (item.ClassId == user.ClassId)
                    {
                        countF = item.Count;
                    }
                }
                if (countF + 1 > _class.Capacity) 
                {
                    return NotFound("Vượt quá sức chứa của lớp");
                }
                updateUser.Class = _class;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return NotFound("Có lỗi trong quá trình xử lý vui lòng thử lại!");
            }
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            User user = _context.Users.Find(id);
            user.IsDeleted = true;
          
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
