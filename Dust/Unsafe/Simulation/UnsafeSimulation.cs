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

namespace GameClay.Dust.Unsafe.Simulation
{

    public unsafe class UnmanagedSimulation : ISimulation
    {
        #region ISimulation implementation
        public unsafe void AdvanceTime(float dt)
        {
            fixed (float* pxstream = SystemData.PositionX)
            {
                fixed (float* pystream = SystemData.PositionY)
                {
                    fixed (float* pzstream = SystemData.PositionZ)
                    {
                        fixed (float* vxstream = SystemData.VelocityX)
                        {
                            fixed (float* vystream = SystemData.VelocityY)
                            {
                                fixed (float* vzstream = SystemData.VelocityZ)
                                {
                                    fixed (float* tstream = SystemData.TimeRemaining)
                                    {
                                        // The JIT should essentially 'remove' this conditional block
                                        if (System.IntPtr.Size == 4)
                                            AdvanceTime32(dt, pxstream, pystream, pzstream, vxstream, vystream, vzstream, tstream, _systemData._numParticles);
                                        else
                                            AdvanceTime64(dt, pxstream, pystream, pzstream, vxstream, vystream, vzstream, tstream, _systemData._numParticles);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [DllImport("UnmanagedSimulation32", EntryPoint = "AdvanceTime")]
        private extern static void AdvanceTime32(float dt, float* pX_stream, float* pY_stream, float* pZ_stream,
            float* vX_stream, float* vY_stream, float* vZ_stream, float* time_stream, int num_particles);

        [DllImport("UnmanagedSimulation64", EntryPoint = "AdvanceTime")]
        private extern static void AdvanceTime64(float dt, float* pX_stream, float* pY_stream, float* pZ_stream,
            float* vX_stream, float* vY_stream, float* vZ_stream, float* time_stream, int num_particles);


        public int AddParticles(ref ISystemData particlesToAdd)
        {
            // Copy particles in to our structure
            int numAdded = _systemData.CopyFrom(_systemData.NumParticles,
                                 ref particlesToAdd, 0, particlesToAdd.NumParticles);

            // Return number added
            return numAdded;
        }


        public void RemoveAllParticles()
        {
            // Clear out the system
            _systemData._numParticles = 0;

            // Zero-out the bounds
        }

        public object Bounds
        {
            get
            {
                return _worldBounds;
            }
        }

        public bool Is2D
        {
            get
            {
                return _is2D;
            }
            set
            {
                _is2D = value;
            }
        }

        public ISystemData SystemData
        {
            get
            {
                return _systemData;
            }
            set
            {
                if (value.GetType() != typeof(GameClay.Dust.SoAData))
                    throw new System.ArgumentException("Supplied ISystemData was not of type GameClay.Dust.SoAData.");
                    
                _systemData = (GameClay.Dust.SoAData)value;
            }
        }
        #endregion

        public UnmanagedSimulation(int maxNumParticles)
        {
            _systemData = new GameClay.Dust.SoAData(maxNumParticles);
            _worldBounds = new object();
            _is2D = false;
        }

        #region Data
        GameClay.Dust.SoAData _systemData;
        object _worldBounds;
        bool _is2D;
        #endregion
    }
}
