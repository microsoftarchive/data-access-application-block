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
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT
{
    public static class ServiceHelper
    {
        public static void Stop(string serviceName)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "net";
                process.StartInfo.Arguments = "stop " + serviceName;
                process.Start();
                process.WaitForExit(30000);
            }
        }

        public static void Start(string serviceName)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "net";
                process.StartInfo.Arguments = "start " + serviceName;
                process.Start();
                process.WaitForExit(30000);
            }

        }

        public static void Restart(string serviceName)
        {
            Restart(serviceName, 0);

        }

        public static void Restart(string serviceName, int seconds)
        {
            Start(serviceName);
            System.Threading.Thread.Sleep(seconds);
            Stop(serviceName);

        }
    }
}

