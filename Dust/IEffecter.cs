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
    /// Manipulates the particles in a Simulation to create localized effects.
    /// </summary>
    ///
    /// <remarks>
    /// An Effecter is an object which manipulates the particles in a Simulation.
    /// These effecters can be used to create almost limitless customizable particle movement
    /// which, when combined with shared simulations and emitter-specific representations,
    /// can create dazzling effects. Each Effecter attached to a Simulation adds
    /// at least an additional iteration over the SimulationData each frame.
    /// </remarks>
    public interface IEffecter
    {
        /// <summary>
        /// Assigns the Simulation this Effecter will manipulate.
        /// </summary>
        bool Simulation { get; set; }

        /// <summary>
        /// An object-to-world transform for this Effecter.
        /// </summary>
        object Transform { get; set; } // TODO: Matrix

        /// <summary>
        /// Advances the Effecter physics by a number of seconds.
        /// </summary>
        ///
        /// <param name="dt">The time which has passed since the last call to AdvanceTime(), in seconds.</param>
        void advanceTime(float dt);
    }
}
