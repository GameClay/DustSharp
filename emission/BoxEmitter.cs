
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
         
         // Initial mass doesn't change
         initialMass[0] = Configuration.InitialMass;
         initialMass[1] = Configuration.InitialMass;
         initialMass[2] = Configuration.InitialMass;
         initialMass[3] = Configuration.InitialMass;
         
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
            // Some per-batch constants to make life easier and faster  
#if !DUST_SIMD
            float partialFrameTime = i * oneOverPPS; // NOTE: This will evaluate to 0 if Configuration.Persistent is false
            float ils = Configuration.InitialLifespan - partialFrameTime;
            initialLifespan[0] = ils;
            initialLifespan[1] = ils;
            initialLifespan[2] = ils;
            initialLifespan[3] = ils;
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
            
            // Copy and normalize to get velocity
#if !DUST_SIMD            
            len[0] = (float)Math.Sqrt((posX[0] * posX[0]) + (posY[0] * posY[0]) + (posZ[0] * posZ[0]));
            len[1] = (float)Math.Sqrt((posX[1] * posX[1]) + (posY[1] * posY[1]) + (posZ[1] * posZ[1]));
            len[2] = (float)Math.Sqrt((posX[2] * posX[2]) + (posY[2] * posY[2]) + (posZ[2] * posZ[2]));
            len[3] = (float)Math.Sqrt((posX[3] * posX[3]) + (posY[3] * posY[3]) + (posZ[3] * posZ[3]));
            
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
#else
#endif
            
            // Avoid clumping by doing some pre-simulation
            if (Configuration.Persistent)
            {
               posX[0] += velX[0] * partialFrameTime;
               posY[0] += velY[0] * partialFrameTime;
               posZ[0] += velZ[0] * partialFrameTime;
            }
            
            // Assign position
#if !DUST_SIMD
            Buffer.BlockCopy(posX, 0, _particlesToEmit.PositionX, 0, 4);
            Buffer.BlockCopy(posY, 0, _particlesToEmit.PositionY, 0, 4);
            Buffer.BlockCopy(posZ, 0, _particlesToEmit.PositionZ, 0, 4);
#else
#endif
            
            // Assign velocity
#if !DUST_SIMD
            Buffer.BlockCopy(velX, 0, _particlesToEmit.VelocityX, 0, 4);
            Buffer.BlockCopy(velY, 0, _particlesToEmit.VelocityY, 0, 4);
            Buffer.BlockCopy(velZ, 0, _particlesToEmit.VelocityZ, 0, 4);
#else
#endif
            
            // Assign lifespan and mass
            Buffer.BlockCopy(initialLifespan, 0, _particlesToEmit.Lifespan, i * 4, 4);
            Buffer.BlockCopy(initialMass, 0, _particlesToEmit.Mass, i * 4, 4);
         }
         
         
#if !DUST_BATCH_EMISSION_ONLY
         // Emit remaining particles individually
         for (int i = 0; i < numRemainder; i++)
         {
            
         }
#endif
         
         // Assign number of particles
         _particlesToEmit.NumParticles = numParticlesToEmit;
         
         // Assign the output variable
         particlesToEmit = _particlesToEmit;
      }
  
      public BoxEmitter()
         : base()
      {
         _particlesToEmit = new SoAData(200);
         
         // Replace the base configuration with our specalized one
         _boxConfiguration = new BoxEmitterConfiguration();
         _configuration = (EmitterConfiguration)_boxConfiguration;
      }
      
      #region Data
      protected SoAData _particlesToEmit;
      protected BoxEmitterConfiguration _boxConfiguration;
      #endregion
   }
}
