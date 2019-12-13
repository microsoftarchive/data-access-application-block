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

using System.Data.SqlClient;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{

    /// <summary>
    /// Sql Server Database configuration options.
    /// </summary>
    public interface IDatabaseSqlDatabaseConfiguration : IDatabaseDefaultConnectionString, IDatabaseConfigurationProperties
    {
        /// <summary>
        /// Define a connection string using the <see cref="SqlConnectionStringBuilder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        IDatabaseConfigurationProperties WithConnectionString(SqlConnectionStringBuilder builder);
    }

    internal class SqlDatabaseConfigurationExtension : DatabaseConfigurationExtension, IDatabaseSqlDatabaseConfiguration
    {

        public SqlDatabaseConfigurationExtension(IDatabaseConfigurationProviders context)
            : base(context)
        {
            base.ConnectionString.ProviderName = DbProviderMapping.DefaultSqlProviderName;
        }

        public IDatabaseConfigurationProperties WithConnectionString(SqlConnectionStringBuilder builder)
        {
            return base.WithConnectionString(builder);
        }
    }
}
