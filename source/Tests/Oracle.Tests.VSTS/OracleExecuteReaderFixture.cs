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
using System.Data.OracleClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    /// <summary>
    /// Test the ExecuteReader method on the Database class
    /// </summary>
    [TestClass]
    public class OracleExecuteReaderFixture
    {
        const string insertString = "Insert into Region values (99, 'Midwest')";
        const string queryString = "Select * from Region";
        Database db;
        ExecuteReaderFixture baseFixture;

        [TestInitialize]
        public void SetUp()
        {
            EnvironmentHelper.AssertOracleClientIsInstalled();

            DatabaseProviderFactory factory = new DatabaseProviderFactory(OracleTestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");

            DbCommand insertCommand = db.GetSqlStringCommand(insertString);
            DbCommand queryCommand = db.GetSqlStringCommand(queryString);

            baseFixture = new ExecuteReaderFixture(db, insertString, insertCommand, queryString, queryCommand);
        }

        [TestMethod]
        public void CanExecuteReaderWithCommandText()
        {
            baseFixture.CanExecuteReaderWithCommandText();
        }

        [TestMethod]
        public void Bug869Test()
        {
            object[] paramarray = new object[2];
            paramarray[0] = "BLAUS";
            paramarray[1] = null;

            using (IDataReader dataReader = db.ExecuteReader("GetCustomersTest", paramarray))
            {
                while (dataReader.Read())
                {
                    // Get the value of the 'Name' column in the DataReader
                    Assert.IsNotNull(dataReader["ContactName"]);
                }
                dataReader.Close();
            }
        }

        [TestMethod]
        public void CanExecuteReaderFromDbCommand()
        {
            baseFixture.CanExecuteReaderFromDbCommand();
        }

        [TestMethod]
        public void WhatGetsReturnedWhenWeDoAnInsertThroughDbCommandExecute()
        {
            baseFixture.WhatGetsReturnedWhenWeDoAnInsertThroughDbCommandExecute();
        }

        [TestMethod]
        public void CanExecuteQueryThroughDataReaderUsingTransaction()
        {
            baseFixture.CanExecuteQueryThroughDataReaderUsingTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteQueryThroughDataReaderUsingNullCommandThrows()
        {
            baseFixture.ExecuteQueryThroughDataReaderUsingNullCommandThrows();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteQueryThroughDataReaderUsingNullCommandAndNullTransactionThrows()
        {
            baseFixture.ExecuteQueryThroughDataReaderUsingNullCommandAndNullTransactionThrows();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteQueryThroughDataReaderUsingNullTransactionThrows()
        {
            baseFixture.ExecuteQueryThroughDataReaderUsingNullTransactionThrows();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteReaderWithNullCommand()
        {
            baseFixture.ExecuteReaderWithNullCommand();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullQueryStringTest()
        {
            baseFixture.NullQueryStringTest();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyQueryStringTest()
        {
            baseFixture.EmptyQueryStringTest();
        }

        [TestMethod]
        public void CanGetTheInnerDataReader()
        {
            DbCommand queryCommand = db.GetSqlStringCommand(queryString);
            IDataReader reader = db.ExecuteReader(queryCommand);
            string accumulator = "";

            int descriptionIndex = reader.GetOrdinal("RegionDescription");
            OracleDataReader innerReader = ((OracleDataReaderWrapper)reader).InnerReader;
            Assert.IsNotNull(innerReader);

            while (reader.Read())
            {
                accumulator += innerReader.GetOracleString(descriptionIndex).Value.Trim();
            }

            reader.Close();

            Assert.AreEqual("EasternWesternNorthernSouthern", accumulator);
            Assert.AreEqual(ConnectionState.Closed, queryCommand.Connection.State);
        }
    }
}
