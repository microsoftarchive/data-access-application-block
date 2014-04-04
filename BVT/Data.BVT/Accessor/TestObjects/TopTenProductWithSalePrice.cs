using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.Accessor
{
    public class TopTenProductWithSalePrice
    {
        public string TenMostExpensiveProducts { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SellingPrice { get; set; }
    }
}