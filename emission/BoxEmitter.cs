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

   public class BoxEmitterConfiguration : EmitterConfiguration
   {

      #region Properties
      public float Width
      {
         get
         {
            return _width;
         }
         set
         {
            _width = value;
         }
      }


      public float Height
      {
         get
         {
            return _height;
         }
         set
         {
            _height = value;
         }
      }


      public bool EmitOnSurfaceOnly
      {
         get
         {
            return _emitOnSurfaceOnly;
         }
         set
         {
            _emitOnSurfaceOnly = value;
         }
      }


      public float Depth
      {
         get
         {
            return _depth;
         }
         set
         {
            _depth = value;
         }
      }
      #endregion

      public BoxEmitterConfiguration()
         : base()
      {
         _width = 1.0f;
         _height = 1.0f;
         _depth = 1.0f;
         _emitOnSurfaceOnly = false;
      }

      #region Data
      protected float _width;
      protected float _height;
      protected float _depth;
      protected bool _emitOnSurfaceOnly;
      #endregion

   }

   public class BoxEmitter : EmitterImpl, IEmitter
   {

      public BoxEmitterConfiguration BoxConfiguration
      {
         get
         {
            return _boxConfiguration;
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

#if DUST_BATCH_EMISSION_ONLY
         // Subtract the remainder from the # of particles to emit
         numParticlesToEmit -= numRemainder;
#endif

         // Some constants to make life easier and faster
#if !DUST_SIMD
         float twoWidth = BoxConfiguration.Width * 2.0f;
         float twoHeight = BoxConfiguration.Height * 2.0f;
         float twoDepth = BoxConfiguration.Depth * 2.0f;

         float negWidth = -BoxConfiguration.Width;
         float negHeight = -BoxConfiguration.Height;
         float negDepth = -BoxConfiguration.Depth;

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

            // Get the randoms
            posX[0] = (float)RandomSource.NextDouble();
            posX[1] = (float)RandomSource.NextDouble();
            posX[2] = (float)RandomSource.NextDouble();
            posX[3] = (float)RandomSource.NextDouble();

            posY[0] = (float)RandomSource.NextDouble();
            posY[1] = (float)RandomSource.NextDouble();
            posY[2] = (float)RandomSource.NextDouble();
            posY[3] = (float)RandomSource.NextDouble();

            posZ[0] = (float)RandomSource.NextDouble();
            posZ[1] = (float)RandomSource.NextDouble();
            posZ[2] = (float)RandomSource.NextDouble();
            posZ[3] = (float)RandomSource.NextDouble();

            // Get the positions in the range -0.5 .. 0.5
#if !DUST_SIMD
            posX[0] -= 0.5f;
            posX[1] -= 0.5f;
            posX[2] -= 0.5f;
            posX[3] -= 0.5f;

            posY[0] -= 0.5f;
            posY[1] -= 0.5f;
            posY[2] -= 0.5f;
            posY[3] -= 0.5f;

            posZ[0] -= 0.5f;
            posZ[1] -= 0.5f;
            posZ[2] -= 0.5f;
            posZ[3] -= 0.5f;
#else
#endif

            // Multiply by 2.0 * Width/Height/Depth of box
#if !DUST_SIMD
            posX[0] *= twoWidth;
            posX[1] *= twoWidth;
            posX[2] *= twoWidth;
            posX[3] *= twoWidth;

            posY[0] *= twoDepth;
            posY[1] *= twoDepth;
            posY[2] *= twoDepth;
            posY[3] *= twoDepth;

            posZ[0] *= twoHeight;
            posZ[1] *= twoHeight;
            posZ[2] *= twoHeight;
            posZ[3] *= twoHeight;
#else
#endif

            // If this emitter is supposed to emit only on the surface
            // do the needed clipping
            if (BoxConfiguration.EmitOnSurfaceOnly)
            {
               switch (RandomSource.Next() % 3)
               {
                  case 0:
#if !DUST_SIMD
                     posX[0] = posX[0] < 0.0f ? negWidth : _boxConfiguration.Width;
                     posX[1] = posX[1] < 0.0f ? negWidth : _boxConfiguration.Width;
                     posX[2] = posX[2] < 0.0f ? negWidth : _boxConfiguration.Width;
                     posX[3] = posX[3] < 0.0f ? negWidth : _boxConfiguration.Width;
#else
#endif
                     break;

                  case 1:
#if !DUST_SIMD
                     posY[0] = posY[0] < 0.0f ? negDepth : _boxConfiguration.Depth;
                     posY[1] = posY[1] < 0.0f ? negDepth : _boxConfiguration.Depth;
                     posY[2] = posY[2] < 0.0f ? negDepth : _boxConfiguration.Depth;
                     posY[3] = posY[3] < 0.0f ? negDepth : _boxConfiguration.Depth;
#else
#endif
                     break;

                  case 2:
#if !DUST_SIMD
                     posZ[0] = posZ[0] < 0.0f ? negHeight : _boxConfiguration.Height;
                     posZ[1] = posZ[1] < 0.0f ? negHeight : _boxConfiguration.Height;
                     posZ[2] = posZ[2] < 0.0f ? negHeight : _boxConfiguration.Height;
                     posZ[3] = posZ[3] < 0.0f ? negHeight : _boxConfiguration.Height;
#else
#endif
                     break;
               }
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
            Array.Copy(initialMass, 0, _particlesToEmit.Mass, iTimesFour, 4);
         }


#if !DUST_BATCH_EMISSION_ONLY
         // Emit remaining particles individually
         int numBatchesTimesFour = numBatches * 4;
         for (int i = 0; i < numRemainder; i++)
         {
            // Get random position
            posX[0] = (float)(RandomSource.NextDouble() - 0.5) * twoWidth;
            posY[0] = (float)(RandomSource.NextDouble() - 0.5) * twoHeight;
            posZ[0] = (float)(RandomSource.NextDouble() - 0.5) * twoDepth;

            // If this emitter is supposed to emit only on the surface
            // do the needed clipping
            if (BoxConfiguration.EmitOnSurfaceOnly)
            {
               switch (RandomSource.Next() % 3)
               {
                  case 0:
                     posX[0] = posX[0] < 0.0f ? negWidth : _boxConfiguration.Width;
                     break;

                  case 1:
                     posY[0] = posY[0] < 0.0f ? negDepth : _boxConfiguration.Height;
                     break;

                  case 2:
                     posZ[0] = posZ[0] < 0.0f ? negHeight : _boxConfiguration.Depth;
                     break;
               }
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
            if (preSimTime > 0)
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

            // Store out initial lifespan and mass
            _particlesToEmit._lifespanStream[numBatchesTimesFour + i] = initialLifespan[i] - preSimTime;
            _particlesToEmit._massStream[numBatchesTimesFour + i] = initialMass[i];
         }
#endif

         // Assign number of particles
         _particlesToEmit._numParticles = numParticlesToEmit;

         // Assign the output variable
         particlesToEmit = _particlesToEmit;
      }

      public BoxEmitter()
         : base()
      {
         _particlesToEmit = new SoAData(200);

         // Replace the base configuration with our specialized one
         _boxConfiguration = new BoxEmitterConfiguration();
         _configuration = (EmitterConfiguration)_boxConfiguration;
      }

      #region Data
      protected SoAData _particlesToEmit;
      protected BoxEmitterConfiguration _boxConfiguration;
      #endregion
   }
}
