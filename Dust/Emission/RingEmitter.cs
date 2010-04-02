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

    public class RingEmitterConfiguration : EmitterConfiguration
    {

        public RingEmitterConfiguration () : base()
        {
        }
        
        #region Data
        
        #endregion
        
    }

    public class RingEmitter : BaseEmitter
    {
        public RingEmitterConfiguration RingConfiguration {
            get { return _ringConfiguration; }
        }

        protected override void _EmitParticles (int numParticlesToEmit, out ISystemData particlesToEmit)
        {
            // TODO: Pool this stuff
            if (_particlesToEmit.MaxNumParticles < numParticlesToEmit)
                _particlesToEmit = new SoAData(numParticlesToEmit);

            #if DUST_SIMD
            // Batch in chunks of 4 for nice maths
            int numBatches = numParticlesToEmit / 4;
            int numRemainder = numParticlesToEmit % 4;
            #else
            const int numBatches = 0;
            int numRemainder = numParticlesToEmit;
            #endif
            float oneOverPPS = Configuration.Persistent ? 1.0f / Configuration.ParticlesPerSecond : 0.0f;
            float radius = 1f;
            
            #if DUST_SIMD
            // Temp data
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
            
            // Radius doesn't change
            float[] radius = new float[4];
            radius[0] = 1f;
            radius[1] = 1f;
            radius[2] = 1f;
            radius[3] = 1f;
            
            // TODO: Revisit this after XNA is done.
            throw new NotImplementedException ();
            
            // Emit particles in chunks of 4
            for (int i = 0; i < numBatches; i++) {
                // Calculate partial frame time to presimulate. 
                partialFrameTime[0] = (i + 1) * oneOverPPS;
                partialFrameTime[1] = (i + 2) * oneOverPPS;
                partialFrameTime[2] = (i + 3) * oneOverPPS;
                partialFrameTime[3] = (i + 4) * oneOverPPS;
                
                // Get the random angles into Z           
                posZ[0] = (float)(RandomSource.NextDouble () * Math.PI * 2);
                posZ[1] = (float)(RandomSource.NextDouble () * Math.PI * 2);
                posZ[2] = (float)(RandomSource.NextDouble () * Math.PI * 2);
                posZ[3] = (float)(RandomSource.NextDouble () * Math.PI * 2);
                
                // Get X and Y
                posX[0] = (float)Math.Cos (posZ[0]);
                posX[1] = (float)Math.Cos (posZ[1]);
                posX[2] = (float)Math.Cos (posZ[2]);
                posX[3] = (float)Math.Cos (posZ[3]);
                
                posY[0] = (float)Math.Sin (posZ[0]);
                posY[1] = (float)Math.Sin (posZ[1]);
                posY[2] = (float)Math.Sin (posZ[2]);
                posY[3] = (float)Math.Sin (posZ[3]);
                
                // Zero-out Z
                posZ[0] = 0f;
                posZ[1] = 0f;
                posZ[2] = 0f;
                posZ[3] = 0f;
                
                // Multiply by radius
                posX[0] *= radius[0];
                posX[1] *= radius[1];
                posX[2] *= radius[2];
                posX[3] *= radius[3];
                
                posY[0] *= radius[0];
                posY[1] *= radius[1];
                posY[2] *= radius[2];
                posY[3] *= radius[3];
                
                // If this emitter is supposed to emit only on the surface
                // don't need to get a random distance
                if (!Configuration.EmitOnSurfaceOnly) {
                    float[] distance = new float[4];
                    distance[0] = (float)RandomSource.NextDouble ();
                    distance[1] = (float)RandomSource.NextDouble ();
                    distance[2] = (float)RandomSource.NextDouble ();
                    distance[3] = (float)RandomSource.NextDouble ();
                    
                    posX[0] *= distance[0];
                    posX[1] *= distance[1];
                    posX[2] *= distance[2];
                    posX[3] *= distance[3];
                    
                    posY[0] *= distance[0];
                    posY[1] *= distance[1];
                    posY[2] *= distance[2];
                    posY[3] *= distance[3];
                }
                
                // Transform position
                // TODO: Matrix and transform stuff
                
                // Calculate velocity
                
                // Get length
                len[0] = (float)Math.Sqrt ((posX[0] * posX[0]) + (posY[0] * posY[0]) + (posZ[0] * posZ[0]));
                len[1] = (float)Math.Sqrt ((posX[1] * posX[1]) + (posY[1] * posY[1]) + (posZ[1] * posZ[1]));
                len[2] = (float)Math.Sqrt ((posX[2] * posX[2]) + (posY[2] * posY[2]) + (posZ[2] * posZ[2]));
                len[3] = (float)Math.Sqrt ((posX[3] * posX[3]) + (posY[3] * posY[3]) + (posZ[3] * posZ[3]));
                
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
                if (oneOverPPS > 0) {
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
                Array.Copy (posX, 0, _particlesToEmit._positionStreamX, iTimesFour, 4);
                Array.Copy (posY, 0, _particlesToEmit._positionStreamY, iTimesFour, 4);
                Array.Copy (posZ, 0, _particlesToEmit._positionStreamZ, iTimesFour, 4);
                
                // Assign velocity
                Array.Copy (velX, 0, _particlesToEmit._velocityStreamX, iTimesFour, 4);
                Array.Copy (velY, 0, _particlesToEmit._velocityStreamY, iTimesFour, 4);
                Array.Copy (velZ, 0, _particlesToEmit._velocityStreamZ, iTimesFour, 4);
                
                // Assign lifespan and mass
                Array.Copy (preSimLifespan, 0, _particlesToEmit.Lifespan, iTimesFour, 4);
                Array.Copy (preSimLifespan, 0, _particlesToEmit.TimeRemaining, iTimesFour, 4);
                Array.Copy (initialMass, 0, _particlesToEmit.Mass, iTimesFour, 4);
            }
            #endif
            
            // Emit remaining particles individually
            int numBatchesTimesFour = numBatches * 4;
            for (int i = 0; i < numRemainder; i++) {
                // Calculate pos on circle with a random angle
                float angle = (float)(RandomSource.NextDouble () * Math.TwoPI);
                float posX = (float)Math.Cos (angle) * radius;
                float posY = (float)Math.Sin (angle) * radius;
                float posZ = 0f;
                
                // If this emitter is supposed to emit only on the surface
                // don't need to get a random distance
                if (!Configuration.EmitOnSurfaceOnly) {
                    float distance = (float)RandomSource.NextDouble ();
                    posX *= distance;
                    posY *= distance;
                }
                
                // Transform position
                // TODO: Matrix and transform stuff
                
                // Length
                float len = (float)Math.Sqrt ((posX * posX) + (posY * posY) + (posZ * posZ));
                
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

        public RingEmitter () : base()
        {
            _particlesToEmit = new SoAData (200);
            
            // Replace the base configuration with our specalized one
            _ringConfiguration = new RingEmitterConfiguration ();
            _configuration = (EmitterConfiguration)_ringConfiguration;
        }

        #region Data
        protected SoAData _particlesToEmit;
        protected RingEmitterConfiguration _ringConfiguration;
        #endregion
    }
}
