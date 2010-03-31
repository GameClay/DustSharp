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

      public float[] Lifespan
      {
         get
         {
            return _lifespanStream;
         }
      }

      public float[] TimeRemaining
      {
         get
         {
            return _timeRemainingStream;
         }
      }

      public object[] UserData
      {
          get
          {
              return _userDataStream;
          }
      }

      public int CopyFrom(int offset, ref ISystemData src, int srcOffset, int count)
      {
         int capacityLeft = MaxNumParticles - NumParticles;
         int numToCopy = count < capacityLeft ? count : capacityLeft;

         // Position
         Array.Copy(src.PositionX, srcOffset, _positionStreamX, offset, numToCopy);
         Array.Copy(src.PositionY, srcOffset, _positionStreamY, offset, numToCopy);
         Array.Copy(src.PositionZ, srcOffset, _positionStreamZ, offset, numToCopy);

         // Time
         Array.Copy(src.Lifespan, srcOffset, _lifespanStream, offset, numToCopy);
         Array.Copy(src.TimeRemaining, srcOffset, _timeRemainingStream, offset, numToCopy);

         // Velocity
         Array.Copy(src.VelocityX, srcOffset, _velocityStreamX, offset, numToCopy);
         Array.Copy(src.VelocityY, srcOffset, _velocityStreamY, offset, numToCopy);
         Array.Copy(src.VelocityZ, srcOffset, _velocityStreamZ, offset, numToCopy);

         // Mass
         Array.Copy(src.Mass, srcOffset, _massStream, offset, numToCopy);

         // UserData
         Array.Copy(src.UserData, srcOffset, _userDataStream, offset, numToCopy);

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
         _timeRemainingStream[dstIndex] = _timeRemainingStream[srcIndex];

         _velocityStreamX[dstIndex] = _velocityStreamX[srcIndex];
         _velocityStreamY[dstIndex] = _velocityStreamY[srcIndex];
         _velocityStreamZ[dstIndex] = _velocityStreamZ[srcIndex];

         _massStream[dstIndex] = _massStream[srcIndex];

         _userDataStream[dstIndex] = _userDataStream[srcIndex];
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
         _timeRemainingStream = new float[MaxNumParticles];

         _velocityStreamX = new float[MaxNumParticles];
         _velocityStreamY = new float[MaxNumParticles];
         _velocityStreamZ = new float[MaxNumParticles];

         _massStream = new float[MaxNumParticles];

         _userDataStream = new object[MaxNumParticles];
      }

      #region Data
      public int _numParticles;
      public int _maxNumParticles;

      public float[] _positionStreamX;
      public float[] _positionStreamY;
      public float[] _positionStreamZ;

      public float[] _lifespanStream;
      public float[] _timeRemainingStream;

      public float[] _velocityStreamX;
      public float[] _velocityStreamY;
      public float[] _velocityStreamZ;

      public float[] _massStream;

      public object[] _userDataStream;
      #endregion
   }
}