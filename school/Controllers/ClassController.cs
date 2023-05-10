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


        public IActionResult Index(int page = 1)
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
                bool check = true;
                Faculty faculty = _context.Faculties.Find(facultyId);
                if (faculty != null)
                {
                    _class.Faculty = faculty;

                    var totalCapacity = _context.Classes
                                        .Where(c => c.FacultyId == faculty.FacultyId)
                                        .Sum(c => c.Capacity);

                    if (_class.Capacity <= 0)
                    {
                        ModelState.AddModelError("Capacity", "Vui lòng nhập sức chứa lớn hơn 0");
                        check = false;
                    }

                    var classes = _context.Classes
                                  .Where(c => c.Faculty.SchoolId == faculty.SchoolId)
                                  .ToList();

                    foreach (var c in classes)
                    {
                        if (c.ClassName.Equals(_class.ClassName))
                        {
                            ModelState.AddModelError("ClassName", "Tên lớp đã tồn tại trong Trường này!");
                            check = false;
                            break;
                        }
                    }
                    // Kiểm tra tổng sức chứa của lớp và khoa

                    if (faculty.Capacity < totalCapacity + _class.Capacity)
                    {
                        ModelState.AddModelError("Capacity", "Sức chứa của lớp không được vượt quá sức chứa của khoa");
                        check = false;
                    }
                    if (check)
                    {
                        var creator = _context.Users.First();
                        _class.CreatedDate = DateTime.Now;
                        _class.CreatorId = creator.CreatorId;
                        _context.Classes.Add(_class);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    ModelState.AddModelError("FacultyId", "Khoa không tồn tại");
                }



            }
            catch (Exception e)
            {

                return NotFound("có lỗi trong quá trình xử lý vui lòng thử lại!");
            }

            ViewBag.Schools = _context.Schools;
            return View(_class);

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
                bool check = true;
                Class classToUpdate = _context.Classes.Find(id);
                if (classToUpdate == null)
                {
                    return NotFound("Không tồn tại lớp này");
                }
                Faculty faculty = _context.Faculties.Find(facultyId);
                if (faculty == null)
                {
                    ModelState.AddModelError("FacultyId", "Khoa không tồn tại");
                    check = false;
                }
                if (!_class.ClassName.Equals(classToUpdate.ClassName))
                {
                    var classes = _context.Classes.Where(x => x.FacultyId == facultyId).ToList();
                    foreach (var c in classes)
                    {
                        if (c.ClassName.Equals(_class.ClassName))
                        {
                            ModelState.AddModelError("ClassName", "Tên lớp đã tồn tại trong khoa này!");
                            check = false;
                            break;
                        }
                    }
                }
                var totalCapacity = _context.Classes
                                   .Where(c => c.FacultyId == faculty.FacultyId)
                                   .Sum(c => c.Capacity);

                // Kiểm tra tổng sức chứa của lớp với sức chứa của khoa
                if (faculty.Capacity < totalCapacity + _class.Capacity)
                {
                    ModelState.AddModelError("Capacity", "Sức chứa của lớp không được vượt quá sức chứa của khoa");
                }
                if (check)
                {
                    classToUpdate.Faculty = faculty;

                    classToUpdate.ClassName = _class.ClassName;


                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception e)
            {
                return NotFound("có lỗi trong quá trình xử lý vui lòng thử lại!");
            }
            return View(_class);

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
