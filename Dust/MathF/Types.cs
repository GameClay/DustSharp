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

namespace GameClay.Dust
{
    internal class MathF
    {
        public const float PI = 3.14159265358979323846f;
        public const float TwoPI = (2.0f * PI);

        public static float Cos(float f)
        {
            return (float)System.Math.Cos(f);
        }

        public static float Sin(float f)
        {
            return (float)System.Math.Sin(f);
        }

        public static float Sqrt(float f)
        {
            return (float)System.Math.Sqrt(f);
        }
    }
}
