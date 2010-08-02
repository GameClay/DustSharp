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

namespace GameClay.Dust.Unsafe
{
    /// <summary>
    /// A direct interface to the pointers of memory which describe the state of the
    /// particle system.
    /// </summary>
    ///
    /// <remarks>
    /// Values for <see cref="PositionStride"/> and <see cref="VelocityStride"/> allow
    /// position and velocity to be stored in vector types. Currently a full-AoS interface,
    /// with stride values for each element of particle data, is not supported.
    /// </remarks>
    public unsafe interface ISystemDataStreams
    {
        /// <summary>
        /// The X co-ordinate of the particles position, in world space.
        /// </summary>
        float* PositionXStream { get; }

        /// <summary>
        /// The Y co-ordinate of the particles position, in world space.
        /// </summary>
        float* PositionYStream { get; }

        /// <summary>
        /// The Z co-ordinate of the particles position, in world space.
        /// </summary>
        float* PositionZStream { get; }
        
        /// <summary>
        /// The length, in bytes, between each subsequent element of the position.
        /// </summary>
        ///
        /// <remarks>
        /// A value of 0 indicates that the position is laid out in SoA format,
        /// whereas a value > 0 indicates that position is laid out in AoS format.
        /// </remarks>
        int PositionStreamStride { get; }

        /// <summary>
        /// The X co-ordinate of the particles velocity.
        /// </summary>
        float* VelocityXStream { get; }

        /// <summary>
        /// The Y co-ordinate of the particles velocity.
        /// </summary>
        float* VelocityYStream { get; }

        /// <summary>
        /// The Z co-ordinate of the particles velocity.
        /// </summary>
        float* VelocityZStream { get; }
        
        /// <summary>
        /// The length, in bytes, between each subsequent element of the velocity.
        /// </summary>
        ///
        /// <remarks>
        /// A value of 0 indicates that the velocity is laid out in SoA format,
        /// whereas a value > 0 indicates that velocity is laid out in AoS format.
        /// </remarks>
        int VelocityStreamStride { get; }

        /// <summary>
        /// The mass of the particle.
        /// </summary>
        /// 
        /// <remarks>
        /// A Simulation is not required to take the MassElement into account when updating the physics
        /// of a particle system.
        /// </remarks>
        float* MassStream { get; }

        /// <summary>
        /// The total lifespan, in seconds, of the particle.
        /// </summary>
        /// 
        /// <remarks>
        /// This value is ignored by simulations, which only decrement the values located in <see cref="TimeRemaining"/>,
        /// but is provided for other uses.
        /// </remarks>
        float* LifespanStream { get; }

        /// <summary>
        /// The remaining lifespan, in seconds, of the particle.
        /// </summary>
        float* TimeRemainingStream { get; }

        /// <summary>
        /// An array of arbitrary integer data associated with a particle.
        /// </summary>
        /// 
        /// <remarks>
        /// No elements of Dust depend on, or modify the elements contained in <see cref="UserData"/>.
        /// </remarks>
        int* UserDataStream { get; }
    }
}