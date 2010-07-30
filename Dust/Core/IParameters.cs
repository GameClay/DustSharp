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

using System.Collections.Generic;

namespace GameClay.Dust
{
    /// <summary>
    /// Bound to code which provides the value of a parameter which
    /// can change over time.
    /// </summary>
    ///
    /// <param name="t"> A normalized interpolation value at which to evaluate the parameter. </param>
    /// <returns> The value of the parameter with respect to t. </returns>
    public delegate T ParameterDelegate<T>(float t = 0.0f);

    public interface IParameters
    {
        /// <summary>
        /// Get a delegate bound to code which provides the value of a parameter.
        /// </summary>
        ///
        /// <remarks>
        /// This method should not be used each time a parameter is required.
        /// Instead, get the delegate for the parameter(s) required and then
        /// invoke as needed.
        /// </remarks>
        ///
        /// <param name="parameterName"> The name of the parameter being requested. </param>
        ParameterDelegate<T> GetParameterDelegate<T>(string parameterName);
    }
}