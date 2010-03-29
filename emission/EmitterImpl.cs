
using System;

namespace GameClay.Dust
{

   /// <summary>
   /// A base class for an <see cref="IEmitter"/> implementation.
   /// </summary>
   /// 
   /// <remarks>
   /// This class takes care of the <see cref="IEmitter.advanceTime"/> implementation and
   /// instead only requires that the derived class implement the <see cref="_EmitParticles"/>
   /// method.
   /// 
   /// All other properties of <see cref="IEmitter"/> must be implemented on the derived class
   /// for functionality.
   /// </remarks>
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
               // Call _EmitParticles and add any resultant particles 
               ISystemData particlesToEmit = null;
               _EmitParticles(numParticlesToEmit, out particlesToEmit);
               
               if (particlesToEmit != null)
               {
                  // Clear the time since the last emission, leaving any time for partially emitted particles intact
                  _timeSinceEmission -= (1.0f / Configuration.ParticlesPerSecond) * particlesToEmit.NumParticles;
                  
                  // Add particles to simulation
                  Simulation.AddParticles(ref particlesToEmit);
               }
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
      }
      
      /// <summary>
      /// The random number generator for the implementation to use.
      /// </summary>
      /// 
      /// <remarks>
      /// This <see cref="Random"/> is initialized using the value obtained from <see cref="IEmitter.Random"/>.
      /// </remarks>
      protected Random RandomSource
      {
         // TODO: Investigate performance of System.Random, and all the casts from double-to-float which is done
         get
         {
            return _randomSource;
         }
      }
      #endregion
      
      /// <summary>
      /// This method is a request to emit a number of particles. The implementor should assign, and populate 
      /// the <see cref="ISystemData"/> passed as a parameter with the particles it does emit.
      /// </summary>
      /// 
      /// <remarks>
      /// If the value of  <see cref="particlesToEmit"/> is not <see cref="null"/>, <see cref="ISimulation.AddParticles"/>
      /// will be called following the return of this method.
      /// </remarks>
      /// 
      /// <param name="numParticlesToEmit"> The number of particles requested for emission </param>
      /// <param name="particlesToEmit"> A <see cref="ISystemData"/> full of particles to add to the <see cref="ISimulation"/>. </param>
      protected abstract void _EmitParticles(int numParticlesToEmit, out ISystemData particlesToEmit);
      
      
      public EmitterImpl()
      {
         _timeSinceEmission = 0;
         _configuration = null;
         _randomSource = new Random(Seed);
      }
      
      #region Data
      protected float _timeSinceEmission;
      protected EmitterConfiguration _configuration;
      protected Random _randomSource;
      #endregion
   }
}
