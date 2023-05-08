
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
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int classId,User user)
        {

            if (ModelState.IsValid)
            { 

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
                if (user.Role.RoleId==1)
                {
                    return NotFound();
                }
                if (user.Role.RoleId == 3)
                {
                    // kiểm tra xem số lượng hiện tại của School.
                    var result = _context.Users
                            .Join(_context.Classes, u => u.ClassId, c => c.ClassId, (u, c) => new { User = u, Class = c })
                            .Join(_context.Faculties, uc => uc.Class.FacultyId, f => f.FacultyId, (uc, f) => new { UserClass = uc, Faculty = f })
                            .GroupBy(fc => fc.Faculty.SchoolId)
                            .Select(g => new { SchoolId = g.Key, Count = g.Count() })
                            .ToList();

                    foreach (var item in result)
                    {
                        if (item.SchoolId == user.Class.Faculty.SchoolId)
                        {
                             count = item.Count;
                        }   
                    }
                    // Kiểm tra số lượng hiện tại của Khoa
                   var result1 = from u in _context.Users join c in _context.Classes on u.ClassId equals c.ClassId
                                 group u by c.FacultyId into g
                                 select new {FacultyId = g.Key,Count = g.Count() };
                    foreach (var item in result1)
                    {
                        if (item.FacultyId == user.Class.FacultyId)
                        {
                            countF = item.Count;
                        }
                    }

                }
                if (count > user.Class.Faculty.School.Capacity || countF> user.Class.Faculty.Capacity)
                {
                    check = false;
                }
                if (check)
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }

            return View(user);
        }

    
        public IActionResult Edit(int? id)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            { 
                _context.Update(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
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
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            User user = _context.Users.Find(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
