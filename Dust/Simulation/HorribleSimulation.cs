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
    class HorribleSystemData : ISystemData
    {
        public HorribleSystemData(int maxNumParticles)
        {
            _systemData = new HorribleParticle[maxNumParticles];
            _maxNumParticles = maxNumParticles;
            _numParticles = 0;
        }

        public struct HorribleParticle
        {
            public float px;
            public float py;
            public float pz;

            public float vx;
            public float vy;
            public float vz;

            public float lifespan;
            public float timeRemaining;
            public float mass;
            public object userdata;


            // Members needed only for the simulation...
            public Microsoft.Xna.Framework.Vector2 Momentum;
            public Microsoft.Xna.Framework.Vector2 Velocity;
            public float Inception;

            public void Update(float dt)
            {
                // Decrement the time remaining
                timeRemaining -= dt;

                // Update position due to velocity
                px += vx * dt;
                py += vy * dt;
                pz += vz * dt;

                // Update velocity due to air resistance/gravity
            }
        }

        public HorribleParticle[] _systemData;
        public int _maxNumParticles;
        public int _numParticles;

        #region ISystemData Members

        public int NumParticles
        {
            get { return _numParticles; }
        }

        public int MaxNumParticles
        {
            get { return _maxNumParticles; }
        }

        public float[] PositionX
        {
            get { throw new System.NotImplementedException(); }
        }

        public float[] PositionY
        {
            get { throw new System.NotImplementedException(); }
        }

        public float[] PositionZ
        {
            get { throw new System.NotImplementedException(); }
        }

        public float[] VelocityX
        {
            get { throw new System.NotImplementedException(); }
        }

        public float[] VelocityY
        {
            get { throw new System.NotImplementedException(); }
        }

        public float[] VelocityZ
        {
            get { throw new System.NotImplementedException(); }
        }

        public float[] Mass
        {
            get { throw new System.NotImplementedException(); }
        }

        public float[] Lifespan
        {
            get { throw new System.NotImplementedException(); }
        }

        public float[] TimeRemaining
        {
            get { throw new System.NotImplementedException(); }
        }

        public object[] UserData
        {
            get { throw new System.NotImplementedException(); }
        }

        public int CopyFrom(int offset, ref ISystemData src, int srcOffset, int count)
        {
            int capacityLeft = MaxNumParticles - NumParticles;
            int numToCopy = count < capacityLeft ? count : capacityLeft;

            for (int i = offset, j = srcOffset; i < numToCopy; i++, j++)
            {
                _systemData[i].px = src.PositionX[j];
                _systemData[i].py = src.PositionY[j];
                _systemData[i].pz = src.PositionZ[j];

                _systemData[i].vx = src.VelocityX[j];
                _systemData[i].vy = src.VelocityY[j];
                _systemData[i].vz = src.VelocityZ[j];

                _systemData[i].lifespan = src.Lifespan[i];
                _systemData[i].timeRemaining = src.TimeRemaining[i];
                _systemData[i].mass = src.Mass[i];
            }

            // Update number of particles
            _numParticles += numToCopy;

            // Return number copied
            return numToCopy;
        }

        public void CopyElement(int srcIndex, int dstIndex)
        {
            _systemData[dstIndex].px = _systemData[srcIndex].px;
            _systemData[dstIndex].py = _systemData[srcIndex].py;
            _systemData[dstIndex].pz = _systemData[srcIndex].pz;

            _systemData[dstIndex].vx = _systemData[srcIndex].vx;
            _systemData[dstIndex].vy = _systemData[srcIndex].vy;
            _systemData[dstIndex].vz = _systemData[srcIndex].vz;

            _systemData[dstIndex].lifespan = _systemData[srcIndex].lifespan;
            _systemData[dstIndex].timeRemaining = _systemData[srcIndex].timeRemaining;
            _systemData[dstIndex].mass = _systemData[srcIndex].mass;
        }

        #endregion
    }

    public class HorribleSimulation : ISimulation
    {
        #region ISimulation implementation
        public void AdvanceTime(float dt)
        {
            // Reset the bounds of the system

            // Process the particle system
            foreach (HorribleSystemData.HorribleParticle p in _systemData._systemData)
                p.Update(dt);

            // Check for particles we need to delete
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
            set
            {
#if DEBUG
                if (value.GetType() != typeof(SoAData))
                    throw new System.ArgumentException("Supplied ISystemData was not of type SoAData.");
#endif
                _systemData = (HorribleSystemData)value;
            }
        }
        #endregion

        public HorribleSimulation(int maxNumParticles)
        {
            _systemData = new HorribleSystemData(maxNumParticles);
            _worldBounds = new object();
            _is2D = false;
        }

        #region Data
        HorribleSystemData _systemData;
        object _worldBounds;
        bool _is2D;
        #endregion
    }
}
