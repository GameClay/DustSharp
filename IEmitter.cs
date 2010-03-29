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
using System;

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
        /// <param name="dt"> The time, in seconds, since the last call to advanceTime(). </param>
        /// <param name="pt"> A normalized value which is to used by the Emitter as an interpolation parameter. </param>
        ///
        /// <returns> The number of particles this Emitter tried to add to the Simulation during the method call. </returns>
        int advanceTime(float dt, float pt);
    }
}