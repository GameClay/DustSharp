using System;

namespace GameClay
{
   namespace Dust
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
      }
   }
}