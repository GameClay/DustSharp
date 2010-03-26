
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
		/// This interface wraps a set of data which is used to describe the state of a Dust system. 
		/// The structure is set up as individual streams of data so that the underlying
		/// simulation may elect to store data in any configuration they desire.
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
			/// A 3-component float vector element storing the position of the particle.
			/// </summary>
			object[] Position { get; } // TODO: Point3F
			
			/// <summary>
			/// A single component element which indicates the lifespan of the particle.
			/// </summary>
			float[] Lifespan { get; }
			
			/// <summary> 
			/// A 3-component float vector element indicating the velocity of the particle.
			/// </summary>
			object[] Velocity { get; } // TODO: Point3F
			
			/// <summary>
			/// A single component element which indicates the mass of the particle.
			/// </summary>
			/// 
			/// <remarks>
			/// A Simulation is not required to take the MassElement into account when updating the physics
			/// of a particle system.
			/// </remarks>
			float[] Mass { get; }
			
			/// <summary>
			/// Copies data from a source ISystemData in to this instance.
			/// </summary>
			/// 
			/// <remarks>
			/// This method will increment the value of NumParticles by the quantity returned.
			/// 
			/// If the following arrays in src are null, the corisponding default value 
			/// will be initialized.
			/// <list>
	        /// 		<item>Lifespan - float.PositiveInfinity</item>
	        /// 		<item>Velocity - ZERO VECTOR CONST</item>
	        /// 		<item>Mass - 1.0</item>
	        /// </list>
			/// </remarks>
			///
			/// <param name="offset">    The target start index in this structure for copying. </param>
			/// <param name="src">       [in] The source <see cref="ISystemData"/>. </param>
			/// <param name="srcOffset"> The starting index in the source data streams to begin copying. </param>
			/// <param name="count">     Number of elements to copy. </param>
			/// 
			/// <returns> The number of elements which were added to this system data. </returns>
			int CopyFrom (int offset, ref ISystemData src, int srcOffset, int count);
			
			/// <summary>
			/// Copies the array data located at srcIndex in to the arrays at dstIndex.
			/// </summary>
			/// 
			/// <param name="srcIndex"> Source element index. </param>
			/// <param name="dstIndex"> Destination element index. </param>
			void CopyElement (int srcIndex, int dstIndex);
		}
	}
}
