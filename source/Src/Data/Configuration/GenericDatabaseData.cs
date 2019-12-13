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
using System.Configuration;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
    /// <summary>
    /// Describes a <see cref="GenericDatabase"/> instance, aggregating information from a 
    /// <see cref="ConnectionStringSettings"/>.
    /// </summary>
    public class GenericDatabaseData : DatabaseData
    {
        ///<summary>
        /// Initializes a new instance of the <see cref="GenericDatabaseData"/> class with a connection string and a configuration
        /// source.
        ///</summary>
        ///<param name="connectionStringSettings">The <see cref="ConnectionStringSettings"/> for the represented database.</param>
        ///<param name="configurationSource">The <see cref="IConfigurationSource"/> from which additional information can 
        /// be retrieved if necessary.</param>
        public GenericDatabaseData(ConnectionStringSettings connectionStringSettings, Func<string, ConfigurationSection> configurationSource)
            : base(connectionStringSettings, configurationSource)
        {
        }

        /// <summary>
        /// Gets the name of the ADO.NET provider for the represented database.
        /// </summary>
        public string ProviderName
        {
            get { return ConnectionStringSettings.ProviderName; }
        }

        /// <summary>
        /// Builds the <see cref="Database" /> represented by this configuration object.
        /// </summary>
        /// <returns>
        /// A database.
        /// </returns>
        public override Database BuildDatabase()
        {
            return new GenericDatabase(this.ConnectionString, DbProviderFactories.GetFactory(this.ProviderName));
        }
    }
}
