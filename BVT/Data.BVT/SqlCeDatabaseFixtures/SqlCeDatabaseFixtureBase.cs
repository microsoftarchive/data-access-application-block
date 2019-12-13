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
using System.Configuration;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.SqlCeDatabaseFixtures
{
    public class SqlCeDatabaseFixtureBase : EntLibFixtureBase
    {
        protected SqlCeDatabase db;

        public SqlCeDatabaseFixtureBase(string configFile)
            : base(configFile)
        {
        }

        public override void Initialize()
        {
            ConnectionStringsSection section = (ConnectionStringsSection)base.ConfigurationSource.GetSection("connectionStrings");
            connStr = section.ConnectionStrings["SqlCeTest"].ConnectionString;
            db = new SqlCeDatabase(connStr);

            SqlCeConnection conn = new SqlCeConnection();
            conn.ConnectionString = connStr;
            conn.Open();
            SqlCeCommand cmd = null;
            cmd = new SqlCeCommand("Delete from Items where itemId > 3 ", conn);
            cmd.ExecuteNonQuery();
            cmd = new SqlCeCommand("Update Items set ItemDescription='Digital Image Pro',QtyInHand = 25, QtyRequired=100, Price = 38.95 where ItemId = 1", conn);
            cmd.ExecuteNonQuery();
            cmd = new SqlCeCommand("Update Items set ItemDescription='Excel 2003',QtyInHand = 95, QtyRequired=100, Price = 98.95 where ItemId = 2", conn);
            cmd.ExecuteNonQuery();
            cmd = new SqlCeCommand("Update Items set ItemDescription='Infopath',QtyInHand = 34, QtyRequired=100, Price = 89 where ItemId = 3", conn);
            cmd.ExecuteNonQuery();
            cmd = new SqlCeCommand("delete from Items where ItemId > 3", conn);
            cmd.ExecuteNonQuery();
            cmd = new SqlCeCommand("delete from CustomersOrders where CustomerId > 3 ", conn);
            cmd.ExecuteNonQuery();
            cmd = new SqlCeCommand("Update CustomersOrders set QtyOrdered=100", conn);
            cmd.ExecuteNonQuery();

            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);
        }
    }
}

