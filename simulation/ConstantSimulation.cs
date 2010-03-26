
using System;
using System.Collections.Generic;

namespace GameClay
{
	namespace Dust
	{
		/// <summary>
		/// This is a simple, single-buffered simulation which simply advances and
		/// times-out particles.
		/// </summary>
		public class ConstantSimulation : ISimulation
		{
			#region ISimulation implementation
			public void AdvanceTime (float dt)
			{
				// Reset the bounds of the system
				
				// Process the particle system
				for (int i = 0; i < _systemData.NumParticles; i++)
				{
					// Don't mess with infinite-lifespan particles
					if (_systemData._lifespanStream[i] < float.PositiveInfinity)
					{
						// Decrement the lifespan
						_systemData._lifespanStream[i] -= dt;
						
						// If the particle is out of time, destroy it
						if (_systemData._lifespanStream[i] < 0.0)
						{							
							// Replace this element with the last element in the list
							int lastIdx = _systemData.NumParticles - 1;
							if (i < lastIdx)
							{
								_systemData.CopyElement(lastIdx, i);
								
								// Decrement i so that this particle will still get processed
								i--;
							}
							
							// Decrement the number of particles
							_systemData._numParticles--;
							
							// Process the next item
							continue;
						}
					}
					
					// Adjust the min/max values of the bounds
					// (this won't happen if the particle has been deleted)
					
				}
			}
			
			
			public int AddParticles (ref ISystemData particlesToAdd)
			{				
				// Copy particles in to our structure
				int numAdded = _systemData.CopyFrom(_systemData.NumParticles, 
				                     ref particlesToAdd, 0, particlesToAdd.NumParticles);
				
				// Return number added
				return numAdded;
			}
			
			
			public void RemoveAllParticles ()
			{				
				// Clear out the system
				_systemData._numParticles = 0;
				
				// Zero-out the bounds
			}
			
			public object Bounds {
				get {
					return _worldBounds;
				}
			}
			
			
			public ISystemData SystemData {
				get {
					return _systemData;
				}
			}
			#endregion
	
			public ConstantSimulation (int maxNumParticles)
			{
				_systemData = new SoAData(maxNumParticles);
				_worldBounds = new object();
			}
			
			#region Data
			SoAData _systemData;
			object _worldBounds;
			#endregion
		}
	}
}