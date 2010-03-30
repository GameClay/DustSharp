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
      /// The remaining lifespan, in seconds, of the particle.
      /// </summary>
      float[] TimeRemaining { get; }

      // TODO: Add an object[] for users to store arbitrary data

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
