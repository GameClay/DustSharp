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
      ISystemData SystemData { get; }

      /// <summary>
      /// Advance the simulation by a number of seconds.
      /// </summary>
      ///
      /// <remarks>
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