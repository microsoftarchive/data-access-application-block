using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.Accessor
{
    public class TopTenProduct
    {
        public string TenMostExpensiveProducts { get; set; }
        public decimal UnitPrice { get; set; }
    }
}