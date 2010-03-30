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
   /// This is a simple, single-buffered simulation which simply advances and
   /// times-out particles.
   /// </summary>
   public class ConstantSimulation : ISimulation
   {
      #region ISimulation implementation
      public void AdvanceTime(float dt)
      {
         // Reset the bounds of the system

         // Process the particle system
         for (int i = 0; i < _systemData.NumParticles; i++)
         {
            // Decrement the time remaining
            _systemData._timeRemainingStream[i] -= dt;

            // If the particle is out of time, destroy it
            if (_systemData._timeRemainingStream[i] < 0.0)
            {
               // Replace this element with the last element in the list
               int lastIdx = _systemData.NumParticles - 1;
               if (i < lastIdx)
               {
                  _systemData.CopyElement(lastIdx, i);

                  // Decrement i so that this particle will still get processed
                  i--;
               }

               // Decrement the number of particles
               _systemData._numParticles--;

               // Process the next item
               continue;
            }

            // Adjust the min/max values of the bounds
            // (this won't happen if the particle has been deleted)

         }
      }


      public int AddParticles(ref ISystemData particlesToAdd)
      {
         // Copy particles in to our structure
         int numAdded = _systemData.CopyFrom(_systemData.NumParticles,
                              ref particlesToAdd, 0, particlesToAdd.NumParticles);

         // Return number added
         return numAdded;
      }


      public void RemoveAllParticles()
      {
         // Clear out the system
         _systemData._numParticles = 0;

         // Zero-out the bounds
      }

      public object Bounds
      {
         get
         {
            return _worldBounds;
         }
      }

      public bool Is2D
      {
         get
         {
            return _is2D;
         }
         set
         {
            _is2D = value;
         }
      }


      public ISystemData SystemData
      {
         get
         {
            return _systemData;
         }
      }
      #endregion

      public ConstantSimulation(int maxNumParticles)
      {
         _systemData = new SoAData(maxNumParticles);
         _worldBounds = new object();
         _is2D = false;
      }

      #region Data
      SoAData _systemData;
      object _worldBounds;
      bool _is2D;
      #endregion
   }
}