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
        }

        #region Data
        protected float _width;
        protected float _height;
        protected float _depth;
        #endregion

    }

    public class BoxEmitter : BaseEmitter
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

#if DUST_SIMD
         // Batch in chunks of 4 for nice maths
         int numBatches = numParticlesToEmit / 4;
         int numRemainder = numParticlesToEmit % 4;
#else
            int numRemainder = numParticlesToEmit;
            const int numBatches = 0;
#endif

            // Some constants to make life easier and faster
            float twoWidth = BoxConfiguration.Width * 2.0f;
            float twoHeight = BoxConfiguration.Height * 2.0f;
            float twoDepth = BoxConfiguration.Depth * 2.0f;

            float negWidth = -BoxConfiguration.Width;
            float negHeight = -BoxConfiguration.Height;
            float negDepth = -BoxConfiguration.Depth;

            float oneOverPPS = Configuration.Persistent ? 1.0f / Configuration.ParticlesPerSecond : 0;

            // Surface-only mode modulo division value
            int planeMod = Simulation.Is2D ? 2 : 3;

#if DUST_SIMD
         // TODO: Revisit SIMD after XNA work is done
         throw new NotImplementedException();

         float[] posX = new float[4];
         float[] posY = new float[4];
         float[] posZ = new float[4];

         float[] velX = new float[4];
         float[] velY = new float[4];
         float[] velZ = new float[4];

         float[] len = new float[4];

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

         // Emit particles in chunks of 4
         for (int i = 0; i < numBatches; i++)
         {
            // Calculate partial frame time to presimulate.
            partialFrameTime[0] = (i + 1) * oneOverPPS;
            partialFrameTime[1] = (i + 2) * oneOverPPS;
            partialFrameTime[2] = (i + 3) * oneOverPPS;
            partialFrameTime[3] = (i + 4) * oneOverPPS;

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

            // Multiply by 2.0 * Width/Height/Depth of box
            posX[0] *= twoWidth;
            posX[1] *= twoWidth;
            posX[2] *= twoWidth;
            posX[3] *= twoWidth;

            posY[0] *= twoHeight;
            posY[1] *= twoHeight;
            posY[2] *= twoHeight;
            posY[3] *= twoHeight;

            posZ[0] *= twoDepth;
            posZ[1] *= twoDepth;
            posZ[2] *= twoDepth;
            posZ[3] *= twoDepth;

            // If this emitter is supposed to emit only on the surface
            // do the needed clipping
            if (Configuration.EmitOnSurfaceOnly)
            {
               switch (RandomSource.Next() % planeMod)
               {
                  case 0:
                     posX[0] = posX[0] < 0.0f ? negWidth : _boxConfiguration.Width;
                     posX[1] = posX[1] < 0.0f ? negWidth : _boxConfiguration.Width;
                     posX[2] = posX[2] < 0.0f ? negWidth : _boxConfiguration.Width;
                     posX[3] = posX[3] < 0.0f ? negWidth : _boxConfiguration.Width;
                     break;

                  case 1:
                     posY[0] = posY[0] < 0.0f ? negHeight : _boxConfiguration.Height;
                     posY[1] = posY[1] < 0.0f ? negHeight : _boxConfiguration.Height;
                     posY[2] = posY[2] < 0.0f ? negHeight : _boxConfiguration.Height;
                     posY[3] = posY[3] < 0.0f ? negHeight : _boxConfiguration.Height;
                     break;

                  case 2:
                     posZ[0] = posZ[0] < 0.0f ? negDepth : _boxConfiguration.Depth;
                     posZ[1] = posZ[1] < 0.0f ? negDepth : _boxConfiguration.Depth;
                     posZ[2] = posZ[2] < 0.0f ? negDepth : _boxConfiguration.Depth;
                     posZ[3] = posZ[3] < 0.0f ? negDepth : _boxConfiguration.Depth;
                     break;
               }
            }

            // Transform position
            // TODO: Matrix and transform stuff

            // Calculate velocity
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
            preSimLifespan[0] = initialLifespan[0] - partialFrameTime[0];
            preSimLifespan[1] = initialLifespan[1] - partialFrameTime[1];
            preSimLifespan[2] = initialLifespan[2] - partialFrameTime[2];
            preSimLifespan[3] = initialLifespan[3] - partialFrameTime[3];

            // Write output result to _particlesToEmit
            int iTimesFour = i * 4;

            // Assign position
            Array.Copy(posX, 0, _particlesToEmit._positionStreamX, iTimesFour, 4);
            Array.Copy(posY, 0, _particlesToEmit._positionStreamY, iTimesFour, 4);
            Array.Copy(posZ, 0, _particlesToEmit._positionStreamZ, iTimesFour, 4);

            // Assign velocity
            Array.Copy(velX, 0, _particlesToEmit._velocityStreamX, iTimesFour, 4);
            Array.Copy(velY, 0, _particlesToEmit._velocityStreamY, iTimesFour, 4);
            Array.Copy(velZ, 0, _particlesToEmit._velocityStreamZ, iTimesFour, 4);

            // Assign lifespan and mass
            Array.Copy(preSimLifespan, 0, _particlesToEmit.Lifespan, iTimesFour, 4);
            Array.Copy(preSimLifespan, 0, _particlesToEmit.TimeRemaining, iTimesFour, 4);
            Array.Copy(initialMass, 0, _particlesToEmit.Mass, iTimesFour, 4);
         }
#endif

            // Emit remaining particles individually
            int numBatchesTimesFour = numBatches * 4;
            for (int i = 0; i < numRemainder; i++)
            {
                // Get random position
                float posX = (float)(RandomSource.NextDouble() - 0.5) * twoWidth;
                float posY = (float)(RandomSource.NextDouble() - 0.5) * twoHeight;
                float posZ = (float)(RandomSource.NextDouble() - 0.5) * twoDepth;

                // If this emitter is supposed to emit only on the surface
                // do the needed clipping
                if (BoxConfiguration.EmitOnSurfaceOnly)
                {
                    switch (RandomSource.Next() % planeMod)
                    {
                        case 0:
                            posX = posX < 0.0f ? negWidth : _boxConfiguration.Width;
                            break;

                        case 1:
                            posY = posY < 0.0f ? negHeight : _boxConfiguration.Height;
                            break;

                        case 2:
                            posZ = posZ < 0.0f ? negDepth : _boxConfiguration.Depth;
                            break;
                    }
                }

                // Transform position
                // TODO: Matrix and transform stuff

                // Length
                float len = Math.Sqrt((posX * posX) + (posY * posY) + (posZ * posZ));

                // Normalize
                float velX = posX / len;
                float velY = posY / len;
                float velZ = posZ / len;

                // Scale by speed
                velX *= Configuration.InitialSpeed;
                velY *= Configuration.InitialSpeed;
                velZ *= Configuration.InitialSpeed;

                // Avoid clumping by doing some pre-simulation
                float preSimTime = (i + 1) * oneOverPPS;
                if (oneOverPPS > 0)
                {
                    posX += velX * preSimTime;
                    posY += velY * preSimTime;
                    posZ += velZ * preSimTime;
                }

                // Store out position
                _particlesToEmit._positionStreamX[numBatchesTimesFour + i] = posX;
                _particlesToEmit._positionStreamY[numBatchesTimesFour + i] = posY;
                _particlesToEmit._positionStreamZ[numBatchesTimesFour + i] = posZ;

                // Store out velocity
                _particlesToEmit._velocityStreamX[numBatchesTimesFour + i] = velX;
                _particlesToEmit._velocityStreamY[numBatchesTimesFour + i] = velY;
                _particlesToEmit._velocityStreamZ[numBatchesTimesFour + i] = velZ;

                // Store out lifespan and mass
                float lifespan = Configuration.InitialLifespan - preSimTime;
                _particlesToEmit._lifespanStream[numBatchesTimesFour + i] = lifespan;
                _particlesToEmit._timeRemainingStream[numBatchesTimesFour + i] = lifespan;
                _particlesToEmit._massStream[numBatchesTimesFour + i] = Configuration.InitialMass;
            }

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
