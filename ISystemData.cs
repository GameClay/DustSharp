
using System;

namespace GameClay
{
	namespace Dust
	{
		/// <summary>
		/// Structure containing the streams of data which describe the state of the
		/// particle system.
		/// </summary>
		///
		/// <remarks>
		/// This structure wraps a set of data which is used to describe the state of a Dust system. 
		/// The structure is set up as individual streams of data so that the host engine and/or underlying
		/// simulation may elect to store data in any configuration they desire.
		/// </remarks>
		public interface ISystemData
		{
			/// <summary>
			/// Number of active particles in the system.
			/// </summary>
			int NumParticles { get; }
			
			/// <summary>
			/// Maximum number of particles that can stored in these buffers.
			/// </summary>
			int MaxNumParticles { get; }
			
			/// <summary>
			/// A 3-component float vector element storing the position of the particle.
			/// </summary>
			object[] Position { get; } // TODO: Point3F
			
			/// <summary>
			/// A single component element which indicates the lifespan of the particle.
			/// </summary>
			float[] Lifespan { get; }
			
			/// <summary> A single component element which indicates the amount of time the particle has left to live.  </summary>
			float[] TimeRemaining { get; }
			
			/// <summary> A 3-component float vector element indicating the velocity of the particle.  </summary>
			object[] Velocity { get; } // TODO: Point3F
			
			/// <summary>
			/// A single component element which indicates the mass of the particle.
			/// </summary>
			/// 
			/// <remarks>
			/// A Simulation is not required to take the MassElement into account when updating the physics
			/// of a particle system.
			/// </remarks>
			float Mass { get; }
			
			/// <summary>
			/// A single component element which contains a unique id for each particle.
			/// </summary>
			/// 
			/// <remarks>
			/// This value is always assigned by a Simulation, and is never read by a Simulation.
			/// </remarks>
			int[] ParticleId { get; }
		}
	}
}
