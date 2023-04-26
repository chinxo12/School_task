using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using school.Data;
using school.Models;
using System.Security.Claims;

namespace school.Controllers
{
    public class ClassController : Controller
    {
        private readonly MyDbContext _context;
        public ClassController(MyDbContext context)
        {
            _context = context;
        }

        // Action Index: Hiển thị danh sách lớp học
        public IActionResult Index()
        {

            return View(_context.Classes);
        }

        public IActionResult Create()
        {

            return View();
        }

        // Action Create: Xử lý tạo mới lớp học từ dữ liệu form
        [HttpPost]
        public IActionResult Create(Class @class, int facultyId)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Faculty faculty = _context.Faculties.Find(facultyId);
                    if (faculty != null)
                    {
                        @class.Faculty = faculty;
                        if (faculty.Capacity<@class.Capacity) {
                            return NotFound("Sức chứa của lớp không được vượt quá sức chứa của khoa!");
                        }
                        _context.Classes.Add(@class);
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


            return View(@class);
        }


        public IActionResult Edit(int id)
        {

            try
            {
                Class @class = _context.Classes.Find(id);
                if (@class == null)
                {
                    return NotFound();
                }
                return View(@class);
            }
            catch (Exception e)
            {
                return NotFound("có lỗi trong quá trình xử lý vui lòng thử lại!");
            }

        }


        [HttpPost]
        public IActionResult Edit(int id, Class @class, int facultyId)
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
                
                classToUpdate.ClassName = @class.ClassName;
                if (faculty.Capacity < @class.Capacity)
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
                Class @class = _context.Classes.Find(id);

                // Kiểm tra lớp học có tồn tại không
                if (@class == null)
                {
                    return NotFound();
                }

                // Truyền dữ liệu lớp học vào View để hiển thị
                return View(@class);
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
                if (confirm)
                {
                    _context.Classes.Remove(@class);
                    _context.SaveChanges();
                }

         
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return NotFound("có lỗi trong quá trình xử lý vui lòng thử lại!");
            }
           
        }
    }
}
