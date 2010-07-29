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

    /// <summary>
    /// The base configuration class for an emitter.
    /// </summary>
    public abstract class EmitterConfiguration
    {

        #region Properties
        /// <summary>
        /// If false, this emitter will emit it's <see cref="ParticlesPerSecond"/> once, and then deactivate.
        /// </summary>
        /// 
        /// <remarks>
        /// This property allows you to make "one-shot" emitters, which emits a number of particles equal
        /// to the value of <see cref="ParticlesPerSecond"/> during the first call to <see cref="IEmitter.AdvanceTime"/>,
        /// and then deactivates (<see cref="IEmitter.Active"/>).
        /// </remarks>
        public bool Persistent
        {
            get
            {
                return _persistent;
            }
            set
            {
                _persistent = value;
            }
        }

        /// <summary>
        /// The number of particles to emit per second. 
        /// </summary>
        public float ParticlesPerSecond
        {
            get
            {
                return _particlesPerSecond;
            }
            set
            {
                _particlesPerSecond = value;
            }
        }

        /// <summary>
        /// The initial speed of the particles emitted.
        /// </summary>
        public float InitialSpeed
        {
            get
            {
                return _initialSpeed;
            }
            set
            {
                _initialSpeed = value;
            }
        }

        /// <summary>
        /// The initial mass of the particles emitted.
        /// </summary>
        public float InitialMass
        {
            get
            {
                return _initialMass;
            }
            set
            {
                _initialMass = value;
            }
        }

        /// <summary>
        /// Initial lifespan of the particles emitted.
        /// </summary>
        public float InitialLifespan
        {
            get
            {
                return _initialLifespan;
            }
            set
            {
                _initialLifespan = value;
            }
        }

        /// <summary>
        /// Emit only on the surface of the emission patern.
        /// </summary>
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
        #endregion

        public EmitterConfiguration()
        {
            _particlesPerSecond = 1.0f;
            _initialLifespan = 1.0f;
            _initialSpeed = 1.0f;
            _initialMass = 1.0f;
            _persistent = true;
            _emitOnSurfaceOnly = false;
        }

        #region Data
        protected float _particlesPerSecond;
        protected float _initialLifespan;
        protected float _initialSpeed;
        protected float _initialMass;
        protected bool _persistent;
        protected bool _emitOnSurfaceOnly;
        #endregion


    }
}
