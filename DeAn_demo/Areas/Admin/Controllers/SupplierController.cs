using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using DeAn_demo.Library;
using System.IO;

namespace DeAn_demo.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        SuppliersDAO suppliersDAO = new SuppliersDAO();

        // GET: Admin/Supplier/Index
        public ActionResult Index()
        {
            return View(suppliersDAO.getList("Index"));
        }

        // GET: Admin/Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Supplier/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong mot so truong
                //---Create At
                suppliers.CreatedAt = DateTime.Now;
                //---Create By
                suppliers.CreatedBy = Convert.ToInt32(Session["UserID"]);

                //slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                // xu ly order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }

                //update at
                suppliers.UpdateAt = DateTime.Now;
                //update by
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                //them moi mau tin vao db
                suppliersDAO.Insert(suppliers);
                TempData["message"] = new XMessage("success", "Tạo mới nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        // POST: Admin/Supplier/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliers)
        {

            if (ModelState.IsValid)
            {
                //cap nhat

                //xu ly tu dong mot so truong
                //slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                // xu ly order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }

                //update at
                suppliers.UpdateAt = DateTime.Now;
                //update by
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //xu ly thong tin
                //xu ly cho phan upload hình ảnh

                // Kiểm tra và xóa file hình ảnh cũ
                string imageName = suppliers.Image;

                // Kiểm tra và xóa file hình ảnh
                if (!string.IsNullOrEmpty(imageName))
                {
                    string slug = XString.Str_Slug(suppliers.Name);
                    string fileExtension = Path.GetExtension(imageName); // Lấy phần mở rộng của tệp tin
                    string imagePath = Path.Combine(Server.MapPath("~/Public/img/supplier/"), slug + fileExtension);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                suppliers.Image = null;
                // lay thong tin file
                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img != null && img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                 //hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success", "Cập nhật thông tin thành công");
                //cap nhat mau tin vao DB
                suppliersDAO.Update(suppliers);

                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }
        // GET: Admin/Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliers = suppliersDAO.getRow(id);
            string imageName = suppliers.Image;

            // Kiểm tra và xóa file hình ảnh
            if (!string.IsNullOrEmpty(imageName))
            {
                string slug = XString.Str_Slug(suppliers.Name);
                string fileExtension = Path.GetExtension(imageName); // Lấy phần mở rộng của tệp tin
                string imagePath = Path.Combine(Server.MapPath("~/Public/img/supplier/"), slug + fileExtension);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            suppliersDAO.Delete(suppliers);

            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
            return RedirectToAction("Trash");
        }
        // POST: Admin/Supplier/Status
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            suppliers.Status = (suppliers.Status == 1) ? 2 : 1;
            //cap nhat update at
            suppliers.UpdateAt = DateTime.Now;
            //cap nhat update by
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            suppliersDAO.Update(suppliers);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //tro ve trang Index
            return RedirectToAction("Index");
        }

        // POST: Admin/Supplier/DelTrash
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            suppliers.Status = 0;
            //cap nhat update at
            suppliers.UpdateAt = DateTime.Now;
            //cap nhat update by
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            suppliersDAO.Update(suppliers);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Index");

        }
        public ActionResult Trash()
        {
            return View(suppliersDAO.getList("Trash"));
        }

        // POST: Admin/Supplier/Undo
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai status =2
            suppliers.Status = 2;
            //cap nhat update at
            suppliers.UpdateAt = DateTime.Now;
            //cap nhat update by
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            suppliersDAO.Update(suppliers);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Trash");
        }
    }
}
