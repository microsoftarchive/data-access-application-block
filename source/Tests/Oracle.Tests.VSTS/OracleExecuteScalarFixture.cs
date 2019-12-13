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
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    /// <summary>
    /// Test the ExecuteScalar method on the Database class
    /// </summary>
    [TestClass]
    public class OracleExecuteScalarFixture
    {
        static Database db;
        static ExecuteScalarFixture baseFixture;

        [TestInitialize]
        public void SetUp()
        {
            EnvironmentHelper.AssertOracleClientIsInstalled();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(OracleTestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");
            DbCommand command = db.GetSqlStringCommand("Select count(*) from region");

            baseFixture = new ExecuteScalarFixture(db, command);
        }

        [TestMethod]
        public void ExecuteScalarWithIDbCommand()
        {
            baseFixture.ExecuteScalarWithIDbCommand();
        }

        [TestMethod]
        public void ExecuteScalarWithIDbTransaction()
        {
            baseFixture.ExecuteScalarWithIDbTransaction();
        }

        [TestMethod]
        public void CanExecuteScalarDoAnInsertion()
        {
            baseFixture.CanExecuteScalarDoAnInsertion();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteScalarWithNullIDbCommand()
        {
            baseFixture.ExecuteScalarWithNullIDbCommand();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteScalarWithNullIDbTransaction()
        {
            baseFixture.ExecuteScalarWithNullIDbTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteScalarWithNullIDbCommandAndNullTransaction()
        {
            baseFixture.ExecuteScalarWithNullIDbCommandAndNullTransaction();
        }

        [TestMethod]
        public void ExecuteScalarWithCommandTextAndTypeInTransaction()
        {
            baseFixture.ExecuteScalarWithCommandTextAndTypeInTransaction();
        }
    }
}
