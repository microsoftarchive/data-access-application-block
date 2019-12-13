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
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestClass]
    public class SqlStoredProcedureCreatingFixture : StoredProcedureCreationBase
    {
        [TestInitialize]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            db = factory.CreateDefault();

            CompleteSetup(db);
        }

        [TestCleanup]
        public void TearDown()
        {
            Cleanup();
        }

        protected override void CreateStoredProcedure()
        {
            string storedProcedureCreation = "CREATE procedure [TestProc] " +
                                             "(@vCount int output, @vCustomerId varchar(15)) AS " +
                                             "set @vCount = (select count(*) from Orders where CustomerId = @vCustomerId)";

            DbCommand command = db.GetSqlStringCommand(storedProcedureCreation);
            db.ExecuteNonQuery(command);
        }

        protected override void DeleteStoredProcedure()
        {
            string storedProcedureDeletion = "Drop procedure TestProc";
            DbCommand command = db.GetSqlStringCommand(storedProcedureDeletion);
            db.ExecuteNonQuery(command);
        }

        [TestMethod]
        public void CanGetOutputValueFromStoredProcedure()
        {
            baseFixture.CanGetOutputValueFromStoredProcedure();
        }

        [TestMethod]
        public void CanGetOutputValueFromStoredProcedureWithCachedParameters()
        {
            baseFixture.CanGetOutputValueFromStoredProcedureWithCachedParameters();
        }

        [TestMethod(), ExpectedException(typeof(InvalidOperationException))]
        public void ArgumentExceptionWhenThereAreTooFewParameters()
        {
            baseFixture.ArgumentExceptionWhenThereAreTooFewParameters();
        }

        [TestMethod(), ExpectedException(typeof(InvalidOperationException))]
        public void ArgumentExceptionWhenThereAreTooManyParameters()
        {
            baseFixture.ArgumentExceptionWhenThereAreTooFewParameters();
        }

        [TestMethod(), ExpectedException(typeof(InvalidOperationException))]
        public void ExceptionThrownWhenReadingParametersFromCacheWithTooFewParameterValues()
        {
            baseFixture.ExceptionThrownWhenReadingParametersFromCacheWithTooFewParameterValues();
        }
    }
}
