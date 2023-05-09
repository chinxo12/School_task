using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school.Data;
using school.Models;
using X.PagedList;

namespace school.Controllers
{
    public class ClassController : Controller
    {
        private readonly MyDbContext _context;
        public ClassController(MyDbContext context)
        {
            _context = context;
        }


        public IActionResult Index(int page=1)
        {
            int pageSize = 3;
            var _classes = _context.Classes.Include(c => c.Faculty).OrderBy(x => x.ClassId).ToPagedList(page, pageSize);
           
            return View(_classes);

      
        }

        public IActionResult Create()
        {
            ViewBag.Schools = _context.Schools;
            return View();
        }


        [HttpPost]
        public IActionResult Create(Class _class, int facultyId)
        {


            try
            {
                Faculty faculty = _context.Faculties.Find(facultyId);
                if (faculty != null)
                {
                    _class.Faculty = faculty;

                    var totalCapacity = _context.Classes
                                        .Where(c => c.FacultyId == faculty.FacultyId)
                                        .Sum(c => c.Capacity);
                    // Kiểm tra tổng sức chứa của lớp và khoa

                    if (faculty.Capacity < totalCapacity + _class.Capacity)
                    {
                        return NotFound("Sức chứa của lớp không được vượt quá sức chứa của khoa!");
                    }
                    var creator = _context.Users.First();
                    _class.CreatedDate = creator.CreatedDate;
                    _class.CreatorId = creator.CreatorId;
                    
                    _context.Classes.Add(_class);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("Khoa không tồn tại!");
                }



            }
            catch (Exception e)
            {
                return NotFound("có lỗi trong quá trình xử lý vui lòng thử lại!");
            }
           
        }

        public IActionResult Details(int id)
        {
            try
            {
                var _class = _context.Classes.Find(id);
                var faculty = _context.Faculties.Find(_class.FacultyId);
                _class.Faculty = faculty;
                return View(_class);

            }
            catch (Exception e)
            {
                return NotFound("có lỗi trong quá trình xử lý vui lòng thử lại!");
            }

        }


        public IActionResult Edit(int id)
        {

            try
            {
                Class _class = _context.Classes.Find(id);
                if (_class == null)
                {
                    return NotFound();
                }

                ViewBag.Faculty = _context.Faculties;
                return View(_class);
            }
            catch (Exception e)
            {
                return NotFound("có lỗi trong quá trình xử lý vui lòng thử lại!");
            }

        }


        [HttpPost]
        public IActionResult Edit(int id, Class _class, int facultyId)
        {
            try
            {
                Class classToUpdate = _context.Classes.Find(id);
                if (classToUpdate == null)
                {
                    return NotFound();
                }
                Faculty faculty = _context.Faculties.Find(facultyId);
                if (faculty == null)
                {
                    return NotFound("Khôn tồn tại khoa này!");
                }
                classToUpdate.Faculty = faculty;

                classToUpdate.ClassName = _class.ClassName;

                var totalCapacity = _context.Classes
                                    .Where(c => c.FacultyId == faculty.FacultyId)
                                    .Sum(c => c.Capacity);

                // Kiểm tra tổng sức chứa của lớp với sức chứa của khoa
                if (faculty.Capacity < totalCapacity + _class.Capacity)
                {
                    return NotFound("Sức chứa của lớp không được vượt quá sức chứa của khoa!");
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return NotFound("có lỗi trong quá trình xử lý vui lòng thử lại!");
            }

        }


        public IActionResult Delete(int id)
        {
            try
            {
                Class _class = _context.Classes.Find(id);

                // Kiểm tra lớp học có tồn tại không
                if (_class == null)
                {
                    return NotFound();
                }

                // Truyền dữ liệu lớp học vào View để hiển thị
                return View(_class);
            }
            catch (Exception e)
            {
                return NotFound("có lỗi trong quá trình xử lý vui lòng thử lại!");
            }

        }


        [HttpPost]
        public IActionResult Delete(int id, bool confirm)
        {
            try
            {
                Class @class = _context.Classes.Find(id);

                // Kiểm tra lớp học có tồn tại không
                if (@class == null)
                {
                    return NotFound();
                }
                
                    _context.Classes.Remove(@class);
                    _context.SaveChanges();
                


                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return NotFound("có lỗi trong quá trình xử lý vui lòng thử lại!");
            }

        }
        public PartialViewResult GetFacultiesBySchoolId(int schoolId)
        {
            var faculties = _context.Faculties.Where(f => f.SchoolId == schoolId).ToList();
            return PartialView("_FacultyList", faculties);
        }

    }
}
