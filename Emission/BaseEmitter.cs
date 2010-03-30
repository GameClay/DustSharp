/* Dust -- Copyright (C) 2009-2010 GameClay LLC
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation;
 * version 2.1 of the License.

 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.

 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */
using System;

namespace GameClay.Dust
{
   /// <summary>
   /// A base class for an <see cref="IEmitter"/> implementation.
   /// </summary>
   /// 
   /// <remarks>
   /// This helper class takes care of the <see cref="IEmitter.AdvanceTime"/> and property implementations, 
   /// only requiring that the derived class implement the <see cref="_EmitParticles"/> method.
   /// </remarks>
   public abstract class BaseEmitter : IEmitter
   {

      #region IEmitter implementation
      public int AdvanceTime(float dt, float pt)
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

      public ISimulation Simulation
      {
         get
         {
            return _simulation;
         }
         set
         {
            _simulation = value;
         }
      }


      public object Transform
      {
         get
         {
            return _transform;
         }
         set
         {
            _transform = value;
         }
      }


      public int Seed
      {
         get
         {
            return _seed;
         }
         set
         {
            _seed = value;
         }
      }


      public bool Active
      {
         get
         {
            return _active;
         }
         set
         {
            _active = value;
         }
      }

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


      public BaseEmitter()
      {
         _timeSinceEmission = 0;
         _configuration = null;
         _randomSource = new Random(Seed);
      }

      #region Data
      protected float _timeSinceEmission;
      protected EmitterConfiguration _configuration;
      protected Random _randomSource;
      protected bool _active;
      protected int _seed;
      protected ISimulation _simulation;
      protected object _transform;
      #endregion
   }
}