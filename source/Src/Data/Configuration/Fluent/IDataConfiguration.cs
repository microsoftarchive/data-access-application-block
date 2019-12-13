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
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{
    ///<summary>
    /// Starting point for data configuration.
    ///</summary>
    /// <seealso cref="DataConfigurationSourceBuilderExtensions"/>
    public interface IDataConfiguration : IDatabaseConfiguration
    {
        /// <summary>
        /// Specify a custom provider name or alias to use.  This must
        /// map to the name of the invarient name specified by <see cref="DbProviderFactories"/>
        /// </summary>
        /// <remarks>If the provider is not mapped to a specific Enterprise Library <see cref="Database"/> class, then the <see cref="GenericDatabase"/> will be used.</remarks>
        /// <param name="providerName">The name of the database provider's invarient.</param>
        /// <returns></returns>
        IDatabaseProviderConfiguration WithProviderNamed(string providerName);
    }

    /// <summary>
    /// Defines the mapping options for providers.
    /// </summary>
    public interface IDatabaseProviderConfiguration : IDataConfiguration
    {
        /// <summary>
        /// The <see cref="Database"/> to map the provider to.
        /// </summary>
        /// <param name="databaseType">The <see cref="Database"/> type.</param>
        /// <returns></returns>
        /// <seealso cref="GenericDatabase"/>
        /// <seealso cref="SqlDatabase" />
        /// <seealso cref="OracleDatabase" />
        IDataConfiguration MappedToDatabase(Type databaseType);

        /// <summary>
        /// The <see cref="Database"/> to map the provider to.
        /// </summary>
        /// <typeparam name="T">Database type to map to</typeparam>
        /// <returns></returns>
        /// <seealso cref="GenericDatabase"/>
        /// <seealso cref="SqlDatabase" />
        /// <seealso cref="OracleDatabase" />
        IDataConfiguration MappedToDatabase<T>() where T : Data.Database;
    }
}
