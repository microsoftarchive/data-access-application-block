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
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT
{
    public class DbAsyncState
    {
        public DbAsyncState(Database db)
        {
            Database = db;
            AutoResetEvent = new AutoResetEvent(false);
        }
        public DbAsyncState(Database db, object accessor)
        {
            Database = db;
            AutoResetEvent = new AutoResetEvent(false);
            Accessor = accessor;

        }
        public Database Database { get; set; }
        public Exception Exception { get; set; }
        public AutoResetEvent AutoResetEvent { get; set; }
        public object State { get; set; }
        public DaabAsyncResult AsyncResult { get; set; }
        public object Accessor { get; set; }
    }
}

