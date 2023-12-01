﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeAn_demo.Library;
using MyClass.DAO;
using MyClass.Model;

namespace DeAn_demo.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        LinksDAO linksDAO = new LinksDAO();
        //-----------------------------------------------------------------------------
        // GET: Admin/Category/Index
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }

        //-----------------------------------------------------------------------------
        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại hàng!");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "KHông tìm thấy loại hàng!");
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        //Lấy tên dựa trên parentId
        public string GetParentCategoryName(int? parentID)
        {
            if (parentID.HasValue)
            {
                Categories parentCategory = categoriesDAO.getRow(parentID.Value);
                if (parentCategory != null)
                {
                    return parentCategory.Name;
                }
            }

            return string.Empty;
        }

        //-----------------------------------------------------------------------------
        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.Catlist = new SelectList(categoriesDAO.getList("Index"), "ID", "Name");
            ViewBag.Orderlist = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //---Create At
                categories.CreatedAt = DateTime.Now;
                //---Create By
                categories.CreatedBy = Convert.ToInt32(Session["UserId"]);
                //Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //ParentID
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //Update at
                categories.UpdateAt = DateTime.Now;
                //Update by
                categories.UpdateBy = Convert.ToInt32(Session["UserId"]);
                //xu ly cho muc Topics
                if (categoriesDAO.Insert(categories) == 1)//khi them du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = categories.Slug;
                    links.TableID = categories.Id;
                    links.Type = "category";
                    linksDAO.Insert(links);
                }
                //hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Tạo mới loại sản phẩm thành công!");
                return RedirectToAction("Index");
            }
            ViewBag.Catlist = new SelectList(categoriesDAO.getList("Index"), "ID", "Name");
            ViewBag.Orderlist = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        //-----------------------------------------------------------------------------
        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Catlist = new SelectList(categoriesDAO.getList("Index"), "ID", "Name");
            ViewBag.Orderlist = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại!");
                return RedirectToAction("Index");
            }
            return View(categories);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //ParentID
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //Update at
                categories.UpdateAt = DateTime.Now;
                //Update by
                categories.UpdateBy = Convert.ToInt32(Session["UserId"]);
                //hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Cập nhật thông tin thành công!");
                //cập nhật links
                if (categoriesDAO.Update(categories) == 1)
                {
                    //Neu trung khop thong tin: Type = category va TableID = categories.ID
                    Links links = linksDAO.getRow(categories.Id, "category");
                    //cap nhat lai thong tin
                    links.Slug = categories.Slug;
                    linksDAO.Update(links);
                }
                return RedirectToAction("Index");
            }
            ViewBag.Catlist = new SelectList(categoriesDAO.getList("Index"), "ID", "Name");
            ViewBag.Orderlist = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        //-----------------------------------------------------------------------------
        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại!");
                return RedirectToAction("Trash");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại!");
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
            //Xóa links khi tìm thấy
            if (categoriesDAO.Delete(categories) == 1)
            {
                //Neu trung khop thong tin: Type = category va TableID = categories.ID
                Links links = linksDAO.getRow(categories.Id, "category");
                //Xóa luôn cho Links
                linksDAO.Delete(links);
            }
            //hiện thị thông báo
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công!");
            return RedirectToAction("Trash");
        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại!");
                return RedirectToAction("Index");
            }

            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại!");
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            categories.Status = (categories.Status == 1) ? 2 : 1;
            //cập nhật update at
            categories.UpdateAt = DateTime.Now;
            //cập nhật update by
            categories.UpdateBy = Convert.ToInt32(Session["UserId"]);
            //xác nhận DB (update DB)
            categoriesDAO.Update(categories);
            //hiện thị thông báo
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công!");
            //trở về trang index
            return RedirectToAction("Index");
        }

        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại!");
                return RedirectToAction("Index");
            }

            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại!");
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            categories.Status = 0;
            //cập nhật update at
            categories.UpdateAt = DateTime.Now;
            //cập nhật update by
            categories.UpdateBy = Convert.ToInt32(Session["UserId"]);
            //xác nhận DB (update DB)
            categoriesDAO.Update(categories);
            //hiện thị thông báo
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công!");
            //trở về trang index
            return RedirectToAction("Index");
        }

        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));
        }
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại!");
                return RedirectToAction("Index");
            }

            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại!");
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái status = 2
            categories.Status = 2;
            //cập nhật update at
            categories.UpdateAt = DateTime.Now;
            //cập nhật update by
            categories.UpdateBy = Convert.ToInt32(Session["UserId"]);
            //xác nhận DB (update DB)
            categoriesDAO.Update(categories);
            //hiện thị thông báo
            TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công!");
            //ở lại tiếp tục lục thùng rác
            return RedirectToAction("Trash");
        }
    }
}