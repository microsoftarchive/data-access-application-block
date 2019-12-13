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
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class SqlCeParameterizedSqlStringFixture
    {
        Database db;

        [TestInitialize]
        public void SetUp()
        {
            TestConnectionString testConnection = new TestConnectionString();
            testConnection.CopyFile();
            db = new SqlCeDatabase(testConnection.ConnectionString);
        }

        [TestCleanup]
        public void TearDown()
        {
            SqlCeConnectionPool.CloseSharedConnections();
        }

        [TestMethod]
        public void ExecuteSqlStringCommandWithParameters()
        {
            string sql = "select * from Region where (RegionID=@Param1) and RegionDescription=@Param2";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@Param1", DbType.Int32, 1);
            db.AddInParameter(cmd, "@Param2", DbType.String, "Eastern");
            DataSet ds = db.ExecuteDataSet(cmd);
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);
        }

        [TestMethod]
        public void ExecuteSqlStringCommandWithNotEnoughParameterValues()
        {
            try
            {
                string sql = "select * from Region where RegionID=@Param1 and RegionDescription=@Param2";
                DbCommand cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "@Param1", DbType.Int32, 1);
                db.ExecuteDataSet(cmd);
            }
            catch (IndexOutOfRangeException)
            {
                // Sql CE 3.5 SP1 throws this
            }
            catch (SqlCeException)
            {    
                // Older Sql CE throws this
            }
        }

        [TestMethod]
        public void ExecuteSqlStringCommandWithTooManyParameterValues()
        {
            try
            {
                string sql = "select * from Region where RegionID=@Param1 and RegionDescription=@Param2";
                DbCommand cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "@Param1", DbType.Int32, 1);
                db.AddInParameter(cmd, "@Param2", DbType.String, "Eastern");
                db.AddInParameter(cmd, "@Param2", DbType.String, "Western");
                db.ExecuteDataSet(cmd);
            }
            catch (ArgumentException)
            {
                // Sql CE 3.5 SP1 throws this
            }
            catch(SqlCeException)
            {
                // Older Sql CE throws this
            }
        }

        [TestMethod]
        public void ExecuteSqlStringCommandWithDoubleAtSymbol()
        {
            string sql = "select *, @@IDENTITY from Region where RegionID=@ID and RegionDescription=@Desc";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@ID", DbType.Int32, 1);
            db.AddInParameter(cmd, "@Desc", DbType.String, "Eastern");
            DataSet ds = db.ExecuteDataSet(cmd);
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);
        }

        [TestMethod]
        public void ExecuteSqlStringWithoutParametersButWithValues()
        {
            string sql = "select * from Region";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@Param1", DbType.Int32, 1);
            db.AddInParameter(cmd, "@Param2", DbType.String, "Eastern");
            DataSet ds = db.ExecuteDataSet(cmd);
            Assert.AreEqual(4, ds.Tables[0].Rows.Count);
        }
    }
}
