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
    public class CustomerDetailsResultSet : IResultSetMapper<CustomerDetails>
    {
        #region IResultSetMapper<CustomerDetails> Members

        public IEnumerable<CustomerDetails> MapSet(IDataReader reader)
        {
            int i = 0;
            while (reader.Read() && i < 2)
            {
                yield return new CustomerDetails()
                {
                    City = (string)reader["City"],
                    CompanyName = (string)reader["CompanyName"],
                    Country = (string)reader["Country"],
                    CustomerID = (string)reader["CustomerID"]
                };

                i++;
            }
        }

        #endregion
    }
}

