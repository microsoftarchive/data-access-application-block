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
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.CommonDatabase
{
    [TestClass]
    public class MultiThreadFixture : EntLibFixtureBase
    {
        private const int NumberOfThreads = 50;
        private int count = 0;

        public MultiThreadFixture()
            : base("ConfigFiles.OlderTestsConfiguration.config")
        {
        }

        [TestInitialize()]
        public override void Initialize()
        {
            ConnectionStringsSection section = (ConnectionStringsSection)base.ConfigurationSource.GetSection("connectionStrings");
            connStr = section.ConnectionStrings["DataSQLTest"].ConnectionString;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connStr;
            conn.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from Items ", conn);
            count = (int)cmd.ExecuteScalar();
            conn.Close();
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            DatabaseFactory.ClearDatabaseProviderFactory();
            base.Cleanup();
        }

        [TestMethod]
        public void MultipleThreadsInsertUsingTheirOwnDatabaseInstance()
        {
            Parallel.Invoke(Enumerable.Range(count + 1, NumberOfThreads).Select(i =>
                new Action(() => MultiThreadMethod(i))).ToArray());

            Database db = DatabaseFactory.CreateDatabase("DataSQLTest");
            int rows = (int)db.ExecuteScalar(CommandType.Text, "select count(*) from Items");
            Assert.AreEqual(NumberOfThreads, (rows - count));
        }

        public void MultiThreadMethod(int i)
        {
            Database db = DatabaseFactory.CreateDatabase("DataSQLTest");
            DbCommand dbCmd = db.GetStoredProcCommand("AddItem");
            db.AddInParameter(dbCmd, "@ItemID", DbType.Int32, i);
            db.AddInParameter(dbCmd, "@ItemDescription", DbType.String, "Item");
            db.AddInParameter(dbCmd, "@Price", DbType.Int16, 100);
            db.AddInParameter(dbCmd, "@QtyInHand", DbType.Int16, 1);
            db.AddInParameter(dbCmd, "@QtyRequired", DbType.Int16, 1);
            db.ExecuteNonQuery(dbCmd);
        }
    }
}

