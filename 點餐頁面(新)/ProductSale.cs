using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 點餐頁面_新_
{
    public class ProductSale
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CatId { get; set; }
        public int SoldQty { get; set; }
        public string ImgUrl { get; set; }
    }
}
