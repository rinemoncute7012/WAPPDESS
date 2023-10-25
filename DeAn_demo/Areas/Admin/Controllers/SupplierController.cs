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
        // GET: Admin/Suppliers
        public ActionResult Index()
        {
            return View(suppliersDAO.getList("Index"));
        }

        // GET: Admin/Suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //thong bao
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

        // GET: Admin/Suppliers/Create
        public ActionResult Create()
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong
                //xuly tu dong
                //---create at
                suppliers.CreatedAt = DateTime.Now;
                //===create by
                suppliers.CreatedBy = Convert.ToInt32(Session["UserID"]);
                //slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }
                //updateat
                suppliers.UpdateAt = DateTime.Now;
                //updateby
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //XU LI THONG TIN CHO HINH ANH
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
                suppliersDAO.Insert(suppliers);
                TempData["message"] = new XMessage("success", "Thêm mới nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        // GET: Admin/Suppliers/Edit/5
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

        // POST: Admin/Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //xuly tu dong

                //slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }
                //updateat
                suppliers.UpdateAt = DateTime.Now;
                //updateby
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //cap nhat mau tin vao db
                suppliersDAO.Update(suppliers);
                TempData["message"] = new XMessage("success", "Thêm mới nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        // GET: Admin/Suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //thong bao
                TempData["message"] = new XMessage("danger", "Xoá thông tin nhà cung cấp thất bại");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //thong bao
                TempData["message"] = new XMessage("danger", "Xoá thông tin nhà cung cấp thất bại");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // POST: Admin/Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliers = suppliersDAO.getRow(id);
            //tim thay mau tin- xoa
            suppliersDAO.Delete(suppliers);
            //thong bao
            TempData["message"] = new XMessage("danger", "Xoá thông tin nhà cung cấp thành công");
            return RedirectToAction("Trash");
        }
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái nhà cung cấp thất bại");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái nhà cung cấp thất bại");
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
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái nhà cung cấp thành công");
            //tro ve trang index
            return RedirectToAction("Index");

        }
        ///////////////////////////////////////////////////
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //thong bao
                TempData["message"] = new XMessage("danger", "Xoá thông tin nhà cung cấp thất bại");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Xoá thông tin nhà cung cấp thất bại");
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
            TempData["message"] = new XMessage("success", "Xoá thông tin nhà cung cấp thành công");
            //tro ve trang index
            return RedirectToAction("Index");

        }
        public ActionResult Trash()
        {
            return View(suppliersDAO.getList("Trash"));
        }
        //// GET: Admin/Category/Edit/5
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai status = 2
            suppliers.Status = 2;
            //Cap nhat Update At
            suppliers.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            suppliersDAO.Update(suppliers);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công thành công!");
            //Tro ve trang Index
            return RedirectToAction("Trash");// ở lại thùng rác tiếp tục phục hồi or xóa
        }
    }
}
