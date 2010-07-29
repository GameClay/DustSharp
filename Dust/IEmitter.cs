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
    /// <summary>
    /// An Emitter adds particles to a Simulation.
    /// </summary>
    ///
    /// <remarks>
    /// Emitters add particles to a simulation by using Simulation::addParticles(). An
    /// Emitter provides the initial velocity, position, lifespan, mass and time remaining 
    /// values to a particle, and than does not alter that particle any further.
    /// </remarks>
    public interface IEmitter
    {
        /// <summary>
        /// The Simulation into which this Emitter is submitting particles.
        /// </summary>
        ISimulation Simulation { get; set; }

        /// <summary>
        /// An object-to-world transform for this Emitter.
        /// </summary>
        object Transform { get; set; } // TODO: Matrix

        /// <summary>
        /// The random number seed this Emitter is using.
        /// </summary>
        int Seed { get; set; }

        /// <summary>
        /// The activated state of the Emitter.
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// Advance time in the Emitter. 
        /// </summary>
        ///
        /// <remarks>
        /// This is the core method for an Emitter implementation. This methods gets called every frame
        /// and gives the Emitter an opportunity to add particles to the Simulation that it has assigned.
        /// 
        /// The method arguments provide the Emitter implementation with the information needed to 
        /// animate the parameters of emission over repeating intervals, as well as track forward
        /// progression of time. 
        /// </remarks>
        ///
        /// <param name="dt"> The time, in seconds, since the last call to AdvanceTime(). </param>
        /// <param name="pt"> A normalized value which is to used by the Emitter as an interpolation parameter. </param>
        ///
        /// <returns> The number of particles this Emitter tried to add to the Simulation during the method call. </returns>
        int AdvanceTime(float dt, float pt);
    }
}