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
			void advanceTime(float dt);
			
			/// <summary>
			/// Adds particles to the Simulation.
			/// </summary>
			///
			/// <remarks>
			/// Input values are copied from the specified SystemData into the internals of the Simulation.
			/// Particle id's are never read, they are always assigned by the simulation. To query
			/// the id's of particles which got added as a result of this call, use getAddedIds(). This information
			/// is only accessible until the next call to addParticles().
			/// 
			/// @see getAddedIds()
			/// @see SystemData
			/// </remarks>
			///
			/// <param name="particlesToAdd"> [in] The initial data values for the added particles. </param>
			///
			/// <returns> The number of particles added to the Simulation. </returns>
			int addParticles(ref ISystemData particlesToAdd);
			
			/// <summary>
			/// Removes all the particles from the Simulation.
			/// </summary>
			void removeAllParticles();
			
			/// <summary>
			/// Query the world-space bounds of the Simulation.
			/// </summary>
			///
			/// <param name="bounds"> [out] A world-space box which describes the area that the particles in the Simulation occupy. </param>
			void getBounds(ref Object bounds);
			
			/// <summary>
			/// Gets the list of particle ids added by the last call to addParticles().
			/// </summary>
			///
			/// <remarks>
			/// Returns an array of ids which were added to the particle simulation in the
			/// last call to addParticles(). This list is ordered in the same order which
			/// the corresponding particles were added.
			/// </remarks>
			///
			/// <returns> An array of added particle ids. </returns>
			int[] getAddedIds();
			
			/// <summary>
			/// Gets the list of particle ids deleted during the last call to advanceTime().
			/// </summary>
			///
			/// <remarks>
			/// Returns a pointer to the start of an array of ids which have been deleted
			/// from the Simulation during the last call to advanceTime().
			/// </remarks>
			///
			/// <returns> An array of deleted particle ids. </returns>
			int[] getDeletedIds();
		}
	}
}