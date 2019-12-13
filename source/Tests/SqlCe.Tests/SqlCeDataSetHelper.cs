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
using System.Text;
using System.Data.Common;
using System.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Tests.VSTS
{
    class SqlCeDataSetHelper
    {
        public static void CreateDataAdapterCommandsDynamically(Database db, ref DbCommand insertCommand, ref DbCommand updateCommand, ref DbCommand deleteCommand)
        {
            CreateDataAdapterCommands(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }

        public static void CreateDataAdapterCommands(Database db, ref DbCommand insertCommand, ref DbCommand updateCommand, ref DbCommand deleteCommand)
        {
            insertCommand = db.GetSqlStringCommand("INSERT INTO Region VALUES(@RegionID, @RegionDescription)");
            updateCommand = db.GetSqlStringCommand("UPDATE region SET RegionDescription=@RegionDescription WHERE RegionID=@RegionId");
            deleteCommand = db.GetSqlStringCommand("DELETE FROM Region WHERE RegionID=@RegionID");

            db.AddInParameter(insertCommand, "@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            db.AddInParameter(insertCommand, "@RegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);

            db.AddInParameter(updateCommand, "@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            db.AddInParameter(updateCommand, "@RegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);

            db.AddInParameter(deleteCommand, "@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
        }

        public static void AddTestData(Database database)
        {
            SqlCeDatabase db = (SqlCeDatabase)database;

            db.ExecuteNonQuerySql("insert into Region values (99, 'Midwest');");
            db.ExecuteNonQuerySql("insert into Region values (100, 'Central Europe');");
            db.ExecuteNonQuerySql("insert into Region values (101, 'Middle East');");
            db.ExecuteNonQuerySql("insert into Region values (102, 'Australia')");
        }
    }
}
