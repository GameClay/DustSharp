/* Dust -- Copyright (C) 2009-2010 GameClay LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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
            // TODO: Pool this stuff
            if (_particlesToEmit.MaxNumParticles < numParticlesToEmit)
                _particlesToEmit = new SoAData(numParticlesToEmit);

            int numRemainder = numParticlesToEmit;
            const int numBatches = 0;

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
