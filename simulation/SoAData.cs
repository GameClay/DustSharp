
using System;
using SlimMath;

namespace GameClay
{
   namespace Dust
   {
      /// <summary>
      /// An ISystemData implemented as a structure of arrays.
      /// </summary>
      public class SoAData : ISystemData
      {
         #region ISystemData implementation
         public int NumParticles
         {
            get
            {
               return _numParticles;
            }
         }

         public int MaxNumParticles
         {
            get
            {
               return _maxNumParticles;
            }
         }

         public float[] PositionX
         {
            get
            {
               return _positionStreamX;
            }
         }
         
         public float[] PositionY
         {
            get
            {
               return _positionStreamY;
            }
         }
         
         public float[] PositionZ
         {
            get
            {
               return _positionStreamZ;
            }
         }

         public float[] Lifespan
         {
            get
            {
               return _lifespanStream;
            }
         }

         public float[] VelocityX
         {
            get
            {
               return _velocityStreamX;
            }
         }
         
         public float[] VelocityY
         {
            get
            {
               return _velocityStreamY;
            }
         }
         
         public float[] VelocityZ
         {
            get
            {
               return _velocityStreamZ;
            }
         }

         public float[] Mass
         {
            get
            {
               return _massStream;
            }
         }

         public int CopyFrom(int offset, ref ISystemData src, int srcOffset, int count)
         {
            int capacityLeft = MaxNumParticles - NumParticles;
            int numToCopy = count < capacityLeft ? count : capacityLeft;
            
            // TODO: Test the speed of Array.Copy vs Buffer.BlockCopy 

            // Position
            Buffer.BlockCopy(src.PositionX, srcOffset, _positionStreamX, offset, numToCopy);
            Buffer.BlockCopy(src.PositionY, srcOffset, _positionStreamY, offset, numToCopy);
            Buffer.BlockCopy(src.PositionZ, srcOffset, _positionStreamZ, offset, numToCopy);

            // Lifespan
            Buffer.BlockCopy(src.Lifespan, srcOffset, _lifespanStream, offset, numToCopy);

            // Velocity
            Buffer.BlockCopy(src.VelocityX, srcOffset, _velocityStreamX, offset, numToCopy);
            Buffer.BlockCopy(src.VelocityY, srcOffset, _velocityStreamY, offset, numToCopy);
            Buffer.BlockCopy(src.VelocityZ, srcOffset, _velocityStreamZ, offset, numToCopy);

            // Mass
            Buffer.BlockCopy(src.Mass, srcOffset, _massStream, offset, numToCopy);

            // Update number of particles
            _numParticles += numToCopy;

            // Return number copied
            return numToCopy;
         }

         public void CopyElement(int srcIndex, int dstIndex)
         {
            // This isn't super awesome, in the SoA case, but what can ya do
            _positionStreamX[dstIndex] = _positionStreamX[srcIndex];
            _positionStreamY[dstIndex] = _positionStreamY[srcIndex];
            _positionStreamZ[dstIndex] = _positionStreamZ[srcIndex];
            
            _lifespanStream[dstIndex] = _lifespanStream[srcIndex];
            
            _velocityStreamX[dstIndex] = _velocityStreamX[srcIndex];
            _velocityStreamY[dstIndex] = _velocityStreamY[srcIndex];
            _velocityStreamZ[dstIndex] = _velocityStreamZ[srcIndex];
            
            _massStream[dstIndex] = _massStream[srcIndex];
         }

         #endregion

         public SoAData(int maxNumParticles)
         {
            _numParticles = 0;
            _maxNumParticles = maxNumParticles;

            _positionStreamX = new float[MaxNumParticles];
            _positionStreamY = new float[MaxNumParticles];
            _positionStreamZ = new float[MaxNumParticles];
            
            _lifespanStream = new float[MaxNumParticles];
            
            _velocityStreamX = new float[MaxNumParticles];
            _velocityStreamY = new float[MaxNumParticles];
            _velocityStreamZ = new float[MaxNumParticles];
            
            _massStream = new float[MaxNumParticles];
         }

         #region Data
         public int _numParticles;
         public int _maxNumParticles;
         
         public float[] _positionStreamX;
         public float[] _positionStreamY;
         public float[] _positionStreamZ;
         
         public float[] _lifespanStream;
         
         public float[] _velocityStreamX;
         public float[] _velocityStreamY;
         public float[] _velocityStreamZ;
         
         public float[] _massStream;
         #endregion
      }
   }
}