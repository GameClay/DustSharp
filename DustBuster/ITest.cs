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

namespace GameClay
{
    namespace DustBuster
    {

        /// <summary>
        /// Test result
        /// </summary>
        public enum Result
        {
            Passed,
            Failed
        }

        /// <summary>
        /// This is a really simple automated test.
        /// </summary>
        public interface ITest
        {
            /// <summary>
            /// Run the test, and return the result of the test.
            /// </summary>
            /// 
            /// <returns> Returns the <see cref="Result"/> of the test. </returns>
            Result RunTest(); // TODO: Pass in a test delegate which is fed a boolean?
        }
    }
}