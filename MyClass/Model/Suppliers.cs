using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Suppliers")]
    public class Suppliers
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
        [Display(Name = "Tên nhà cung cấp")]
        public string Name { get; set; }
        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }
        [Display(Name = "Tên rút gọn")]
        public string Slug { get; set; }
        [Display(Name = "Sắp xếp")]
        public int? Order { get; set; }
        [Display(Name = "Tên đầy đủ")]
        public string Fullname { get; set; }
        [Display(Name = "Số điện thoại nhà cung cấp")]
        public string Phone { get; set; }
        [Display(Name = "Email nhà cung cấp")]
        public string Email { get; set; }
        [Display(Name = "Trang web nhà cung cấp")]
        public string UrlSite { get; set; }
        [Required(ErrorMessage = "Mô tả nhà cung cấp không được để trống")]
        [Display(Name = "Mô tả")]
        public string MetaDesc { get; set; }
        [Required(ErrorMessage = "Từ khoá không được để trống")]
        [Display(Name = "Từ khoá")]
        public string MetaKey { get; set; }
        [Required(ErrorMessage = "Người tạo không được để trống")]
        [Display(Name = "Tạo bởi")]
        public int CreatedBy { get; set; }
        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        [Display(Name = "Tạo lúc")]
        public DateTime CreatedAt { get; set; }
        [Required(ErrorMessage = "Người cập nhật không được để trống")]
        [Display(Name = "Cập nhật bởi")]
        public int UpdateBy { get; set; }
        [Required(ErrorMessage = "Ngày cập nhật không được để trống")]
        [Display(Name = "Cập nhật lúc")]
        public DateTime UpdateAt { get; set; }
        [Display(Name = "Trạng thái")]
        public int? Status { get; set; }


    }
}
