/* Dust -- Copyright (C) 2009-2010 GameClay LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;

namespace GameClay
{
    namespace DustBuster
    {
        class MainClass
        {
            public static int Main(string[] args)
            {
                // This is pretty braindead, but it works
                List<ITest> testList = new List<ITest>();

                // Add some tests
                testList.Add(new SoADataTest());
                testList.Add(new SystemPerformanceTester());

                // Run all the tests
                Console.WriteLine("DustBuster running " + testList.Count + " automated tests.");
                int testsPassed = 0;
                int testsFailed = 0;

                foreach (ITest test in testList)
                {
                    Console.WriteLine(" - " + test);
                    switch (test.RunTest())
                    {
                        case Result.Passed:
                            Console.WriteLine(" passed!\n");
                            testsPassed++;
                            break;

                        case Result.Failed:
                            Console.WriteLine(" failed!\n");
                            testsFailed++;
                            break;
                    }
                }

                // Report to the console (kind of lame)
                Console.WriteLine("\nDustBuster results:");
                Console.WriteLine("   " + testsPassed + " tests passed.");
                Console.WriteLine("   " + testsFailed + " tests failed.");

                // Return 0 if no tests failed
                return testsFailed;
            }
        }
    }
}
