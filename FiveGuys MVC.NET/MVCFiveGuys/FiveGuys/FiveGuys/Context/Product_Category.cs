using FiveGuys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGuys.Context
{
    public class Product_Category
    {
        public List<TraSua> ListProduct { get; set; }
        public List<Category> ListCategory { get; set; }
    }
    public static class Formatter
    {
        public static string FormatCurrency(decimal amount)
        {
            return string.Format("{0:N0} Đ", amount);
        }
    }
}