using Microsoft.AspNetCore.Mvc;
using school.Models;

namespace school.Controllers
{
    public class SchoolController : Controller
    {
        private readonly List<School> _schools = new List<School>();

        // Action Index: Hiển thị danh sách trường học
        public IActionResult Index()
        {
            // Lấy danh sách trường học từ cơ sở dữ liệu (hoặc nơi khác)
            // và truyền vào View để hiển thị
            return View(_schools);
        }

        // Action Create: Hiển thị giao diện tạo mới trường học
        public IActionResult Create()
        {
            return View();
        }

        // Action Create: Xử lý tạo mới trường học từ dữ liệu form
        [HttpPost]
        public IActionResult Create(School school)
        {
            // Lưu trường học vào cơ sở dữ liệu (hoặc nơi khác)
            _schools.Add(school);

            // Chuyển hướng về danh sách trường học sau khi tạo mới thành công
            return RedirectToAction("Index");
        }

        // Action Details: Hiển thị thông tin chi tiết trường học
        public IActionResult Details(int id)
        {
            // Tìm kiếm trường học trong cơ sở dữ liệu (hoặc nơi khác) theo id
            var school = _schools.Find(s => s.SchoolId == id);

            // Kiểm tra trường học có tồn tại không
            if (school == null)
            {
                return NotFound();
            }

            // Truyền dữ liệu trường học vào View để hiển thị
            return View(school);
        }

        // Action Edit: Hiển thị giao diện chỉnh sửa trường học
        public IActionResult Edit(int id)
        {
            // Tìm kiếm trường học trong cơ sở dữ liệu (hoặc nơi khác) theo id
            var school = _schools.Find(s => s.SchoolId == id);

            // Kiểm tra trường học có tồn tại không
            if (school == null)
            {
                return NotFound();
            }

            // Truyền dữ liệu trường học vào View để hiển thị
            return View(school);
        }

        // Action Edit: Xử lý cập nhật trường học từ dữ liệu form
        [HttpPost]
        public IActionResult Edit(int id, School school)
        {
            // Tìm kiếm trường học trong cơ sở dữ liệu (hoặc nơi khác) theo id
            var existingSchool = _schools.Find(s => s.SchoolId == id);

            // Kiểm tra trường học có tồn tại không
            if (existingSchool == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin trường học
            existingSchool.SchoolName = school.SchoolName;
            existingSchool.Address = school.Address;
            return View(existingSchool);
        }
        // Action Delete: Hiển thị giao diện xác nhận xóa trường học
        public IActionResult Delete(int id)
        {
            // Tìm kiếm trường học trong cơ sở dữ liệu (hoặc nơi khác) theo id
            var school = _schools.Find(s => s.SchoolId == id);

            // Kiểm tra trường học có tồn tại không
            if (school == null)
            {
                return NotFound();
            }

            // Truyền dữ liệu trường học vào View để hiển thị
            return View(school);
        }

        // Action Delete: Xử lý xóa trường học
        [HttpPost]
        public IActionResult Delete(int id, bool confirm)
        {
            // Tìm kiếm trường học trong cơ sở dữ liệu (hoặc nơi khác) theo id
            var school = _schools.Find(s => s.SchoolId == id);

            // Kiểm tra trường học có tồn tại không
            if (school == null)
            {
                return NotFound();
            }

            // Xóa trường học khỏi cơ sở dữ liệu (hoặc nơi khác)
            _schools.Remove(school);

            // Chuyển hướng về danh sách trường học sau khi xóa thành công
            return RedirectToAction("Index");
        }


    }
}
