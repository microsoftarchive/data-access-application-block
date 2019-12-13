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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// The <see cref="GenericDatabase"/> is used when no specific behavior is required or known for a database.
    /// </summary>
    /// <remarks>
    /// This database exposes the <see cref="DbProviderFactory"/> used to allow for a provider 
    /// agnostic programming model.
    /// </remarks>
    [ConfigurationElementType(typeof(GenericDatabaseData))]
    public class GenericDatabase : Database
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDatabase"/> class with a connection string and 
        /// a provider factory.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbProviderFactory">The provider factory.</param>
        public GenericDatabase(string connectionString, DbProviderFactory dbProviderFactory)
            : base(connectionString, dbProviderFactory)
        {
        }

        /// <summary>
        /// This operation is not supported in this class.
        /// </summary>
        /// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>
        /// <remarks>There is no generic way to do it, the operation is not implemented for <see cref="GenericDatabase"/>.</remarks>
        /// <exception cref="NotSupportedException">Thrown whenever this method is called.</exception>
        protected override void DeriveParameters(DbCommand discoveryCommand)
        {
            throw new NotSupportedException(Resources.ExceptionParameterDiscoveryNotSupportedOnGenericDatabase);
        }
    }
}
