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

namespace GameClay.Dust.Simulation
{
    /// <summary>
    /// The standard simulation
    /// </summary>
    public class StandardSimulation : ISimulation
    {
        #region ISimulation implementation
        public void AdvanceTime(float dt)
        {
            // Reset the bounds of the system

            // Process the particle system
            int lastIdx = _systemData.NumParticles - 1;
            for (int i = 0; i <= lastIdx; i++)
            {
                // Decrement the time remaining
                _systemData._timeRemainingStream[i] = _systemData._timeRemainingStream[i] - dt;

                // If the particle is out of time, destroy it
                if (_systemData._timeRemainingStream[i] < 0.0)
                {
                    // Replace this element with the last element in the list
                    if (i < lastIdx)
                    {
                        _systemData.CopyElement(lastIdx, i);

                        // Decrement i so that this particle will still get processed
                        i--;
                    }

                    // Decrement the number of particles to process
                    lastIdx--;

                    // Process the next item
                    continue;
                }

                // Update position due to velocity
                _systemData._positionStreamX[i] += _systemData._velocityStreamX[i] * dt;
                _systemData._positionStreamY[i] += _systemData._velocityStreamY[i] * dt;
                _systemData._positionStreamZ[i] += _systemData._velocityStreamZ[i] * dt;

                // Update velocity due to air resistance/gravity

                // Adjust the min/max values of the bounds

            }

            // Update the number of particles in the system
            _systemData._numParticles = lastIdx + 1;
        }


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

        public StandardSimulation(int maxNumParticles)
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
