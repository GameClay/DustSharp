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

using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace GameClay.DustBuster
{

    public class SystemPerformanceTester : ITest
    {
        const int NumTestParticles = 50000;
        const float TimeStep = 0.01f;
        const int NumTimesToRunTest = 50;

        #region ITest implementation
        public Result RunTest()
        {
            List<Dust.ISimulation> simulation = new List<Dust.ISimulation>();

            simulation.Add(new Dust.Simulation.StandardSimulation(NumTestParticles));
            //simulation.Add(new Dust.Simulation.UnmanagedSimulation(NumTestParticles));
#if __MonoCS__
            simulation.Add(new Dust.Mono.Simulation.SimdSimulation(NumTestParticles));
            simulation.Add(new Dust.Mono.Simulation.UnsafeSimdSimulation(NumTestParticles));
#endif

            // Blast out some particles into the simulations
            Dust.Parameters config = new Dust.Parameters();
            config.SetParameter("ParticlesPerSecond", NumTestParticles);
            config.SetParameter("Persistent", false);
            config.SetParameter("InitialLifespan", 5.0f);
            config.SetParameter("InitialSpeed", 1.0f);

            Dust.Emitter.BoxEmitter emitter = new Dust.Emitter.BoxEmitter(config);

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
                    simulationResults[j, i] = testTimer.ElapsedTicks * 1000 / (float)Stopwatch.Frequency;
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
    }
}
