
using System;

namespace GameClay.Dust
{


   public abstract class EmitterImpl : IEmitter
   {

      #region IEmitter implementation
      public int advanceTime(float dt, float pt)
      {
         int numParticlesEmitted = 0;
         
         if (Active)
         {
#if DEBUG
            // Test for null Simulation
            if (Simulation == null)
               throw new NullReferenceException("No Simulation assigned.");
#endif
            
            _timeSinceEmission += dt;
            
            // Figure out the number of particles to emit
            int numParticlesToEmit = 0;

            if (Configuration.Persistent)
               numParticlesToEmit = (int)(Configuration.ParticlesPerSecond * _timeSinceEmission);
            else
            {
               numParticlesToEmit = (int)Configuration.ParticlesPerSecond;
               Active = false;
            }
            
            // Emit particles
            if (numParticlesToEmit > 0)
            {
               numParticlesEmitted = _EmitParticles(numParticlesToEmit);
            }
         }
         
         return numParticlesEmitted;
      }
      
      #region Stub implementations
      public ISimulation Simulation
      {
         get
         {
            throw new System.NotImplementedException();
         }
         set 
         {
            throw new System.NotImplementedException();
         }
      }
      
      
      public object Transform 
      {
         get 
         {
            throw new System.NotImplementedException();
         }
         set 
         {
            throw new System.NotImplementedException();
         }
      }
      
      
      public int Seed 
      {
         get 
         {
            throw new System.NotImplementedException();
         }
         set 
         {
            throw new System.NotImplementedException();
         }
      }
      
      
      public bool Active 
      {
         get
         {
            throw new System.NotImplementedException();
         }
         set
         {
            throw new System.NotImplementedException();
         }
      }
      #endregion
      
      #endregion
      
      #region Properties
      public EmitterConfiguration Configuration
      {
         get
         {
            return _configuration;
         }
         set
         {
            _configuration = value;
         }
      }
      #endregion
      
      protected abstract int _EmitParticles(int numParticlesToEmit);
      
      public EmitterImpl()
      {
         _timeSinceEmission = 0;
      }
      
      #region Data
      protected float _timeSinceEmission;
      protected EmitterConfiguration _configuration;
      
      #endregion
   }
}
