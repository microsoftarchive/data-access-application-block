using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.Accessor
{
    public class Sale
    {
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal TotalPurchase { get; set; }
    }
}