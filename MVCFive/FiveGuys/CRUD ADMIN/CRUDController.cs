using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FiveGuys.Models;
using FiveGuys.Context;



namespace FiveGuys.CRUD_ADMIN
{
    public class CRUDController : Controller
    {
        // GET: CRUD
        FiveGuysProductEntities4 objModel = new FiveGuysProductEntities4();
        public ActionResult Index()
        {
            var lstCategory = objModel.Categories.ToList();
            var lstProduct = objModel.TraSuas.ToList();
            Product_Category objProduct_Category = new Product_Category();
            objProduct_Category.ListCategory = lstCategory;
            objProduct_Category.ListProduct = lstProduct;
            return View(objProduct_Category);
        }
        public ActionResult Create()
        {
            return View();
        }
    }   
}