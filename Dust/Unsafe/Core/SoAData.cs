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

using System.Runtime.InteropServices;

namespace GameClay.Dust.Unsafe
{
    /// <summary>
    /// An ISystemData implemented as a structure of arrays.
    /// </summary>
    public unsafe class SoAData : ISystemData, ISystemDataStreams, System.IDisposable
    {

        #region IDisposable implementation
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!__disposed)
            {
                if (disposing)
                {
                    // Release managed resources
                    _positionStreamX.Dispose();
                    _positionStreamY.Dispose();
                    _positionStreamZ.Dispose();

                    _lifespanStream.Dispose();
                    _timeRemainingStream.Dispose();
                    _massStream.Dispose();

                    _velocityStreamX.Dispose();
                    _velocityStreamY.Dispose();
                    _velocityStreamZ.Dispose();
                }

                // Release unmanaged resources
                Marshal.FreeHGlobal(_freeUD);
                _userDataStream = (int*)System.IntPtr.Zero;
            }
            __disposed = true;
        }

        private bool __disposed = false;
        #endregion

        #region ISystemData implementation
        public int NumParticles
        {
            get
            {
                return _numParticles;
            }
        }

        public int MaxNumParticles
        {
            get
            {
                return _maxNumParticles;
            }
        }

        public float[] PositionX
        {
            get
            {
                if (_positionStreamX_M == null || MaxNumParticles > _positionStreamX_M.Length)
                    _positionStreamX_M = new float[MaxNumParticles];

                Marshal.Copy((System.IntPtr)_positionStreamX, _positionStreamX_M, 0, MaxNumParticles);
                return _positionStreamX_M;
            }
        }

        public float[] PositionY
        {
            get
            {
                if (_positionStreamY_M == null || MaxNumParticles > _positionStreamY_M.Length)
                    _positionStreamY_M = new float[MaxNumParticles];

                Marshal.Copy((System.IntPtr)_positionStreamY, _positionStreamY_M, 0, MaxNumParticles);
                return _positionStreamY_M;
            }
        }

        public float[] PositionZ
        {
            get
            {
                if (_positionStreamZ_M == null || MaxNumParticles > _positionStreamZ_M.Length)
                    _positionStreamZ_M = new float[MaxNumParticles];

                Marshal.Copy((System.IntPtr)_positionStreamZ, _positionStreamZ_M, 0, MaxNumParticles);
                return _positionStreamZ_M;
            }
        }

        public float[] VelocityX
        {
            get
            {
                if (_velocityStreamX_M == null || MaxNumParticles > _velocityStreamX_M.Length)
                    _velocityStreamX_M = new float[MaxNumParticles];

                if (_velocityStreamZ != null)
                    Marshal.Copy((System.IntPtr)_velocityStreamX, _velocityStreamX_M, 0, MaxNumParticles);

                return _velocityStreamX_M;
            }
        }

        public float[] VelocityY
        {
            get
            {
                if (_velocityStreamY_M == null || MaxNumParticles > _velocityStreamY_M.Length)
                    _velocityStreamY_M = new float[MaxNumParticles];

                if (_velocityStreamY != null)
                    Marshal.Copy((System.IntPtr)_velocityStreamY, _velocityStreamY_M, 0, MaxNumParticles);

                return _velocityStreamY_M;
            }
        }

        public float[] VelocityZ
        {
            get
            {
                if (_velocityStreamZ_M == null || MaxNumParticles > _velocityStreamZ_M.Length)
                    _velocityStreamZ_M = new float[MaxNumParticles];

                if (_velocityStreamZ != null)
                    Marshal.Copy((System.IntPtr)_velocityStreamZ, _velocityStreamZ_M, 0, MaxNumParticles);

                return _velocityStreamZ_M;
            }
        }

        public float[] Mass
        {
            get
            {
                if (_massStream_M == null || MaxNumParticles > _massStream_M.Length)
                    _massStream_M = new float[MaxNumParticles];

                if (_massStream != null)
                    Marshal.Copy((System.IntPtr)_massStream, _massStream_M, 0, MaxNumParticles);

                return _massStream_M;
            }
        }

        public float[] Lifespan
        {
            get
            {
                if (_lifespanStream_M == null || MaxNumParticles > _lifespanStream_M.Length)
                    _lifespanStream_M = new float[MaxNumParticles];

                if (_lifespanStream != null)
                    Marshal.Copy((System.IntPtr)_lifespanStream, _lifespanStream_M, 0, MaxNumParticles);

                return _lifespanStream_M;
            }
        }

        public float[] TimeRemaining
        {
            get
            {
                if (_timeRemainingStream_M == null || MaxNumParticles > _timeRemainingStream_M.Length)
                    _timeRemainingStream_M = new float[MaxNumParticles];

                if (_timeRemainingStream != null)
                    Marshal.Copy((System.IntPtr)_timeRemainingStream, _timeRemainingStream_M, 0, MaxNumParticles);

                return _timeRemainingStream_M;
            }
        }

        public int[] UserData
        {
            get
            {
                if (_userDataStream_M == null || MaxNumParticles > _userDataStream_M.Length)
                    _userDataStream_M = new int[MaxNumParticles];

                if (_userDataStream != null)
                    Marshal.Copy((System.IntPtr)_userDataStream, _userDataStream_M, 0, MaxNumParticles);

                return _userDataStream_M;
            }
        }

        public SystemDataFlags Flags
        {
            get
            {
                return SystemDataFlags.UnmanagedData | SystemDataFlags.AlignedMemory;
            }
        }

        public int CopyFrom(int offset, ref ISystemData src, int srcOffset, int count)
        {
            int capacityLeft = MaxNumParticles - NumParticles;
            int numToCopy = count < capacityLeft ? count : capacityLeft;

            // Position
            Marshal.Copy(src.PositionX, srcOffset, _positionStreamX + offset, numToCopy);
            Marshal.Copy(src.PositionY, srcOffset, _positionStreamY + offset, numToCopy);
            Marshal.Copy(src.PositionZ, srcOffset, _positionStreamZ + offset, numToCopy);

            // Time
            if (_lifespanStream != null)
                Marshal.Copy(src.Lifespan, srcOffset, _lifespanStream + offset, numToCopy);
            if (_timeRemainingStream != null)
                Marshal.Copy(src.TimeRemaining, srcOffset, _timeRemainingStream + offset, numToCopy);

            // Velocity
            if (_velocityStreamX != null)
                Marshal.Copy(src.VelocityX, srcOffset, _velocityStreamX + offset, numToCopy);
            if (_velocityStreamY != null)
                Marshal.Copy(src.VelocityY, srcOffset, _velocityStreamY + offset, numToCopy);
            if (_velocityStreamZ != null)
                Marshal.Copy(src.VelocityZ, srcOffset, _velocityStreamZ + offset, numToCopy);

            // Mass
            if (_massStream != null)
                Marshal.Copy(src.Mass, srcOffset, _massStream + offset, numToCopy);

            // UserData
            if (_userDataStream != null)
                Marshal.Copy(src.UserData, srcOffset, (System.IntPtr)(_userDataStream + offset), numToCopy);

            // Update number of particles
            _numParticles += numToCopy;

            // Return number copied
            return numToCopy;
        }

        public void CopyElement(int srcIndex, int dstIndex)
        {
            // This isn't super awesome, in the SoA case, but what can ya do
            _positionStreamX[dstIndex] = _positionStreamX[srcIndex];
            _positionStreamY[dstIndex] = _positionStreamY[srcIndex];
            _positionStreamZ[dstIndex] = _positionStreamZ[srcIndex];

            if (_lifespanStream != null)
                _lifespanStream[dstIndex] = _lifespanStream[srcIndex];
            if (_timeRemainingStream != null)
                _timeRemainingStream[dstIndex] = _timeRemainingStream[srcIndex];

            if (_velocityStreamX != null)
                _velocityStreamX[dstIndex] = _velocityStreamX[srcIndex];
            if (_velocityStreamY != null)
                _velocityStreamY[dstIndex] = _velocityStreamY[srcIndex];
            if (_velocityStreamZ != null)
                _velocityStreamZ[dstIndex] = _velocityStreamZ[srcIndex];

            if (_massStream != null)
                _massStream[dstIndex] = _massStream[srcIndex];

            _userDataStream[dstIndex] = _userDataStream[srcIndex];
        }

        public void Clear()
        {
            _numParticles = 0;
        }
        #endregion

        #region ISystemDataStreams
        public float* PositionXStream
        {
            get
            {
                return _positionStreamX;
            }
        }

        public float* PositionYStream
        {
            get
            {
                return _positionStreamY;
            }
        }

        public float* PositionZStream
        {
            get
            {
                return _positionStreamZ;
            }
        }

        public int PositionStreamStride
        {
            get
            {
                return (((int)Flags & (int)SystemDataFlags.Vector3Position) > 0 ?
                    (((int)Flags & (int)SystemDataFlags.Vector4Position) > 0 ? 16 : 12) : 0);
            }
        }

        public float* VelocityXStream
        {
            get
            {
                return _velocityStreamX;
            }
        }

        public float* VelocityYStream
        {
            get
            {
                return _velocityStreamY;
            }
        }

        public float* VelocityZStream
        {
            get
            {
                return _velocityStreamZ;
            }
        }

        public int VelocityStreamStride
        {
            get
            {
                return (((int)Flags & (int)SystemDataFlags.Vector3Velocity) > 0 ?
                    (((int)Flags & (int)SystemDataFlags.Vector4Velocity) > 0 ? 16 : 12) : 0);
            }
        }

        public float* MassStream
        {
            get
            {
                return _massStream;
            }
        }

        public float* LifespanStream
        {
            get
            {
                return _lifespanStream;
            }
        }

        public float* TimeRemainingStream
        {
            get
            {
                return _timeRemainingStream;
            }
        }

        public int* UserDataStream
        {
            get
            {
                return _userDataStream;
            }
        }
        #endregion

        public SoAData(int maxNumParticles)
        {
            _numParticles = 0;
            _maxNumParticles = maxNumParticles;

            _positionStreamX = new AlignedHGlobalF(MaxNumParticles);
            _positionStreamY = new AlignedHGlobalF(MaxNumParticles);
            _positionStreamZ = new AlignedHGlobalF(MaxNumParticles);

            _lifespanStream = new AlignedHGlobalF(MaxNumParticles);
            _timeRemainingStream = new AlignedHGlobalF(MaxNumParticles);
            _massStream = new AlignedHGlobalF(MaxNumParticles);

            _velocityStreamX = new AlignedHGlobalF(MaxNumParticles);
            _velocityStreamY = new AlignedHGlobalF(MaxNumParticles);
            _velocityStreamZ = new AlignedHGlobalF(MaxNumParticles);

            //_userDataStream = (int*)AllocHGlobalAlignedF(MaxNumParticles, out _freeUD);

            // Assign null to these and lazy-allocate them
            _positionStreamX_M = null;
            _positionStreamY_M = null;
            _positionStreamZ_M = null;

            _lifespanStream_M = null;
            _timeRemainingStream_M = null;
            _massStream_M = null;

            _velocityStreamX_M = null;
            _velocityStreamY_M = null;
            _velocityStreamZ_M = null;

            _userDataStream_M = null;
        }

        ~SoAData()
        {
            Dispose(false);
        }

        #region Data
        public int _numParticles;
        public int _maxNumParticles;

        protected AlignedHGlobalF _positionStreamX;
        protected AlignedHGlobalF _positionStreamY;
        protected AlignedHGlobalF _positionStreamZ;

        protected AlignedHGlobalF _lifespanStream;
        protected AlignedHGlobalF _timeRemainingStream;
        protected AlignedHGlobalF _massStream;

        protected AlignedHGlobalF _velocityStreamX;
        protected AlignedHGlobalF _velocityStreamY;
        protected AlignedHGlobalF _velocityStreamZ;

        protected int* _userDataStream;

        protected System.IntPtr _freeUD;

        protected float[] _positionStreamX_M;
        protected float[] _positionStreamY_M;
        protected float[] _positionStreamZ_M;

        protected float[] _lifespanStream_M;
        protected float[] _timeRemainingStream_M;
        protected float[] _massStream_M;

        protected float[] _velocityStreamX_M;
        protected float[] _velocityStreamY_M;
        protected float[] _velocityStreamZ_M;

        protected int[] _userDataStream_M;
        #endregion
    }
}