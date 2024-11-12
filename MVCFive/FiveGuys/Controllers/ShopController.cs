using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FiveGuys.Context;
using FiveGuys.Models;
namespace FiveGuys.Controllers
{
    public class ShopController : Controller
    {
        FiveGuysProductEntities4 objModel = new FiveGuysProductEntities4(); 
        // GET: Shop
        public ActionResult Index()
        {
            var lstCategory = objModel.Categories.ToList();
            var lstProduct =  objModel.TraSuas.ToList();
            
            Product_Category objProduct_Category = new Product_Category();  
            objProduct_Category.ListCategory = lstCategory;
            objProduct_Category.ListProduct = lstProduct;
            return View(objProduct_Category);
        }
    }
}