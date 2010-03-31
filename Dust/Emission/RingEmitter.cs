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

   public class RingEmitterConfiguration : EmitterConfiguration
   {
      
      public RingEmitterConfiguration()
         : base()
      {
      }
      
      #region Data
      
      #endregion

   }

   public class RingEmitter : BaseEmitter
   {  
      public RingEmitterConfiguration RingConfiguration
      {
         get
         {
            return _ringConfiguration;
         }
      }
      
      protected override void _EmitParticles(int numParticlesToEmit, out ISystemData particlesToEmit)
      {
         // Temp data
         float[] posX = new float[4];
         float[] posY = new float[4];
         float[] posZ = new float[4];
         
         float[] velX = new float[4];
         float[] velY = new float[4];
         float[] velZ = new float[4];
         
         float[] len = new float[4];
         
         // Batch in chunks of 4 for nice maths
         int numBatches = numParticlesToEmit / 4;
         int numRemainder = numParticlesToEmit % 4;
         
         float[] initialMass = new float[4];
         float[] initialLifespan = new float[4];
         float[] initialSpeed = new float[4];

         float[] preSimLifespan = new float[4];
         float[] partialFrameTime = new float[4];
         
         // Initial mass doesn't change
         initialMass[0] = Configuration.InitialMass;
         initialMass[1] = Configuration.InitialMass;
         initialMass[2] = Configuration.InitialMass;
         initialMass[3] = Configuration.InitialMass;
         
         // Initial lifespan varies, but it will be corrected without modifying this value
         initialLifespan[0] = Configuration.InitialLifespan;
         initialLifespan[1] = Configuration.InitialLifespan;
         initialLifespan[2] = Configuration.InitialLifespan;
         initialLifespan[3] = Configuration.InitialLifespan;

         // Initial speed doesn't change
         initialSpeed[0] = Configuration.InitialSpeed;
         initialSpeed[1] = Configuration.InitialSpeed;
         initialSpeed[2] = Configuration.InitialSpeed;
         initialSpeed[3] = Configuration.InitialSpeed;
         
         // Radius doesn't change
         float[] radius = new float[4];
         radius[0] = 1.0f;
         radius[1] = 1.0f;
         radius[2] = 1.0f;
         radius[3] = 1.0f;
         
#if DUST_BATCH_EMISSION_ONLY
         // Subtract the remainder from the # of particles to emit
         numParticlesToEmit -= numRemainder;
#endif
         
         // Some constants to make life easier and faster
#if !DUST_SIMD         
         
         float oneOverPPS = Configuration.Persistent ? 1.0f / Configuration.ParticlesPerSecond : 0;
#else
         throw new NotImplementedException();
#endif
         
         // Emit particles in chunks of 4
         for (int i = 0; i < numBatches; i++)
         {
            // Calculate partial frame time to presimulate. 
#if !DUST_SIMD
            partialFrameTime[0] = (i + 1) * oneOverPPS;
            partialFrameTime[1] = (i + 2) * oneOverPPS;
            partialFrameTime[2] = (i + 3) * oneOverPPS;
            partialFrameTime[3] = (i + 4) * oneOverPPS;
#else
#endif
            
            // Get the random angles into Z           
            posZ[0] = (float)(RandomSource.NextDouble() * Math.PI * 2);
            posZ[1] = (float)(RandomSource.NextDouble() * Math.PI * 2);
            posZ[2] = (float)(RandomSource.NextDouble() * Math.PI * 2);
            posZ[3] = (float)(RandomSource.NextDouble() * Math.PI * 2);
            
            // Get X and Y
            posX[0] = (float)Math.Cos(posZ[0]);
            posX[1] = (float)Math.Cos(posZ[1]);
            posX[2] = (float)Math.Cos(posZ[2]);
            posX[3] = (float)Math.Cos(posZ[3]);
            
            posY[0] = (float)Math.Sin(posZ[0]);
            posY[1] = (float)Math.Sin(posZ[1]);
            posY[2] = (float)Math.Sin(posZ[2]);
            posY[3] = (float)Math.Sin(posZ[3]);
            
            // Zero-out Z
            posZ[0] = 0.0f;
            posZ[1] = 0.0f;
            posZ[2] = 0.0f;
            posZ[3] = 0.0f;
            
            // Multiply by radius
#if !DUST_SIMD
            posX[0] *= radius[0];
            posX[1] *= radius[1];
            posX[2] *= radius[2];
            posX[3] *= radius[3];
            
            posY[0] *= radius[0];
            posY[1] *= radius[1];
            posY[2] *= radius[2];
            posY[3] *= radius[3];
#else
#endif
            
            // If this emitter is supposed to emit only on the surface
            // don't need to get a random distance
            if (!Configuration.EmitOnSurfaceOnly)
            {
#if !DUST_SIMD
               float[] distance = new float[4];
               distance[0] = (float)RandomSource.NextDouble();
               distance[1] = (float)RandomSource.NextDouble();
               distance[2] = (float)RandomSource.NextDouble();
               distance[3] = (float)RandomSource.NextDouble();
               
               posX[0] *= distance[0];
               posX[1] *= distance[1];
               posX[2] *= distance[2];
               posX[3] *= distance[3];
               
               posY[0] *= distance[0];
               posY[1] *= distance[1];
               posY[2] *= distance[2];
               posY[3] *= distance[3];
#else
#endif
            }
            
            // Transform position
            // TODO: Matrix and transform stuff
            
            // Calculate velocity
#if !DUST_SIMD
            // Get length
            len[0] = (float)Math.Sqrt((posX[0] * posX[0]) + (posY[0] * posY[0]) + (posZ[0] * posZ[0]));
            len[1] = (float)Math.Sqrt((posX[1] * posX[1]) + (posY[1] * posY[1]) + (posZ[1] * posZ[1]));
            len[2] = (float)Math.Sqrt((posX[2] * posX[2]) + (posY[2] * posY[2]) + (posZ[2] * posZ[2]));
            len[3] = (float)Math.Sqrt((posX[3] * posX[3]) + (posY[3] * posY[3]) + (posZ[3] * posZ[3]));

            // Normalize
            velX[0] = posX[0] / len[0];
            velY[0] = posY[0] / len[0];
            velZ[0] = posZ[0] / len[0];

            velX[1] = posX[1] / len[1];
            velY[1] = posY[1] / len[1];
            velZ[1] = posZ[1] / len[1];

            velX[2] = posX[2] / len[2];
            velY[2] = posY[2] / len[2];
            velZ[2] = posZ[2] / len[2];

            velX[3] = posX[3] / len[3];
            velY[3] = posY[3] / len[3];
            velZ[3] = posZ[3] / len[3];

            // Scale by speed
            velX[0] *= initialSpeed[0];
            velY[0] *= initialSpeed[0];
            velZ[0] *= initialSpeed[0];

            velX[1] *= initialSpeed[1];
            velY[1] *= initialSpeed[1];
            velZ[1] *= initialSpeed[1];

            velX[2] *= initialSpeed[2];
            velY[2] *= initialSpeed[2];
            velZ[2] *= initialSpeed[2];

            velX[3] *= initialSpeed[3];
            velY[3] *= initialSpeed[3];
            velZ[3] *= initialSpeed[3];
#else
#endif

            // Avoid clumping by doing some pre-simulation
            if (oneOverPPS > 0)
            {
               posX[0] += velX[0] * partialFrameTime[0];
               posY[0] += velY[0] * partialFrameTime[0];
               posZ[0] += velZ[0] * partialFrameTime[0];

               posX[1] += velX[1] * partialFrameTime[1];
               posY[1] += velY[1] * partialFrameTime[1];
               posZ[1] += velZ[1] * partialFrameTime[1];

               posX[2] += velX[2] * partialFrameTime[2];
               posY[2] += velY[2] * partialFrameTime[2];
               posZ[2] += velZ[2] * partialFrameTime[2];

               posX[3] += velX[3] * partialFrameTime[3];
               posY[3] += velY[3] * partialFrameTime[3];
               posZ[3] += velZ[3] * partialFrameTime[3];
            }

            // Adjust lifespan after presim
#if !DUST_SIMD
            preSimLifespan[0] = initialLifespan[0] - partialFrameTime[0];
            preSimLifespan[1] = initialLifespan[1] - partialFrameTime[1];
            preSimLifespan[2] = initialLifespan[2] - partialFrameTime[2];
            preSimLifespan[3] = initialLifespan[3] - partialFrameTime[3];
#else
#endif

            // Write output result to _particlesToEmit
            int iTimesFour = i * 4;

            // Assign position
#if !DUST_SIMD
            Array.Copy(posX, 0, _particlesToEmit._positionStreamX, iTimesFour, 4);
            Array.Copy(posY, 0, _particlesToEmit._positionStreamY, iTimesFour, 4);
            Array.Copy(posZ, 0, _particlesToEmit._positionStreamZ, iTimesFour, 4);
#else
#endif

            // Assign velocity
#if !DUST_SIMD
            Array.Copy(velX, 0, _particlesToEmit._velocityStreamX, iTimesFour, 4);
            Array.Copy(velY, 0, _particlesToEmit._velocityStreamY, iTimesFour, 4);
            Array.Copy(velZ, 0, _particlesToEmit._velocityStreamZ, iTimesFour, 4);
#else
#endif

            // Assign lifespan and mass
            Array.Copy(preSimLifespan, 0, _particlesToEmit.Lifespan, iTimesFour, 4);
            Array.Copy(preSimLifespan, 0, _particlesToEmit.TimeRemaining, iTimesFour, 4);
            Array.Copy(initialMass, 0, _particlesToEmit.Mass, iTimesFour, 4);
         }
         
         
#if !DUST_BATCH_EMISSION_ONLY
         // Emit remaining particles individually
         int numBatchesTimesFour = numBatches * 4;
         for (int i = 0; i < numRemainder; i++)
         {
            //
            float angle = (float)(RandomSource.NextDouble() * Math.PI * 2);
            posX[0] = (float)Math.Cos(angle) * radius[0];
            posY[0] = (float)Math.Sin(angle) * radius[0];
            posZ[0] = 0.0f;
            
            // If this emitter is supposed to emit only on the surface
            // don't need to get a random distance
            if (!Configuration.EmitOnSurfaceOnly)
            {
#if !DUST_SIMD
               float distance = (float)RandomSource.NextDouble();
               posX[0] *= distance;
               posY[0] *= distance;
#else
#endif
            }
            
            // Transform position
            // TODO: Matrix and transform stuff

            // Length
            len[0] = (float)Math.Sqrt((posX[0] * posX[0]) + (posY[0] * posY[0]) + (posZ[0] * posZ[0]));

            // Normalize
            velX[0] = posX[0] / len[0];
            velY[0] = posY[0] / len[0];
            velZ[0] = posZ[0] / len[0];

            // Scale by speed
            velX[0] *= initialSpeed[i];
            velY[0] *= initialSpeed[i];
            velZ[0] *= initialSpeed[i];

            // Avoid clumping by doing some pre-simulation
            float preSimTime = (i + 1) * oneOverPPS;
            if (oneOverPPS > 0)
            {
               posX[0] += velX[0] * preSimTime;
               posY[0] += velY[0] * preSimTime;
               posZ[0] += velZ[0] * preSimTime;
            }

            // Store out position
            _particlesToEmit._positionStreamX[numBatchesTimesFour + i] = posX[0];
            _particlesToEmit._positionStreamY[numBatchesTimesFour + i] = posY[0];
            _particlesToEmit._positionStreamZ[numBatchesTimesFour + i] = posZ[0];

            // Store out velocity
            _particlesToEmit._velocityStreamX[numBatchesTimesFour + i] = velX[0];
            _particlesToEmit._velocityStreamY[numBatchesTimesFour + i] = velY[0];
            _particlesToEmit._velocityStreamZ[numBatchesTimesFour + i] = velZ[0];

            // Store out lifespan and mass
            float lifespan = initialLifespan[i] - preSimTime;
            _particlesToEmit._lifespanStream[numBatchesTimesFour + i] = lifespan;
            _particlesToEmit._timeRemainingStream[numBatchesTimesFour + i] = lifespan;
            _particlesToEmit._massStream[numBatchesTimesFour + i] = initialMass[i];
         }
#endif
         
         // Assign number of particles
         _particlesToEmit._numParticles = numParticlesToEmit;
         
         // Assign the output variable
         particlesToEmit = _particlesToEmit;
      }
  
      public RingEmitter()
         : base()
      {
         _particlesToEmit = new SoAData(200);
         
         // Replace the base configuration with our specalized one
         _ringConfiguration = new RingEmitterConfiguration();
         _configuration = (EmitterConfiguration)_ringConfiguration;
      }
      
      #region Data
      protected SoAData _particlesToEmit;
      protected RingEmitterConfiguration _ringConfiguration;
      #endregion
   }
}
