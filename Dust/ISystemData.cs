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
    /// Flags used to indicate the memory layout of the underlying data.
    /// </summary>
    ///
    /// <remarks>
    /// These are intended for use as optimization hints, and not
    /// as functionality requirements.
    /// </remarks>
    public enum SystemDataFlags : int
    {
        ManagedData     = 0,  // Mutually exclusive with UnmanageData,
        UnmanagedData   = 1,
        Vector3Position = 2,  // Vectorized position, with bit 4 clear
        Vector3Velocity = 4,  // Vectorized velocity, with bit 5 clear
        Vector4Position = 10, // 8 + 2, indicating both vectorized position and 4-component vector type
        Vector4Velocity = 20, // 16 + 4, indicating both vectorized velocity and 4-component vector type
        FullAoSData     = 32, // This flag really only exists to yell at the user
    }

    /// <summary>
    /// Structure containing the streams of data which describe the state of the
    /// particle system.
    /// </summary>
    ///
    /// <remarks>
    /// This interface wraps a set of data which is used to describe the state of a Dust system. 
    /// The structure is set up as individual streams of data so that the underlying
    /// simulation may elect to store data in any configuration they desire. While this is
    /// intended to wrap a Structure-of-Arrays (SoA), alternative underlying data organization
    /// can be used if desired. 
    /// </remarks>
    public interface ISystemData
    {
        /// <summary>
        /// Number of active particles in the system.
        /// </summary>
        int NumParticles { get; }

        /// <summary>
        /// Maximum number of particles that can stored in the system.
        /// </summary>
        int MaxNumParticles { get; }

        /// <summary>
        /// The X co-ordinate of the particles position, in world space.
        /// </summary>
        float[] PositionX { get; }

        /// <summary>
        /// The Y co-ordinate of the particles position, in world space.
        /// </summary>
        float[] PositionY { get; }

        /// <summary>
        /// The Z co-ordinate of the particles position, in world space.
        /// </summary>
        float[] PositionZ { get; }

        /// <summary>
        /// The X co-ordinate of the particles velocity.
        /// </summary>
        float[] VelocityX { get; }

        /// <summary>
        /// The Y co-ordinate of the particles velocity.
        /// </summary>
        float[] VelocityY { get; }

        /// <summary>
        /// The Z co-ordinate of the particles velocity.
        /// </summary>
        float[] VelocityZ { get; }

        /// <summary>
        /// The mass of the particle.
        /// </summary>
        /// 
        /// <remarks>
        /// A Simulation is not required to take the MassElement into account when updating the physics
        /// of a particle system.
        /// </remarks>
        float[] Mass { get; }

        /// <summary>
        /// The total lifespan, in seconds, of the particle.
        /// </summary>
        /// 
        /// <remarks>
        /// This value is ignored by simulations, which only decrement the values located in <see cref="TimeRemaining"/>,
        /// but is provided for other uses.
        /// </remarks>
        float[] Lifespan { get; }

        /// <summary>
        /// The remaining lifespan, in seconds, of the particle.
        /// </summary>
        float[] TimeRemaining { get; }

        /// <summary>
        /// An array of arbitrary data for use in user code.
        /// </summary>
        /// 
        /// <remarks>
        /// No elements of Dust depend on, or modify the elements contained in <see cref="UserData"/>.
        /// </remarks>
        object[] UserData { get; }

        /// <summary>
        ///
        /// </summary>
        SystemDataFlags Flags { get; }

        /// <summary>
        /// Copies data from a source ISystemData in to this instance.
        /// </summary>
        /// 
        /// <remarks>
        /// This method will increment the value of NumParticles by the quantity returned.
        /// </remarks>
        ///
        /// <param name="offset">    The target start index in this structure for copying. </param>
        /// <param name="src">       [in] The source <see cref="ISystemData"/>. </param>
        /// <param name="srcOffset"> The starting index in the source data streams to begin copying. </param>
        /// <param name="count">     Number of elements to copy. </param>
        /// 
        /// <returns> The number of elements which were added to this system data. </returns>
        int CopyFrom(int offset, ref ISystemData src, int srcOffset, int count);

        /// <summary>
        /// Copies the array data located at srcIndex in to the arrays at dstIndex.
        /// </summary>
        /// 
        /// <param name="srcIndex"> Source element index. </param>
        /// <param name="dstIndex"> Destination element index. </param>
        void CopyElement(int srcIndex, int dstIndex);
    }
}
