﻿/*
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
using System.Collections.Concurrent;
using System.Configuration;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// <para>Represents a factory for creating named instances of <see cref="Database"/> objects.</para>
    /// </summary>
    public class DatabaseProviderFactory
    {
        private DatabaseConfigurationBuilder builder;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DatabaseProviderFactory"/> class 
        /// with the default configuration source.</para>
        /// </summary>
        public DatabaseProviderFactory()
            : this(s => (ConfigurationSection)ConfigurationManager.GetSection(s))
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DatabaseProviderFactory"/> class 
        /// with the given configuration source.</para>
        /// </summary>
        /// <param name="configurationSource">The source for configuration information.</param>
        public DatabaseProviderFactory(IConfigurationSource configurationSource)
            : this(configurationSource.GetSection)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DatabaseProviderFactory"/> class 
        /// with the given configuration accessor.</para>
        /// </summary>
        /// <param name="configurationAccessor">The source for configuration information.</param>
        public DatabaseProviderFactory(Func<string, ConfigurationSection> configurationAccessor)
        {
            Guard.ArgumentNotNull(configurationAccessor, "configurationAccessor");

            this.builder = new DatabaseConfigurationBuilder(configurationAccessor);
        }

        /// <summary>
        /// Returns a new <see cref="Database"/> instance based on the default instance configuration.
        /// </summary>
        /// <returns>
        /// A new Database instance.
        /// </returns>
        public Database CreateDefault()
        {
            return this.builder.CreateDefault();
        }

        /// <summary>
        /// Returns a new <see cref="Database"/> instance based on the configuration for <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the required instance.</param>
        /// <returns>
        /// A new Database instance.
        /// </returns>
        public Database Create(string name)
        {
            Guard.ArgumentNotNullOrEmpty(name, "name");

            return this.builder.Create(name);
        }

        private class DatabaseConfigurationBuilder
        {
            private readonly DatabaseSyntheticConfigSettings settings;
            private readonly ConcurrentDictionary<string, DatabaseData> databases;

            public DatabaseConfigurationBuilder(Func<string, ConfigurationSection> configurationAccessor)
            {
                Guard.ArgumentNotNull(configurationAccessor, "configurationAccessor");

                this.settings = new DatabaseSyntheticConfigSettings(configurationAccessor);
                this.databases = new ConcurrentDictionary<string, DatabaseData>();
            }

            public Database CreateDefault()
            {
                var defaultDatabaseName = this.settings.DefaultDatabase;

                if (string.IsNullOrEmpty(defaultDatabaseName))
                {
                    throw new InvalidOperationException(Resources.ExceptionNoDefaultDatabaseDefined);
                }

                var data =
                    this.databases.GetOrAdd(
                        defaultDatabaseName,
                        n =>
                        {
                            try
                            {
                                return this.settings.GetDatabase(n);
                            }
                            catch (ConfigurationErrorsException e)
                            {
                                throw new InvalidOperationException(
                                    string.Format(CultureInfo.CurrentCulture, Resources.ExceptionDefaultDatabaseInvalid, n),
                                    e);
                            }
                        });

                return data.BuildDatabase();
            }

            public Database Create(string name)
            {
                Guard.ArgumentNotNull(name, "name");

                var data =
                    this.databases.GetOrAdd(
                        name,
                        n =>
                        {
                            try
                            {
                                return this.settings.GetDatabase(n);
                            }
                            catch (ConfigurationErrorsException e)
                            {
                                throw new InvalidOperationException(
                                    string.Format(CultureInfo.CurrentCulture, Resources.ExceptionDatabaseInvalid, n),
                                    e);
                            }
                        });

                return data.BuildDatabase();
            }
        }
    }
}
