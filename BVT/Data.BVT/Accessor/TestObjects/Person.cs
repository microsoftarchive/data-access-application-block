using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.Accessor.TestObjects
{
    public abstract class Person
    {
        public int CustomerID { get; set; }
        public bool IsEmployee { get; set; }
        public string CompanyName { get; set; }
    }
}