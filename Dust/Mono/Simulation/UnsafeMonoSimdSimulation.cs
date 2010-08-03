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
    public unsafe class UnsafeSimdSimulation : ISimulation
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
            for (int i = 0; i < numBatches; i++)
            {
                // Load streams
                int streamIdx = i * 4;
                ts_reg = Vector4f.LoadAligned ((Vector4f*)(_systemData.TimeRemainingStream + streamIdx));
                
                vX_reg = Vector4f.LoadAligned ((Vector4f*)(_systemData.VelocityXStream + streamIdx));
                vY_reg = Vector4f.LoadAligned ((Vector4f*)(_systemData.VelocityYStream + streamIdx));
                vZ_reg = Vector4f.LoadAligned ((Vector4f*)(_systemData.VelocityZStream + streamIdx));
                
                pX_reg = Vector4f.LoadAligned ((Vector4f*)(_systemData.PositionXStream + streamIdx));
                pY_reg = Vector4f.LoadAligned ((Vector4f*)(_systemData.PositionYStream + streamIdx));
                pZ_reg = Vector4f.LoadAligned ((Vector4f*)(_systemData.PositionZStream + streamIdx));
                
                // Decrement time remaining
                ts_reg = VectorOperations.Max(ts_reg - dt_v, Vector4f.Zero);
                
                // Update position
                Vector4f tx = vX_reg * dt_v;
                Vector4f ty = vY_reg * dt_v;
                Vector4f tz = vZ_reg * dt_v;
                
                // Decrement time remaining
                Vector4f.StoreAligned ((Vector4f*)_systemData.TimeRemainingStream, ts_reg);
                
                // Store
                Vector4f.StoreAligned ((Vector4f*)_systemData.PositionXStream, pX_reg + tx);
                Vector4f.StoreAligned ((Vector4f*)_systemData.PositionYStream, pY_reg + ty);
                Vector4f.StoreAligned ((Vector4f*)_systemData.PositionZStream, pZ_reg + tz);
                
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
                _systemData = (GameClay.Dust.Unsafe.SoAData)value;
            }
        }
        #endregion

        public UnsafeSimdSimulation (int maxNumParticles)
        {
            _systemData = new GameClay.Dust.Unsafe.SoAData(maxNumParticles);
            _worldBounds = new object();
            _is2D = false;
        }

        #region Data
        GameClay.Dust.Unsafe.SoAData _systemData;
        object _worldBounds;
        bool _is2D;
        #endregion
    }
}
