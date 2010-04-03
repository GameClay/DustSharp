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
