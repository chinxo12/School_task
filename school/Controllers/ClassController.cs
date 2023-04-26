using Microsoft.AspNetCore.Mvc;
using school.Models;

namespace school.Controllers
{
    public class ClassController : Controller
    {
        private readonly List<Class> _classes = new List<Class>();

        // Action Index: Hiển thị danh sách lớp học
        public IActionResult Index()
        {
            // Lấy danh sách lớp học từ cơ sở dữ liệu (hoặc nơi khác)
            // và truyền vào View để hiển thị
            return View(_classes);
        }
        // Action Create: Hiển thị giao diện tạo mới lớp học
        public IActionResult Create()
        {
            // Truyền dữ liệu mẫu cho View để hiển thị form tạo mới lớp học
            return View();
        }

        // Action Create: Xử lý tạo mới lớp học từ dữ liệu form
        [HttpPost]
        public IActionResult Create(Class @class)
        {
            // Kiểm tra tính hợp lệ của dữ liệu form
            if (ModelState.IsValid)
            {
                // Thêm lớp học vào cơ sở dữ liệu (hoặc nơi khác)
                _classes.Add(@class);

                // Chuyển hướng về danh sách lớp học sau khi tạo mới thành công
                return RedirectToAction("Index");
            }

            // Nếu dữ liệu không hợp lệ, trả về lại View tạo mới lớp học với thông báo lỗi
            return View(@class);
        }

        // Action Edit: Hiển thị giao diện chỉnh sửa lớp học
        public IActionResult Edit(int id)
        {
            // Tìm kiếm lớp học trong cơ sở dữ liệu (hoặc nơi khác) theo id
            var @class = _classes.Find(c => c.ClassId == id);

            // Kiểm tra lớp học có tồn tại không
            if (@class == null)
            {
                return NotFound();
            }

            // Truyền dữ liệu lớp học vào View để hiển thị
            return View(@class);
        }

        // Action Edit: Xử lý chỉnh sửa lớp học từ dữ liệu form
        [HttpPost]
        public IActionResult Edit(int id, Class @class)
        {
            // Tìm kiếm lớp học trong cơ sở dữ liệu (hoặc nơi khác) theo id
            var classToUpdate = _classes.Find(c => c.ClassId == id);

            // Kiểm tra lớp học có tồn tại không
            if (classToUpdate == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin lớp học
            classToUpdate.ClassName = @class.ClassName;


            // Chuyển hướng về danh sách lớp học sau khi cập nhật thành công
            return RedirectToAction("Index");
        }

        // Action Delete: Hiển thị giao diện xác nhận xóa lớp học
        public IActionResult Delete(int id)
        {
            // Tìm kiếm lớp học trong cơ sở dữ liệu (hoặc nơi khác) theo id
            var @class = _classes.Find(c => c.ClassId == id);

            // Kiểm tra lớp học có tồn tại không
            if (@class == null)
            {
                return NotFound();
            }

            // Truyền dữ liệu lớp học vào View để hiển thị
            return View(@class);
        }

        // Action Delete: Xử lý xóa lớp học
        [HttpPost]
        public IActionResult Delete(int id, bool confirm)
        {
            //

            var @class = _classes.Find(c => c.ClassId == id);

            // Kiểm tra lớp học có tồn tại không
            if (@class == null)
            {
                return NotFound();
            }

            // Xóa lớp học nếu xác nhận xóa từ form
            if (confirm)
            {
                _classes.Remove(@class);
            }

            // Chuyển hướng về danh sách lớp học sau khi xóa thành công
            return RedirectToAction("Index");
        }

    }
}
