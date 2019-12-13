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

using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlServerCe;

namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe
{
    /// <summary>
    ///        This class provides a primitive connection pool for <see cref="SqlCeDatabase"/>.
    /// </summary>
    public class SqlCeConnectionPool
    {
        /// <summary>
        ///        Keeps a list of "keep alive" connections.
        /// </summary>
        protected static Dictionary<string, DatabaseConnectionWrapper> connections = new Dictionary<string, DatabaseConnectionWrapper>();

        /// <summary>
        ///        Closes the "keep alive" connection that is used by all databases with the same connection
        ///        string as the one you provide.
        /// </summary>
        /// <param name="database">The database with the connection string that defines which connections should be closed.</param>
        public static void CloseSharedConnection(Database database)
        {
            DatabaseConnectionWrapper connection;
            string connectionString = database.ConnectionStringWithoutCredentials;

            lock (connections)
            {
                if (connections.TryGetValue(connectionString, out connection))
                {
                    connection.Dispose();
                    connections.Remove(connectionString);
                }
            }
        }

        /// <summary>
        ///        Closes all "keep alive" connections for all database instanced.
        /// </summary>
        public static void CloseSharedConnections()
        {
            lock (connections)
            {
                foreach (KeyValuePair<string, DatabaseConnectionWrapper> pair in connections)
                {
                    pair.Value.Dispose();
                }
                connections.Clear();
            }
        }

        /// <summary>
        ///        Creates a new connection. If this is the first connection,  it also creates an extra
        ///        "Keep Alive" connection to keep the database open.
        /// If <paramref name="usePooledConnection"/> is true, than if this connection has been opened before,
        /// the connection from the pool will be returned rather than creating a new one.
        /// </summary>
        /// <param name="db">The database instance that will be used to create a connection.</param>
        /// <param name="usePooledConnection">If true, return an already created connection for this object. If
        /// false, always create a new one.</param>
        /// <returns>A new connection.</returns>
        public static DatabaseConnectionWrapper CreateConnection(SqlCeDatabase db, bool usePooledConnection)
        {
            string connectionString = db.ConnectionStringWithoutCredentials;
            DatabaseConnectionWrapper connection;
            lock (connections)
            {
                if (!connections.TryGetValue(connectionString, out connection))
                {
                    //
                    // We have to test this again in case another thread added a connection.
                    //
                    if (!connections.ContainsKey(connectionString))
                    {
                        DbConnection keepAliveConnection = new SqlCeConnection();
                        db.SetConnectionString(keepAliveConnection);
                        keepAliveConnection.Open();
                        connection = new DatabaseConnectionWrapper(keepAliveConnection);
                        connections.Add(connectionString, connection);
                    }
                }
                if (usePooledConnection)
                {
                    connection.AddRef();
                    return connection;
                }

                return new DatabaseConnectionWrapper(new SqlCeConnection());
            }
        }

        /// <summary>
        ///        Creates a new connection. If this is the first connection,  it also creates an extra
        ///        "Keep Alive" connection to keep the database open. Always returns a new <see cref="SqlCeConnection"/>
        ///     object, not a pooled one.
        /// </summary>
        /// <param name="db">The database instance that will be used to create a connection.</param>
        /// <returns>A new connection.</returns>
        public static DatabaseConnectionWrapper CreateConnection(SqlCeDatabase db)
        {
            return CreateConnection(db, false);
        }
    }
}
