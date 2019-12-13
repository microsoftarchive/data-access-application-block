/*
Copyright 2013 Microsoft Corporation
Licensed under the Apache License, Version 2.0 (the "License");

you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.Accessor.TestObjects
{
    public class CustomerDetailsRowMapper : IRowMapper<CustomerDetails>
    {

        #region IRowMapper<CustomerDetails> Members

        public CustomerDetails MapRow(IDataRecord row)
        {
            return new CustomerDetails()
            {
                City = (string)row["City"],
                CompanyName = (string)row["CompanyName"],
                Country = (string)row["Country"],
                CustomerID = (string)row["CustomerID"]
            };
        }

        #endregion
    }
}

