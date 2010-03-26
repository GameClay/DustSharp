
using System;

namespace GameClay
{
	namespace Dust
	{
		public class ConstantSimulation : ISimulation
		{
			#region ISimulation implementation
			public void advanceTime (float dt)
			{
				throw new System.NotImplementedException();
			}
			
			
			public int addParticles (ref ISystemData particlesToAdd)
			{
				throw new System.NotImplementedException();
			}
			
			
			public void removeAllParticles ()
			{
				throw new System.NotImplementedException();
			}
			
			
			public void getBounds (ref object bounds)
			{
				throw new System.NotImplementedException();
			}
			
			
			public int[] getAddedIds ()
			{
				throw new System.NotImplementedException();
			}
			
			
			public int[] getDeletedIds ()
			{
				throw new System.NotImplementedException();
			}
			
			
			public ISystemData SystemData {
				get {
					throw new System.NotImplementedException();
				}
				set {
					throw new System.NotImplementedException();
				}
			}
			#endregion
	
			public ConstantSimulation ()
			{
				
			}
			
		}
	}
}