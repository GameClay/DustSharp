
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
			
			
			public float[] TimeRemaining {
				get {
					return _timeRemainingStream;
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
			
			
			public int[] ParticleId {
				get {
					return _idStream;
				}
			}
			
			#endregion
			
			public SoAData(int maxNumParticles)
			{
				_numParticles = 0;
				_maxNumParticles = maxNumParticles;
				_positionStream = new object[MaxNumParticles];
				_lifespanStream = new float[MaxNumParticles];
				_timeRemainingStream = new float[MaxNumParticles];
				_velocityStream = new object[MaxNumParticles];
				_massStream = new float[MaxNumParticles];
				_idStream = new int[MaxNumParticles];
			}
			
			#region Data
			public int _numParticles;
			public int _maxNumParticles;
			public object[] _positionStream;
			public float[] _lifespanStream;
			public float[] _timeRemainingStream;
			public object[] _velocityStream;
			public float[] _massStream;
			public int[] _idStream;
			#endregion
		}
	}
}