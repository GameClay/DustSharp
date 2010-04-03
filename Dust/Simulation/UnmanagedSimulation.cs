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
using System.Runtime.InteropServices;

namespace GameClay.Dust
{

    public class UnmanagedSimulation : ISimulation
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
                                        fixed (int* numparticles = &_systemData._numParticles)
                                        {
                                            AdvanceTime(dt, pxstream, pystream, pzstream, vxstream, vystream, vzstream, tstream, numparticles);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [DllImport("UnmanagedSimulation.dll")]
        private unsafe extern static void AdvanceTime(float dt, float* pX_stream, float* pY_stream, float* pZ_stream,
            float* vX_stream, float* vY_stream, float* vZ_stream, float* time_stream, int* num_particles);


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
#if DEBUG
                if (value.GetType() != typeof(SoAData))
                    throw new System.ArgumentException("Supplied ISystemData was not of type SoAData.");
#endif
                _systemData = (SoAData)value;
            }
        }
        #endregion

        public UnmanagedSimulation(int maxNumParticles)
        {
            _systemData = new SoAData(maxNumParticles);
            _worldBounds = new object();
            _is2D = false;
        }

        #region Data
        SoAData _systemData;
        object _worldBounds;
        bool _is2D;
        #endregion
    }
}
