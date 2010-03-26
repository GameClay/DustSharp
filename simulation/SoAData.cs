
using System;

namespace GameClay
{
	namespace Dust
	{
		/// <summary>
		/// An ISystemData implemented as a structure of arrays.
		/// </summary>
		public class SoAData : ISystemData
		{
			#region ISystemData implementation
			public int NumParticles {
				get {
					return _numParticles;
				}
			}
			
			
			public int MaxNumParticles {
				get {
					return _maxNumParticles;
				}
			}
			
			
			public object[] Position {
				get {
					return _positionStream;
				}
			}
			
			
			public float[] Lifespan {
				get {
					return _lifespanStream;
				}
			}
			
			
			public object[] Velocity {
				get {
					return _velocityStream;
				}
			}
			
			
			public float[] Mass {
				get {
					return _massStream;
				}
			}
			
			
			public int CopyFrom (int offset, ref ISystemData src, int srcOffset, int count)
			{
				int capacityLeft = MaxNumParticles - NumParticles;
				int numToCopy = count < capacityLeft ? count : capacityLeft;
				
				// Use Buffer.BlockCopy instead of Array.CopyTo because Buffer.BlockCopy 
				// is a much faster method for moving arrays of data around.
				
				// Position
				Buffer.BlockCopy(src.Position, srcOffset,
				                 _positionStream, offset,
				                 numToCopy);
				
				// Lifespan
				Buffer.BlockCopy(src.Lifespan, srcOffset,
				                 _lifespanStream, offset,
				                 numToCopy);
				
				// Velocity
				Buffer.BlockCopy(src.Velocity, srcOffset,
				                 _velocityStream, offset,
				                 numToCopy);
				
				// Mass
				Buffer.BlockCopy(src.Mass, srcOffset,
				                 _massStream, offset,
				                 numToCopy);
				
				// Update number of particles
				_numParticles += numToCopy;
				
				// Return number copied
				return numToCopy;
			}
			
			public void CopyElement (int srcIndex, int dstIndex)
			{
				// This isn't super awesome, in the SoA case, but what can ya do
				_positionStream[dstIndex] = _positionStream[srcIndex];
				_lifespanStream[dstIndex] = _lifespanStream[srcIndex];
				_velocityStream[dstIndex] = _velocityStream[srcIndex];
				_massStream[dstIndex]     = _massStream[srcIndex];
			}
			
			#endregion
			
			public SoAData (int maxNumParticles)
			{
				_numParticles = 0;
				_maxNumParticles = maxNumParticles;
				
				_positionStream = new object[MaxNumParticles];
				_lifespanStream = new float[MaxNumParticles];
				_velocityStream = new object[MaxNumParticles];
				_massStream = new float[MaxNumParticles];
			}
			
			#region Data
			public int _numParticles;
			public int _maxNumParticles;
			public object[] _positionStream;
			public float[] _lifespanStream;
			public object[] _velocityStream;
			public float[] _massStream;
			#endregion
		}
	}
}