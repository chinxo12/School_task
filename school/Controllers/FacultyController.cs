using Microsoft.AspNetCore.Mvc;
using school.Models;

namespace school.Controllers
{
    public class FacultyController : Controller
    {
        private readonly List<Faculty> _faculties = new List<Faculty>();

        // Action Index: Hiển thị danh sách khoa
        public IActionResult Index()
        {
            // Lấy danh sách khoa từ cơ sở dữ liệu (hoặc nơi khác)
            // và truyền vào View để hiển thị
            return View(_faculties);
        }

        // Action Create: Hiển thị giao diện tạo mới khoa
        public IActionResult Create()
        {
            return View();
        }

        // Action Create: Xử lý tạo mới khoa từ dữ liệu form
        [HttpPost]
        public IActionResult Create(Faculty faculty)
        {
            // Lưu khoa vào cơ sở dữ liệu (hoặc nơi khác)
            _faculties.Add(faculty);

            // Chuyển hướng về danh sách khoa sau khi tạo mới thành công
            return RedirectToAction("Index");
        }

        // Action Details: Hiển thị thông tin chi tiết khoa
        public IActionResult Details(int id)
        {
            // Tìm kiếm khoa trong cơ sở dữ liệu (hoặc nơi khác) theo id
            var faculty = _faculties.Find(f => f.FacultyId == id);

            // Kiểm tra khoa có tồn tại không
            if (faculty == null)
            {
                return NotFound();
            }

            // Truyền dữ liệu khoa vào View để hiển thị
            return View(faculty);
        }


        public IActionResult Edit(int id)
        {
            var faculty = _faculties.Find(f => f.FacultyId == id);

            if (faculty == null)
            {
                return NotFound();
            }

            // Truyền dữ liệu khoa vào View để hiển thị
            return View(faculty);
        }

        // Action Edit: Xử lý chỉnh sửa khoa từ dữ liệu form
        [HttpPost]
        public IActionResult Edit(int id, Faculty faculty)
        {
            // Tìm kiếm khoa trong cơ sở dữ liệu (hoặc nơi khác) theo id
            var facultyToUpdate = _faculties.Find(f => f.FacultyId == id);

            // Kiểm tra khoa có tồn tại không
            if (facultyToUpdate == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin khoa
            facultyToUpdate.FacultyName = faculty.FacultyName;
            // Chuyển hướng về danh sách khoa sau khi cập nhật thành công
            return RedirectToAction("Index");
        }

        // Action Delete: Hiển thị giao diện xác nhận xóa khoa
        public IActionResult Delete(int id)
        {
            // Tìm kiếm khoa trong cơ sở dữ liệu (hoặc nơi khác) theo id
            var faculty = _faculties.Find(f => f.FacultyId == id);

            // Kiểm tra khoa có tồn tại không
            if (faculty == null)
            {
                return NotFound();
            }

            // Truyền dữ liệu khoa vào View để hiển thị
            return View(faculty);
        }

        // Action Delete: Xử lý xóa khoa
        [HttpPost]
        public IActionResult Delete(int id, bool confirm)
        {
            // Tìm kiếm khoa trong cơ sở dữ liệu (hoặc nơi khác) theo id
            var faculty = _faculties.Find(f => f.FacultyId == id);

            // Kiểm tra khoa có tồn tại không
            if (faculty == null)
            {
                return NotFound();
            }

            // Xóa khoa khỏi cơ sở dữ liệu (hoặc nơi khác)
            _faculties.Remove(faculty);

            // Chuyển hướng về danh sách khoa sau khi xóa thành công
            return RedirectToAction("Index");
        }

    }
}
