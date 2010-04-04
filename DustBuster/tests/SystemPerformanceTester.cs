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
using System.Diagnostics;
using System.Collections.Generic;

namespace GameClay.DustBuster
{

    public class SystemPerformanceTester : ITest
    {
        const int NumTestParticles = 3200000;
        const float TimeStep = 0.01f;
        const int NumTimesToRunTest = 50;

        #region ITest implementation
        public Result RunTest()
        {
            List<Dust.ISimulation> simulation = new List<Dust.ISimulation>();

            simulation.Add(new Dust.StandardSimulation(NumTestParticles));
            //simulation.Add(new Dust.UnsafeSimulation(NumTestParticles));
            //simulation.Add(new Dust.UnsafeThreadedSimulation(NumTestParticles));
            //simulation.Add(new Dust.HorribleSimulation(NumTestParticles));
#if DUST_MONO
            simulation.Add(new Dust.Mono.SimdSimulation(NumTestParticles));
            simulation.Add(new Dust.Mono.UnsafeSimdSimulation(NumTestParticles));
#endif

            // Blast out some particles into the simulations
            Dust.BoxEmitter emitter = new Dust.BoxEmitter();
            emitter.Configuration.ParticlesPerSecond = NumTestParticles;
            emitter.Configuration.Persistent = false;
            for (int i = 0; i < simulation.Count; i++)
            {
                emitter.Active = true;
                emitter.Simulation = simulation[i];
                simulation[i].RemoveAllParticles();
                emitter.AdvanceTime(TimeStep, TimeStep);
            }

            Stopwatch testTimer = new Stopwatch();

            float[,] simulationResults = new float[simulation.Count, NumTimesToRunTest];

            for (int j = 0; j < simulation.Count; j++)
            {
                // Run once without profiling just to engage the JIT, then run the tests for real
                simulation[j].AdvanceTime(TimeStep);

                for (int i = 0; i < NumTimesToRunTest; i++)
                {
                    testTimer.Reset();
                    testTimer.Start();
                    simulation[j].AdvanceTime(TimeStep);
                    testTimer.Stop();
                    simulationResults[j, i] = (float)testTimer.ElapsedTicks * 1000 / (float)Stopwatch.Frequency;
                }
            }

            float[] simulationAvg = new float[simulation.Count];
            float[] simulationMin = new float[simulation.Count];
            float[] simulationMax = new float[simulation.Count];
            for (int i = 0; i < simulation.Count; i++)
            {
                simulationAvg[i] = 0;
                simulationMax[i] = 0;
                simulationMin[i] = float.PositiveInfinity;
            }

            for (int j = 0; j < simulation.Count; j++)
            {
                for (int i = 0; i < NumTimesToRunTest; i++)
                {
                    simulationAvg[j] += simulationResults[j, i];
                    simulationMin[j] = Math.Min(simulationMin[j], simulationResults[j, i]);
                    simulationMax[j] = Math.Max(simulationMax[j], simulationResults[j, i]);
                }
            }

            Console.WriteLine("   Ran tests with {0} partcicles {1} times:", NumTestParticles, NumTimesToRunTest);
            Console.WriteLine("   [timer        avg        min        max]");

            for (int i = 0; i < simulation.Count; i++)
            {
                simulationAvg[i] /= NumTimesToRunTest;
                string outptStr = "   " + simulation[i] + " " + simulationAvg[i] + "   " + simulationMin[i] + "   " + simulationMax[i];
                Console.WriteLine(outptStr);
            }

            return Result.Passed;
        }

        #endregion

        public SystemPerformanceTester()
        {

        }
    }
}
