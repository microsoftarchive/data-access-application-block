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

using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    /// <summary>
    /// Tests SqlDatabase.GetSqlStringCommand when using a parameterized sql string
    /// </summary>
    [TestClass]
    public class OracleParameterizedSqlStringFixture
    {
        Database db;

        [TestInitialize]
        public void SetUp()
        {
            EnvironmentHelper.AssertOracleClientIsInstalled();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(OracleTestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");
        }

        [TestMethod]
        public void ExecuteSqlStringCommandWithParameters()
        {
            string sql = "select * from Region where (RegionID=:Param1) and RegionDescription=:Param2";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, ":Param1", DbType.Int32, 1);
            db.AddInParameter(cmd, ":Param2", DbType.String, "Eastern");
            DataSet ds = db.ExecuteDataSet(cmd);
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(OracleException))]
        public void ExecuteSqlStringCommandWithNotEnoughParameterValues()
        {
            string sql = "select * from Region where RegionID=:Param1 and RegionDescription=:Param2";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, ":Param1", DbType.Int32, 1);
            DataSet ds = db.ExecuteDataSet(cmd);
        }

        [TestMethod]
        [ExpectedException(typeof(OracleException))]
        public void ExecuteSqlStringCommandWithTooManyParameterValues()
        {
            string sql = "select * from Region where RegionID=:Param1 and RegionDescription=:Param2";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, ":Param1", DbType.Int32, 1);
            db.AddInParameter(cmd, ":Param2", DbType.String, "Eastern");
            db.AddInParameter(cmd, ":Param3", DbType.String, "Western");
            DataSet ds = db.ExecuteDataSet(cmd);
        }

        [TestMethod]
        [ExpectedException(typeof(OracleException))]
        public void ExecuteSqlStringWithoutParametersButWithValues()
        {
            string sql = "select * from Region";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, ":Param1", DbType.Int32, 1);
            db.AddInParameter(cmd, ":Param2", DbType.String, "Eastern");
            DataSet ds = db.ExecuteDataSet(cmd);
            Assert.AreEqual(4, ds.Tables[0].Rows.Count);
        }
    }
}
