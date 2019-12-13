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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;


namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration
{
    /// <summary>
    /// Oracle-specific connection information.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "OracleConnectionDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "OracleConnectionDataDisplayName")]
    [NameProperty("Name", NamePropertyDisplayFormat = "Oracle Packages for {0}")]
    public class OracleConnectionData : NamedConfigurationElement
    {
        private const string packagesProperty = "packages";

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleConnectionData"/> class with default values.
        /// </summary>
        public OracleConnectionData()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Reference(typeof(ConnectionStringSettingsCollection), typeof(ConnectionStringSettings))]
        [ResourceDescription(typeof(DesignResources), "OracleConnectionDataNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "OracleConnectionDataNameDisplayName")]
        public override string Name
        {
            get{ return base.Name; }
            set{ base.Name = value; }
        }

        /// <summary>
        /// Gets a collection of <see cref="OraclePackageData"/> objects.
        /// </summary>
        /// <value>
        /// A collection of <see cref="OraclePackageData"/> objects.
        /// </value>
        [ConfigurationProperty(packagesProperty, IsRequired = true)]
        [ConfigurationCollection(typeof(OraclePackageData))]
        [ResourceDescription(typeof(DesignResources), "OracleConnectionDataPackagesDescription")]
        [ResourceDisplayName(typeof(DesignResources), "OracleConnectionDataPackagesDisplayName")]
        [Editor(CommonDesignTime.EditorTypes.CollectionEditor, CommonDesignTime.EditorTypes.FrameworkElement)]
        [EnvironmentalOverrides(false)]
        [DesignTimeReadOnly(false)]
        public NamedElementCollection<OraclePackageData> Packages
        {
            get
            {
                return (NamedElementCollection<OraclePackageData>)base[packagesProperty];
            }
        }
    }
}
