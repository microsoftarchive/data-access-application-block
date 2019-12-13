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

using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.SqlDatabaseFixtures
{
    [TestClass]
    public class GetDataAdapterFixture : EntLibFixtureBase
    {
        private string itemsXMLfile;

        public GetDataAdapterFixture()
            : base("ConfigFiles.OlderTestsConfiguration.config")
        {
        }

        [TestInitialize()]
        [DeploymentItem(@"Testfiles\items.xml")]
        public override void Initialize()
        {
            base.Initialize();

            itemsXMLfile = "items.xml";
        }

        [TestCleanup]
        public override void Cleanup()
        {
            DatabaseFactory.ClearDatabaseProviderFactory();
            base.Cleanup();
        }

        [TestMethod]
        [DeploymentItem(@"Testfiles\items.xml")]
        public void RecordsAreUpdatedWhenUsingDataAdapterAndTransactionCommit()
        {
            Database db = DatabaseFactory.CreateDatabase("DataSQLTest");
            DataSet itemsDataSet = new DataSet();
            DbCommand command = db.GetStoredProcCommand("ItemsGet");
            using (DbDataAdapter adapter = db.GetDataAdapter())
            {
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction() as DbTransaction;
                    command.Connection = conn;
                    ((IDbDataAdapter)adapter).SelectCommand = command;
                    command.Transaction = trans;
                    adapter.TableMappings.Add("Table", "Items");
                    ((IDbDataAdapter)adapter).Fill(itemsDataSet);
                    DataTable products = itemsDataSet.Tables["Items"];
                    products.Rows.Add(new object[] { 4, "New Row Added", 100, 100, 100 });
                    products.Rows[3].Delete();
                    DbCommand insertCommandWrapper = db.GetStoredProcCommandWithSourceColumns("AddItem", "ItemID", "ItemDescription", "Price", "QtyinHand", "QtyRequired");
                    DbCommand deleteCommandWrapper = db.GetStoredProcCommandWithSourceColumns("deleteItem", "ItemID");
                    DbCommand updateCommandWrapper = db.GetStoredProcCommandWithSourceColumns("UpdateItem", "ItemID", "ItemDescription");
                    insertCommandWrapper.Connection = conn;
                    insertCommandWrapper.Transaction = trans;
                    deleteCommandWrapper.Connection = conn;
                    deleteCommandWrapper.Transaction = trans;
                    updateCommandWrapper.Connection = conn;
                    updateCommandWrapper.Transaction = trans;
                    ((IDbDataAdapter)adapter).InsertCommand = insertCommandWrapper; //.Command;
                    ((IDbDataAdapter)adapter).DeleteCommand = deleteCommandWrapper; //.Command;
                    ((IDbDataAdapter)adapter).UpdateCommand = updateCommandWrapper; //.Command;
                    adapter.Update(itemsDataSet);
                    trans.Commit();
                }
            }
            DataSet da = new DataSet();
            da.ReadXml(itemsXMLfile);
            int count = da.Tables[0].Rows.Count;
            int index = 0;
            Assert.AreEqual(da.Tables[0].Rows.Count, itemsDataSet.Tables[0].Rows.Count);
            for (index = 0; index < da.Tables[0].Rows.Count; index++)
            {
                Assert.AreEqual(da.Tables[0].Rows[index][0].ToString().Trim(), itemsDataSet.Tables[0].Rows[index][1].ToString().Trim());
            }
        }
    }
}

