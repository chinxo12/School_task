using Microsoft.AspNetCore.Mvc;
using school.Data;
using school.Models;
using X.PagedList;

namespace school.Controllers
{
    public class SchoolController : Controller
    {
        private readonly MyDbContext _context;

        public SchoolController(MyDbContext context)
        {
            _context = context;
        }


        public IActionResult Index(int page=1)
        {
            int pageSize = 10;
            var schools = _context.Schools.OrderBy(x => x.SchoolId).ToPagedList(page, pageSize);
            return View(schools);
            /*return View(_context.Schools);*/
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(School school)
        {
            var creator = _context.Users.Where(u => u.RoleId == 1).First();
            school.Creator = creator;
            school.CreatorId = creator.UserId;
            
                if (school.Capacity <= 1000)
                {
                    try
                    {
                        school.FoundedTime = DateTime.Now;
                        _context.Schools.Add(school);

                        _context.SaveChanges(); 

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex) { 
                        return NotFound("Có lỗi trong quá trình xử lý vui lòng thử lại!!!");
                    }

                }
                else
                {
                    return NotFound("Sức chứa của trường không được vượt quá 1000!!!");
                }              
            
          
        }

      
        public IActionResult Details(int id)
        {
            if (id == null)
            {
                return NotFound() ;
            }

            try
            {
                School school = _context.Schools.Find(id);

                if (school == null)
                {
                    return NotFound();
                }
                var creator = _context.Users.First();
                school.Creator = creator;

                return View(school);

            }
            catch (Exception e)
            {
                return NotFound("Có lỗi trong quá trình xử lý vui lòng thử lại!!!");
            }

          
        }

       
        public IActionResult Edit(int id)
        {

            if (id == null)
            {
                return NotFound();
            }

            try
            {
                School school = _context.Schools.Find(id);

                if (school == null)
                {
                    return NotFound();
                }

      
                return View(school);

            }
            catch (Exception e)
            {
                return NotFound("Có lỗi trong quá trình xử lý vui lòng thử lại!!!");
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, School school)
        {
            try
            {
                School existingSchool = _context.Schools.Find(id);

                if (existingSchool == null)
                {
                    return NotFound();
                }

                if (school.Capacity <= 1000)
                {
                    existingSchool.SchoolName = school.SchoolName;
                    existingSchool.Address = school.Address;
                    existingSchool.Capacity = school.Capacity;

                    _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("Sức chứa của trường không được vượt quá 1000!!!");
                }

            }
            catch (Exception e)
            {
                return NotFound("Có lỗi trong quá trình xử lý vui lòng thử lại!!!");
            }
        }

        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                School school = _context.Schools.Find(id);

                if (school == null)
                {
                    return NotFound();
                }


                return View(school);

            }
            catch (Exception e)
            {
                return NotFound("Có lỗi trong quá trình xử lý vui lòng thử lại!!!");
            }
        }

        // Action Delete: Xử lý xóa trường học
        [HttpPost]
        public IActionResult Delete(int id, bool confirm)
        {
           
            try
            {
               
                    School school = _context.Schools.Find(id);
                    if (school == null)
                    {
                        return NotFound();
                    }
                    _context.Schools.Remove(school);
                    _context.SaveChanges();


                
                return RedirectToAction("Index");

            }
            catch(Exception e)
            {
                return NotFound("Có lỗi trong quá trình xử lý vui lòng thử lại!!!");
            }
         
        }


    }
}
