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
using System.Runtime.InteropServices;

namespace GameClay
{
    public unsafe class AlignedHGlobalF : IDisposable
    {
        public float* Pointer
        {
            get
            {
                return _fPtr;
            }
        }

        public float this[int index]
        {
            get
            {
                return _fPtr[index];
            }
            set
            {
                _fPtr[index] = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        ///
        /// <param name="n">
        /// The number of floats to allocate.
        /// </param>
        public AlignedHGlobalF(int n)
        {
            _freePtr = Marshal.AllocHGlobal(sizeof(float) * n + 15);
            _fPtr = (float*)(((int)_freePtr + 15) & ~ 0x0F);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~AlignedHGlobalF()
        {
            Dispose(false);
        }

        #region Operator overloads
        /// <summary>
        /// Implicit conversion to float*
        /// </summary>
        public static implicit operator float*(AlignedHGlobalF alignedF)
        {
            return alignedF._fPtr;
        }

        /// <summary>
        /// Implicit conversion to <see cref="IntPtr"/>
        /// </summary>
        public static implicit operator IntPtr(AlignedHGlobalF alignedF)
        {
            return (IntPtr)alignedF._fPtr;
        }

        /// <summary>
        /// Pointer addition support
        /// </summary>
        ///
        /// <returns>
        /// The aligned pointer offset by the specified value.
        /// </returns>
        public static IntPtr operator +(AlignedHGlobalF alignedF, int offset)
        {
            return (IntPtr)(alignedF._fPtr + offset);
        }
        #endregion

        #region Data
        protected IntPtr _freePtr;
        protected float* _fPtr;
        #endregion

        #region IDisposable implementation
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Release managed resources
                }

                // Release unmanaged resources
                if(_freePtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(_freePtr);
                _freePtr = IntPtr.Zero;
                _fPtr = (float*)IntPtr.Zero;
            }
            _disposed = true;
        }

        private bool _disposed = false;
        #endregion
    }
}

