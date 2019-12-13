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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration
{
    /// <summary>
    /// Oracle-specific configuration section.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "OracleConnectionSettingsDescription")]
    [ResourceDisplayName(typeof(DesignResources), "OracleConnectionSettingsDisplayName")]
    public class OracleConnectionSettings : SerializableConfigurationSection
    {
        private const string oracleConnectionDataCollectionProperty = "";

        /// <summary>
        /// The section name for the <see cref="OracleConnectionSettings"/>.
        /// </summary>
        public const string SectionName = "oracleConnectionSettings";

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleConnectionSettings"/> class with default values.
        /// </summary>
        public OracleConnectionSettings()
        {
        }

        /// <summary>
        /// Retrieves the <see cref="OracleConnectionSettings"/> from the configuration source.
        /// </summary>
        /// <param name="configurationSource">The configuration source to retrieve the configuration from.</param>
        /// <returns>The configuration section, or <see langword="null"/> (<b>Nothing</b> in Visual Basic) 
        /// if not present in the configuration source.</returns>
        public static OracleConnectionSettings GetSettings(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException("configurationSource");

            return configurationSource.GetSection(SectionName) as OracleConnectionSettings;
        }

        /// <summary>
        /// Collection of Oracle-specific connection information.
        /// </summary>
        [ConfigurationProperty(oracleConnectionDataCollectionProperty, IsRequired=false, IsDefaultCollection=true)]
        [ConfigurationCollection(typeof(OracleConnectionData))]
        [ResourceDescription(typeof(DesignResources), "OracleConnectionSettingsOracleConnectionsDataDescription")]
        [ResourceDisplayName(typeof(DesignResources), "OracleConnectionSettingsOracleConnectionsDataDisplayName")]
        public NamedElementCollection<OracleConnectionData> OracleConnectionsData
        {
            get { return (NamedElementCollection<OracleConnectionData>)base[oracleConnectionDataCollectionProperty]; }
        }
    }
}
