using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class ProductsDAO
    {
        private MyDBContext db = new MyDBContext();
        //index
        public List<Products> getList()
        {
            return db.Products.ToList();
        }
        //index dua vao status =1,2, con status =0 == thung rac
        public List<Products> getList(string status = "All")
        {
            List<Products> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Products
                            .Where(m => m.Status != 0)
                            .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Products
                            .Where(m => m.Status == 0)
                            .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Products.ToList();
                        break;
                    }
            }
            return list;
        }
        //DETAILS
        public Products getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Products.Find(id);
            }
        }
        //create
        public int Insert(Products row)
        {
            db.Products.Add(row);
            return db.SaveChanges();
        }
        //update
        public int Update(Products row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //delete
        public int Delete(Products row)
        {
            db.Products.Remove(row);
            return db.SaveChanges();
        }
    }
}
