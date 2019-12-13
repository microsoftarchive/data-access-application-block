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
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    ///<summary>
    /// Provides extensions for common database providers.
    ///</summary>
    public static class DatabaseProviderExtensions
    {
        ///<summary>
        /// A Sql database for use with the System.Data.SqlClient namespace.
        ///</summary>
        /// <param name="context">Configuration context</param>
        ///<returns></returns>
        /// <seealso cref="System.Data.SqlClient"/>
        public static IDatabaseSqlDatabaseConfiguration ASqlDatabase(this IDatabaseConfigurationProviders context)
        {
            return new SqlDatabaseConfigurationExtension(context);
        }

        /// <summary>
        /// A Sql CE database for use with the System.Data.SqlServerCe namespace.
        /// </summary>
        /// <param name="context">Configuration context</param>
        /// <returns></returns>   
        public static IDatabaseSqlCeDatabaseConfiguration ASqlCeDatabase(this IDatabaseConfigurationProviders context)
        {
            return new SqlCeDatabaseConfigurationExtension(context);
        }

        /// <summary>
        /// An OleDb database for use with the <see cref="System.Data.OleDb"/> namespace.
        /// </summary>
        /// <returns></returns>
        public static IOleDbDatabaseConfiguration AnOleDbDatabase(this IDatabaseConfigurationProviders context)
        {
            return new OleDbConfigurationExtension(context);
        }

        /// <summary>
        /// An Odbc database for use with the <see cref="System.Data.Odbc"/> namespace.
        /// </summary>
        /// <returns></returns>
        public static IOdbcDatabaseConfiguration AnOdbcDatabase(this IDatabaseConfigurationProviders context)
        {
            return new OdbcConfigurationExtension(context);
        }

        ///<summary>
        /// An Oracle database for use with the System.Data.OracleClient namespace.
        ///</summary>
        ///<returns></returns>
        ///<seealso cref="System.Data.OracleClient"/>
        [Obsolete("OracleDatabase has been deprecated. http://go.microsoft.com/fwlink/?LinkID=144260", false)]
        public static IDatabaseOracleConfiguration AnOracleDatabase(this IDatabaseConfigurationProviders context)
        {
            return new OracleConfigurationExtension(context);
        }

        ///<summary>
        /// A database with the specified database provider name.
        ///</summary>
        /// <param name="context">Extension context for fluent-interface</param>
        /// <param name="providerName">The provider name to use for this database connection</param>
        ///<returns></returns>
        /// <seealso cref="DbProviderFactories"/>
        public static IDatabaseAnotherDatabaseConfiguration AnotherDatabaseType(this IDatabaseConfigurationProviders context, string providerName)
        {
            return new AnotherDatabaseConfigurationExtensions(context, providerName);
        }
    }
}
