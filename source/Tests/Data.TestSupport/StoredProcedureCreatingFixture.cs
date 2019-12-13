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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.TestSupport
{
    /// <summary>
    /// Use the Data Access Application Block to execute a create a stored procedure script using ExecNonQuery.
    /// </summary>    
    public class StoredProcedureCreatingFixture
    {
        Database db;

        public StoredProcedureCreatingFixture(Database db)
        {
            this.db = db;
        }

        public void ArgumentExceptionWhenThereAreTooFewParameters()
        {
            DbCommand storedProcedure = db.GetStoredProcCommand("TestProc", "ALFKI");
            db.ExecuteNonQuery(storedProcedure);
        }

        public void ArgumentExceptionWhenThereAreTooManyParameters()
        {
            DbCommand invalidCommand = db.GetStoredProcCommand("TestProc", "ALFKI", "EIEIO", "Hello");
            db.ExecuteNonQuery(invalidCommand);
        }

        public void CanGetOutputValueFromStoredProcedure()
        {
            DbCommand storedProcedure = db.GetStoredProcCommand("TestProc", null, "ALFKI");
            db.ExecuteNonQuery(storedProcedure);

            int resultCount = Convert.ToInt32(db.GetParameterValue(storedProcedure, "vCount"));
            Assert.AreEqual(6, resultCount);
        }

        public void CanGetOutputValueFromStoredProcedureWithCachedParameters()
        {
            DbCommand storedProcedure = db.GetStoredProcCommand("TestProc", null, "ALFKI");
            db.ExecuteNonQuery(storedProcedure);

            DbCommand duplicateStoredProcedure = db.GetStoredProcCommand("TestProc", null, "CHOPS");
            db.ExecuteNonQuery(duplicateStoredProcedure);

            int resultCount = Convert.ToInt32(db.GetParameterValue(duplicateStoredProcedure, "vCount"));
            Assert.AreEqual(8, resultCount);
        }

        public void ExceptionThrownWhenReadingParametersFromCacheWithTooFewParameterValues()
        {
            DbCommand storedProcedure = db.GetStoredProcCommand("TestProc", null, "ALFKI");
            db.ExecuteNonQuery(storedProcedure);

            DbCommand duplicateStoredProcedure = db.GetStoredProcCommand("TestProc", "CHOPS");
            db.ExecuteNonQuery(duplicateStoredProcedure);
        }
    }
}
