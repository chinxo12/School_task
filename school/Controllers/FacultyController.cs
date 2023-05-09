using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school.Data;
using school.Models;
using X.PagedList;

namespace school.Controllers
{
    public class FacultyController : Controller
    {
        private readonly MyDbContext _context;
        public FacultyController(MyDbContext context)
        {
            _context = context;
        }



        public IActionResult Index(int page =1)
        {
            int pageSize = 10;
            var facultys = _context.Faculties.Include(f => f.School).OrderBy(f => f.FacultyId).ToPagedList(page,pageSize);
   

            return View(facultys);
        }

        public IActionResult Create()
        {
            ViewBag.Schools = _context.Schools;

            return View();
        }



        [HttpPost]
        public IActionResult Create(Faculty faculty, int schoolId)
        {
            try
            {
                School school = _context.Schools.Find(schoolId);
                if (school != null)
                {
                    int total = _context.Faculties
                        .Where(c => c.SchoolId == school.SchoolId)
                        .Sum(c => c.Capacity);

                    if (school.Capacity > total + faculty.Capacity)
                    {
                        faculty.School = school;
                        faculty.SchoolId = schoolId;
                        faculty.CreatedDate = DateTime.Now;
                        var creator = _context.Users.First();
                        faculty.CreatorId = creator.UserId;
                        _context.Faculties.Add(faculty);
                        _context.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sức chứa của khoa không được lớn hơn sức chứa của trường !!!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Thông tin trường vừa nhập không tồn tại!");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi trong quá trình xử lý vui lòng thử lại !!!");
            }

            // Nếu có lỗi ModelState, hiển thị lại View với lỗi ModelState
            ViewBag.Schools = _context.Schools.ToList();
            return View(faculty);
        }



        public IActionResult Details(int id)
        {
            try
            {
                Faculty faculty = _context.Faculties.Find(id);
                if (faculty == null)
                {
                    return NotFound();
                }
                var school = _context.Schools.Find(faculty.SchoolId);
                faculty.School = school;
                return View(faculty);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }


        public IActionResult Edit(int id)
        {
            try
            {
                Faculty faculty = _context.Faculties.Find(id);
                if (faculty == null)
                {
                    return NotFound();
                }
                var school = _context.Schools.Find(faculty.SchoolId);
                faculty.School = school;
                ViewBag.Schools = _context.Schools;
                return View(faculty);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }


        [HttpPost]
        public IActionResult Edit(int id, Faculty faculty, int schoolId)
        {
            try
            {
                School school = _context.Schools.Find(schoolId);
                if (school == null)
                {
                    return NotFound();
                }
                Faculty facultyToUpdate = _context.Faculties.Find(id);
                int total = _context.Faculties
                                   .Where(c => c.SchoolId == school.SchoolId)
                                   .Sum(c => c.Capacity);
                if (total - facultyToUpdate.Capacity + faculty.Capacity < facultyToUpdate.School.Capacity)
                {

                    if (facultyToUpdate == null)
                    {
                        return NotFound();
                    }
                    facultyToUpdate.FacultyName = faculty.FacultyName;
                    facultyToUpdate.Capacity = faculty.Capacity;
                    facultyToUpdate.School = school;
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("Sức chứa của khoa không được vượt quá sức chứa của trường");
                }

            }
            catch (Exception ex)
            {
                return NotFound("Có lỗi trong quá trình xử lý vui lòng thử lại !!!");
            }


        }


        public IActionResult Delete(int id)
        {
            try
            {
                Faculty faculty = _context.Faculties.Find(id);
                if (faculty == null)
                {
                    return NotFound();
                }

                return View(faculty);
            }
            catch (Exception ex)
            {
                return NotFound("Có lỗi trong quá trình xử lý vui lòng thử lại !!!");
            }
        }

        // Action Delete: Xử lý xóa khoa
        [HttpPost]
        public IActionResult Delete(int id, bool confirm)
        {
          
            try
            {
                Faculty faculty = _context.Faculties.Find(id);

                if (faculty == null)
                {
                    return NotFound("Không tìm thấy dữ liệu từ Id này!");
                }

                _context.Faculties.Remove(faculty);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return NotFound("Có lỗi trong quá trình xử lý vui lòng thử lại !!!");
            }

        }

    }
}
