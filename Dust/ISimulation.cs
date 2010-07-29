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
    /// A Simulation provides the physics component to a Dust particle system.
    /// </summary>
    ///
    /// <remarks>
    /// A Simulation controls the behavior of particles once they are
    /// added by an Emitter. This includes updating their position, velocity,
    /// decrementing their time remaining, and deleting them when that time 
    /// reaches zero. 
    ///
    /// The Simulation does not allocate, resize, or destroy the memory used for output. 
    /// That memory is described in a SystemData structure, and assigned using setSystemData().
    /// 
    /// @see SystemData
    /// </remarks>
    public interface ISimulation
    {
        /// <summary>
        /// The SystemData structure this Simulation uses to store output data.
        /// </summary>
        ISystemData SystemData { get; set; }

        /// <summary>
        /// Advance the simulation by a number of seconds.
        /// </summary>
        ///
        /// <remarks>
        /// This method will decrement the <see cref="ISystemData.TimeRemaining"/> element of
        /// each particle in the Simulation. A call to this method will never result in a
        /// <see cref="ISystemData.TimeRemaining"/> element which is less than zero. It will
        /// allow each particle to exist, with a remaining-time of zero for at least one call
        /// to AdvanceTime.
        /// </remarks>
        /// 
        /// <param name="dt"> The time, in seconds, to advance the simulation. </param>
        void AdvanceTime(float dt);

        /// <summary>
        /// Adds particles to the Simulation.
        /// </summary>
        ///
        /// <remarks>
        /// Input values are copied from the specified SystemData into the internals of the Simulation.
        /// 
        /// @see SystemData
        /// </remarks>
        ///
        /// <param name="particlesToAdd"> [in] The initial data values for the added particles. </param>
        ///
        /// <returns> The number of particles added to the Simulation. </returns>
        int AddParticles(ref ISystemData particlesToAdd);

        /// <summary>
        /// Removes all the particles from the Simulation.
        /// </summary>
        /// 
        /// <remarks>
        /// This method also clears the DeletedIds.
        /// </remarks>
        void RemoveAllParticles();

        /// <summary>
        /// The world-space bounds of the Simulation.
        /// </summary>
        object Bounds { get; }

        /// <summary>
        /// Queries if this Simulation is in 2D mode.
        /// </summary>
        /// 
        /// <remarks>
        /// A simulation in 2D mode is not required to process the Z co-ordinate.
        /// </remarks>
        bool Is2D { get; set; }
    }
}