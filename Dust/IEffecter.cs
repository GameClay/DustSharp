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
