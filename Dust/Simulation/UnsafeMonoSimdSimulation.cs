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
#define DUST_MONO
#if DUST_MONO
using System.Collections.Generic;
using Mono.Simd;

namespace GameClay.Dust.Mono
{
    /// <summary>
    /// The standard simulation
    /// </summary>
    public unsafe class UnsafeSimdSimulation : ISimulation
    {
        #region ISimulation implementation
        public void AdvanceTime (float dt)
        {
            // Reset the bounds of the system

            Vector4f dt_v = new Vector4f (dt);
            
            fixed (float* xPosStream = _systemData._positionStreamX)
            {
                fixed (float* yPosStream = _systemData._positionStreamY)
                {
                    fixed (float* zPosStream = _systemData._positionStreamZ)
                    {
                        fixed (float* xVelStream = _systemData._velocityStreamX)
                        {
                            fixed (float* yVelStream = _systemData._velocityStreamY)
                            {
                                fixed (float* zVelStream = _systemData._velocityStreamZ)
                                {
                                    fixed (float* timeStream = _systemData._timeRemainingStream)
                                    {
                                        // Temporary registers
                                        Vector4f ts_reg, vX_reg, vY_reg, vZ_reg, pX_reg, pY_reg, pZ_reg;
                                        
                                        // Process the particle system
                                        int numBatches = _systemData.NumParticles / 4;
                                        for (int i = 0; i < numBatches; i++)
                                        {
                                            // Load streams
                                            int streamIdx = i * 4;
                                            ts_reg = Vector4f.LoadAligned ((Vector4f*)(timeStream + streamIdx));

                                            vX_reg = Vector4f.LoadAligned ((Vector4f*)(xVelStream + streamIdx));
                                            vY_reg = Vector4f.LoadAligned ((Vector4f*)(yVelStream + streamIdx));
                                            vZ_reg = Vector4f.LoadAligned ((Vector4f*)(zVelStream + streamIdx));

                                            pX_reg = Vector4f.LoadAligned ((Vector4f*)(xPosStream + streamIdx));
                                            pY_reg = Vector4f.LoadAligned ((Vector4f*)(yPosStream + streamIdx));
                                            pZ_reg = Vector4f.LoadAligned ((Vector4f*)(zPosStream + streamIdx));

                                            // Decrement time remaining
                                            ts_reg = VectorOperations.Max(ts_reg - dt_v, Vector4f.Zero);
                                            
                                            // Update position
                                            Vector4f tx = vX_reg * dt_v;
                                            Vector4f ty = vY_reg * dt_v;
                                            Vector4f tz = vZ_reg * dt_v;

                                            // Decrement time remaining
                                            Vector4f.StoreAligned ((Vector4f*)timeStream, ts_reg);
                                            
                                            // Store
                                            Vector4f.StoreAligned ((Vector4f*)xPosStream, pX_reg + tx);
                                            Vector4f.StoreAligned ((Vector4f*)yPosStream, pY_reg + ty);
                                            Vector4f.StoreAligned ((Vector4f*)zPosStream, pZ_reg + tz);

                                            // Adjust the min/max values of the bounds
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        public int AddParticles (ref ISystemData particlesToAdd)
        {
            // Copy particles in to our structure
            int numAdded = _systemData.CopyFrom (_systemData.NumParticles, ref particlesToAdd, 0, particlesToAdd.NumParticles);
            
            // Return number added
            return numAdded;
        }


        public void RemoveAllParticles ()
        {
            // Clear out the system
            _systemData._numParticles = 0;
            
            // Zero-out the bounds
        }

        public object Bounds {
            get { return _worldBounds; }
        }

        public bool Is2D {
            get { return _is2D; }
            set { _is2D = value; }
        }

        public ISystemData SystemData {
            get { return _systemData; }
            set {
                #if DEBUG
                if (value.GetType () != typeof(SoAData))
                    throw new System.ArgumentException ("Supplied ISystemData was not of type SoAData.");
                #endif
                _systemData = (SoAData)value;
            }
        }
        #endregion

        public UnsafeSimdSimulation (int maxNumParticles)
        {
            _systemData = new SoAData (maxNumParticles);
            _worldBounds = new object ();
            _is2D = false;
        }

        #region Data
        SoAData _systemData;
        object _worldBounds;
        bool _is2D;
        #endregion
    }
}
#endif
