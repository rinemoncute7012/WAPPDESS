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

namespace DeAn_demo.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        //
        // GET: Admin/Category/index
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }
        /// //////////////////////////////////////////////
        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            return View(categories);
        }
        //////////////////////////////////////////////
        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Slug,ParentId,Order,MetaDesc,MetaKey,CreatedBy,CreatedAt,UpdateBy,UpdateAt,Status")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                //xuly tu dong
                //---create at
                categories.CreatedAt = DateTime.Now;
                //===create by
                categories.CreatedBy = Convert.ToInt32(Session["UserID"]);
                //slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //pareentid
                if(categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //order
                if(categories.Order == null)
                {
                    categories.Order = 1;
                }    
                else
                {
                    categories.Order += 1;
                }
                //updateat
                categories.UpdateAt = DateTime.Now;
                //updateby
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
                categoriesDAO.Insert(categories);
                //hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success","Tạo mới loại sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        //GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Slug,ParentId,Order,MetaDesc,MetaKey,CreatedBy,CreatedAt,UpdateBy,UpdateAt,Status")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                //chinh sua 1 so thong tin tu dong
                //xuly tu dong
               
                //slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //pareentid
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //updateat
                categories.UpdateAt = DateTime.Now;
                //updateby
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //cap nhat db
                categoriesDAO.Update(categories);
                //hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success", "Cập nhật sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }
        ///// //////////////////////////////////////////////
        //// GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //thong bao
                TempData["message"] = new XMessage("danger", "Xoá sản phẩm thất bại");
                return RedirectToAction("Trash");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thong bao
                TempData["message"] = new XMessage("danger", "Xoá sản phẩm thất bại");
                return RedirectToAction("Trash");
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            //tim thay mau tin- xoa
            categoriesDAO.Delete(categories);
            //thong bao
            TempData["message"] = new XMessage("danger", "Xoá sản phẩm thành công");
            return RedirectToAction("Trash");
        }
        // GET: Admin/Category/Details/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            categories.Status = (categories.Status == 1) ? 2 : 1;
            //cap nhat update at
            categories.UpdateAt = DateTime.Now;
            //cap nhat update by
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái sản phẩm thành công");
            //tro ve trang index
            return RedirectToAction("Index");

        }
        ///////////////////////////////////////////////////
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //thong bao
                TempData["message"] = new XMessage("danger", "Xoá sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Xoá sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            categories.Status = 0;
            //cap nhat update at
            categories.UpdateAt = DateTime.Now;
            //cap nhat update by
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xoá sản phẩm thành công");
            //tro ve trang index
            return RedirectToAction("Index");

        }
        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));
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
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai status = 2
            categories.Status = 2;
            //Cap nhat Update At
            categories.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            categoriesDAO.Update(categories);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công thành công!");
            //Tro ve trang Index
            return RedirectToAction("Trash");// ở lại thùng rác tiếp tục phục hồi or xóa
        }
    }
}
