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
    /// This is a very simple implementation of the IParameters interface
    /// and is used primarily for testing.
    /// </summary>
    public class Parameters : IParameters
    {
        #region IParameters Members

        public System.Func<float, T> GetParameterDelegate<T>(string parameterName) where T : new()
        {
            // Create default if needed
            if (!_parameterTypeDictionary.ContainsKey(typeof(T)))
                return SetParameter(parameterName, new T());

            Dictionary<string, System.Func<float, T>> paramDict = _parameterTypeDictionary[typeof(T)] as Dictionary<string, System.Func<float, T>>;

            // Create default if needed
            if(!paramDict.ContainsKey(parameterName))
                return SetParameter(parameterName, new T());

            return paramDict[parameterName];
        }

        #endregion

        public System.Func<float, T> SetParameter<T>(string parameterName, T val) where T : new()
        {
            // Create the parameter dictionary if needed
            if (!_parameterTypeDictionary.ContainsKey(typeof(T)))
                _parameterTypeDictionary.Add(typeof(T), new Dictionary<string, System.Func<float, T>>());

            Dictionary<string, System.Func<float, T>> paramDict = _parameterTypeDictionary[typeof(T)] as Dictionary<string, System.Func<float, T>>;

            if (!paramDict.ContainsKey(parameterName))
            {
                paramDict.Add(parameterName, new System.Func<float, T>(
                    (float t) =>
                    {
                        return val;
                    })
                );
            }

            return paramDict[parameterName];
        }

        public Parameters()
        {
            _parameterTypeDictionary = new Dictionary<System.Type, object>();
        }

        protected Dictionary<System.Type, object> _parameterTypeDictionary;
    }
}
