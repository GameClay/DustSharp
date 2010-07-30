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

namespace GameClay.Dust.Emitter
{

    public class RingEmitter : BaseEmitter
    {

        protected override void _EmitParticles(int numParticlesToEmit, float oneOverPPS, float dt, float pt, out ISystemData particlesToEmit)
        {
            // TODO: Pool this stuff
            if (_particlesToEmit.MaxNumParticles < numParticlesToEmit)
                _particlesToEmit = new SoAData(numParticlesToEmit);

            const int numBatches = 0;
            int numRemainder = numParticlesToEmit;
            
            float initialMass = _InitialMass(pt);
            float initialSpeed = _InitialSpeed(pt);
            float initialLifespan = _InitialLifespan(pt);
            
            bool emitOnSurfaceOnly = _EmitOnSurfaceOnly(pt);

            float radius = _Radius(pt);
            
            // Emit remaining particles individually
            int numBatchesTimesFour = numBatches * 4;
            for (int i = 0; i < numRemainder; i++) {
                // Calculate pos on circle with a random angle
                float angle = (float)(RandomSource.NextDouble()) * MathF.TwoPI;
                float posX = MathF.Cos(angle) * radius;
                float posY = MathF.Sin(angle) * radius;
                float posZ = 0f;
                
                // If this emitter is supposed to emit only on the surface
                // don't need to get a random distance
                if (!emitOnSurfaceOnly) {
                    float distance = (float)RandomSource.NextDouble();
                    posX *= distance;
                    posY *= distance;
                }
                
                // Transform position
                // TODO: Matrix and transform stuff
                
                // Length
                float len = (float)MathF.Sqrt((posX * posX) + (posY * posY) + (posZ * posZ));
                
                // Normalize
                float velX = posX / len;
                float velY = posY / len;
                float velZ = posZ / len;
                
                // Scale by speed
                velX *= initialSpeed;
                velY *= initialSpeed;
                velZ *= initialSpeed;
                
                // Avoid clumping by doing some pre-simulation
                float preSimTime = (i + 1) * oneOverPPS;
                if (oneOverPPS > 0) {
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
                float lifespan = initialLifespan - preSimTime;
                _particlesToEmit._lifespanStream[numBatchesTimesFour + i] = lifespan;
                _particlesToEmit._timeRemainingStream[numBatchesTimesFour + i] = lifespan;
                _particlesToEmit._massStream[numBatchesTimesFour + i] = initialMass;
            }
            
            // Assign number of particles
            _particlesToEmit._numParticles = numParticlesToEmit;
            
            // Assign the output variable
            particlesToEmit = _particlesToEmit;
        }

        public RingEmitter(IParameters parameters) 
            : base(parameters)
        {
            _particlesToEmit = new SoAData(200);
            
            _Radius = Parameters.GetParameterDelegate<float>("Radius");
        }

        #region Data
        protected SoAData _particlesToEmit;
        protected ParameterDelegate<float> _Radius;
        #endregion
    }
}
