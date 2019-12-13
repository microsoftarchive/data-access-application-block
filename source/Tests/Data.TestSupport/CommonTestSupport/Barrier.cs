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
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport
{
    public class Barrier
    {
        private readonly object lockObj = new object();
        private readonly int originalCount;
        private int currentCount;

        public Barrier(int count)
        {
            originalCount = count;
            currentCount = count;
        }

        public void Await()
        {
            Await(TimeSpan.FromMilliseconds(Timeout.Infinite));
        }

        public void Await(int timeoutInMs)
        {
            Await(TimeSpan.FromMilliseconds(timeoutInMs));
        }

        public void Await(TimeSpan timeout)
        {
            lock(lockObj)
            {
                if(currentCount > 0)
                {
                    --currentCount;

                    if(currentCount == 0)
                    {
                        Monitor.PulseAll(lockObj);
                        currentCount = originalCount;
                    }
                    else
                    {
                        if(!Monitor.Wait(lockObj, timeout))
                        {
                            throw new TimeoutException();
                        }
                    }
                }
            }
        }
    }
}
