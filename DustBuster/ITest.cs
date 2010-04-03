/* Dust -- Copyright (C) 2009-2010 GameClay LLC
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation;
 * version 2.1 of the License.

 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.

 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
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