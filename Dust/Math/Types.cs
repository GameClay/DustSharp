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
#if DUST_XNA
using Vector4 = Microsoft.Xna.Framework.Vector4;
using Matrix = Microsoft.Xna.Framework.Matrix;
#endif

namespace GameClay.Dust
{
    internal class Math
    {
        public const float PI = 3.14159265358979323846f;
        public const float TwoPI = (2.0f * 3.14159265358979323846f);

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
