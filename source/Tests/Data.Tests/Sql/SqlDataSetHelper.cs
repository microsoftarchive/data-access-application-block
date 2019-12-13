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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    internal sealed class SqlDataSetHelper
    {
        public static void CreateDataAdapterCommandsDynamically(Database db, ref DbCommand insertCommand, ref DbCommand updateCommand, ref DbCommand deleteCommand)
        {
            insertCommand = db.GetStoredProcCommandWithSourceColumns("RegionInsert", "RegionID", "RegionDescription");
            updateCommand = db.GetStoredProcCommandWithSourceColumns("RegionUpdate", "RegionID", "RegionDescription");
            deleteCommand = db.GetStoredProcCommandWithSourceColumns("RegionDelete", "RegionID");
        }

        public static void CreateDataAdapterCommands(Database db, ref DbCommand insertCommand, ref DbCommand updateCommand, ref DbCommand deleteCommand)
        {
            insertCommand = db.GetStoredProcCommand("RegionInsert");
            updateCommand = db.GetStoredProcCommand("RegionUpdate");
            deleteCommand = db.GetStoredProcCommand("RegionDelete");

            db.AddInParameter(insertCommand, "@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            db.AddInParameter(insertCommand, "@RegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);

            db.AddInParameter(updateCommand, "@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            db.AddInParameter(updateCommand, "@RegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);

            db.AddInParameter(deleteCommand, "@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
        }

        public static void CreateStoredProcedures(Database db)
        {
            string sql = "create procedure RegionSelect as " +
                "select * from Region Order by RegionId";

            DbCommand command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);

            sql = "create procedure RegionInsert (@RegionID int, @RegionDescription varchar(100) ) as " +
                "insert into Region values(@RegionID, @RegionDescription)";

            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);

            sql = "create procedure RegionUpdate (@RegionID int, @RegionDescription varchar(100) ) as " +
                "update Region set RegionDescription = @RegionDescription where RegionID = @RegionID";

            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);

            sql = "create procedure RegionDelete (@RegionID int) as " +
                "delete from Region where RegionID = @RegionID";

            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);
        }

        public static void DeleteStoredProcedures(Database db)
        {
            DbCommand command;
            string sql = "drop procedure RegionSelect; " +
                "drop procedure RegionInsert; " +
                "drop procedure RegionDelete; " +
                "drop procedure RegionUpdate; ";
            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);
        }

        public static void AddTestData(Database db)
        {
            string sql =
                "insert into Region values (99, 'Midwest');" +
                    "insert into Region values (100, 'Central Europe');" +
                    "insert into Region values (101, 'Middle East');" +
                    "insert into Region values (102, 'Australia')";
            DbCommand testDataInsertion = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(testDataInsertion);
        }
    }
}

