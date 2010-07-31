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
using Mono.Simd;

namespace GameClay.Dust.Mono.Simulation
{
    /// <summary>
    /// The standard simulation
    /// </summary>
    public class SimdSimulation : ISimulation
    {
        #region ISimulation implementation
        public void AdvanceTime (float dt)
        {
            // Reset the bounds of the system
            
            Vector4f dt_v = new Vector4f (dt);
            
            // Temporary registers
            Vector4f ts_reg, vX_reg, vY_reg, vZ_reg, pX_reg, pY_reg, pZ_reg;
            
            // Process the particle system
            int numBatches = _systemData.NumParticles / 4;
            for (int i = 0; i < numBatches; i++) {
                // Load streams
                int streamIdx = i * 4;
                
                ts_reg = ArrayExtensions.GetVectorAligned (_systemData._timeRemainingStream, streamIdx);
                
                vX_reg = ArrayExtensions.GetVectorAligned (_systemData._velocityStreamX, streamIdx);
                vY_reg = ArrayExtensions.GetVectorAligned (_systemData._velocityStreamY, streamIdx);
                vZ_reg = ArrayExtensions.GetVectorAligned (_systemData._velocityStreamZ, streamIdx);

                pX_reg = ArrayExtensions.GetVectorAligned (_systemData._positionStreamX, streamIdx);
                pY_reg = ArrayExtensions.GetVectorAligned (_systemData._positionStreamY, streamIdx);
                pZ_reg = ArrayExtensions.GetVectorAligned (_systemData._positionStreamZ, streamIdx);

                // Decrement time remaining
                ts_reg = VectorOperations.Max(ts_reg - dt_v, Vector4f.Zero);
                
                // Update position
                Vector4f tx = vX_reg * dt_v;
                Vector4f ty = vY_reg * dt_v;
                Vector4f tz = vZ_reg * dt_v;
                
                ArrayExtensions.SetVectorAligned (_systemData._timeRemainingStream, ts_reg, streamIdx);
                
                ArrayExtensions.SetVectorAligned (_systemData._positionStreamX, pX_reg + tx, streamIdx);
                ArrayExtensions.SetVectorAligned (_systemData._positionStreamY, pY_reg + ty, streamIdx);
                ArrayExtensions.SetVectorAligned (_systemData._positionStreamZ, pZ_reg + tz, streamIdx);
                
                // Adjust the min/max values of the bounds
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

        public SimdSimulation (int maxNumParticles)
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
