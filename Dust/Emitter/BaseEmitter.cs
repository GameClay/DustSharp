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
    /// <summary>
    /// A base class for an <see cref="IEmitter"/> implementation.
    /// </summary>
    /// 
    /// <remarks>
    /// This helper class takes care of the <see cref="IEmitter.AdvanceTime"/> and property implementations, 
    /// only requiring that the derived class implement the <see cref="_EmitParticles"/> method.
    /// </remarks>
    public abstract class BaseEmitter : IEmitter
    {

        #region IEmitter implementation
        public int AdvanceTime(float dt, float pt)
        {
            int numParticlesEmitted = 0;

            if (Active)
            {
#if DEBUG
                // Test for null Simulation
                if (Simulation == null)
                    throw new System.NullReferenceException("No Simulation assigned.");
#endif

                _timeSinceEmission += dt;

                // Figure out the number of particles to emit
                int pps = _ParticlesPerSecond(pt);
                bool persistent = _IsPersistent(pt);
                int numParticlesToEmit = (int)(pps * (persistent ? _timeSinceEmission : 1));
                Active = persistent ? Active : false;

                // Emit particles
                if (numParticlesToEmit > 0)
                {
                    // Call _EmitParticles and add any resultant particles 
                    float oneOverPPS = persistent ? 0.0f : (1.0f / pps);
                    ISystemData particlesToEmit = null;
                    _EmitParticles(numParticlesToEmit, oneOverPPS, dt, pt, out particlesToEmit);

                    if (particlesToEmit != null)
                    {
                        // Clear the time since the last emission, leaving any time for partially emitted particles intact
                        _timeSinceEmission -= oneOverPPS * particlesToEmit.NumParticles;

                        // Add particles to simulation
                        Simulation.AddParticles(ref particlesToEmit);
                    }
                }
            }

            return numParticlesEmitted;
        }

        public ISimulation Simulation
        {
            get
            {
                return _simulation;
            }
            set
            {
                _simulation = value;
            }
        }


        public object Transform
        {
            get
            {
                return _transform;
            }
            set
            {
                _transform = value;
            }
        }


        public int Seed
        {
            get
            {
                return _seed;
            }
            set
            {
                _seed = value;
                // RandomSource needs new seed
            }
        }


        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
            }
        }

        #endregion

        #region Properties
        public IParameters Parameters
        {
            get
            {
                return _parameters;
            }
        }

        /// <summary>
        /// The random number generator for the implementation to use.
        /// </summary>
        /// 
        /// <remarks>
        /// This <see cref="Random"/> is initialized using the value obtained from <see cref="IEmitter.Random"/>.
        /// </remarks>
        protected System.Random RandomSource
        {
            // TODO: Investigate performance of System.Random, and all the casts from double-to-float which is done
            get
            {
                return _randomSource;
            }
        }
        #endregion

        /// <summary>
        /// This method is a request to emit a number of particles. The implementor should assign, and populate 
        /// the <see cref="ISystemData"/> passed as a parameter with the particles it does emit.
        /// </summary>
        /// 
        /// <remarks>
        /// If the value of  <see cref="particlesToEmit"/> is not <see cref="null"/>, <see cref="ISimulation.AddParticles"/>
        /// will be called following the return of this method.
        /// </remarks>
        /// 
        /// <param name="numParticlesToEmit"> The number of particles requested for emission </param>
        /// <param name="oneOverPPS"> A value which does not need to be recomputed, and is needed in most emitters (1.0 / ParticlesPerSecond) </param>
        /// <param name="dt"> The time, in seconds, since the last call to AdvanceTime(). </param>
        /// <param name="pt"> A normalized value which is to used by the Emitter as an interpolation parameter. </param>
        /// <param name="particlesToEmit"> A <see cref="ISystemData"/> full of particles to add to the <see cref="ISimulation"/>. </param>
        protected abstract void _EmitParticles(int numParticlesToEmit, float oneOverPPS, float dt, float pt, out ISystemData particlesToEmit);


        public BaseEmitter(IParameters parameters)
        {
            _timeSinceEmission = 0;
            _seed = 0;
            _parameters = parameters;
            _randomSource = new System.Random(Seed);

            // Get the delegates we need from the parameters
            _ParticlesPerSecond = Parameters.GetParameterDelegate<int>("ParticlesPerSecond");
            _IsPersistent = Parameters.GetParameterDelegate<bool>("Persistent");
            _EmitOnSurfaceOnly = Parameters.GetParameterDelegate<bool>("EmitOnSurfaceOnly");

            _InitialMass = Parameters.GetParameterDelegate<float>("InitialMass");
            _InitialLifespan = Parameters.GetParameterDelegate<float>("InitialLifespan");
            _InitialSpeed = Parameters.GetParameterDelegate<float>("InitialSpeed");
        }

        #region Data
        protected float _timeSinceEmission;
        protected IParameters _parameters;
        protected System.Random _randomSource;
        protected bool _active;
        protected int _seed;
        protected ISimulation _simulation;
        protected object _transform;

        protected System.Func<float, int> _ParticlesPerSecond;
        protected System.Func<float, bool> _IsPersistent;
        protected System.Func<float, bool> _EmitOnSurfaceOnly;

        protected System.Func<float, float> _InitialMass;
        protected System.Func<float, float> _InitialLifespan;
        protected System.Func<float, float> _InitialSpeed;
        #endregion
    }
}
