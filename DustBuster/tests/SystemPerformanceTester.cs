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
#define DUST_MONO
using System;
using System.Diagnostics;

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
#if DUST_MONO
            const int NumSimulations = 4;
#else
            const int NumSimulations = 3;
#endif
            Dust.ISimulation[] simulation = new Dust.ISimulation[NumSimulations];

            simulation[0] = new Dust.StandardSimulation(NumTestParticles);
            simulation[1] = new Dust.UnmanagedSimulation(NumTestParticles);
            simulation[2] = new Dust.HorribleSimulation(NumTestParticles);
#if DUST_MONO
            simulation[3] = new Dust.Mono.SimdSimulation(NumTestParticles);
#endif
            // Blast out some particles into the simulations
            Dust.BoxEmitter emitter = new Dust.BoxEmitter();
            emitter.Configuration.ParticlesPerSecond = NumTestParticles;
            emitter.Configuration.Persistent = false;
            for (int i = 0; i < simulation.Length; i++)
            {
                emitter.Active = true;
                emitter.Simulation = simulation[i];
                simulation[i].RemoveAllParticles();
                emitter.AdvanceTime(TimeStep, TimeStep);
            }

            Stopwatch testTimer = new Stopwatch(); 

            float[,] simulationResults = new float[simulation.Length, NumTimesToRunTest];

            for (int j = 0; j < simulation.Length; j++)
            {
                for (int i = 0; i < NumTimesToRunTest; i++)
                {
                    testTimer.Reset();
                    testTimer.Start();
                    simulation[j].AdvanceTime(TimeStep);
                    testTimer.Stop();
                    simulationResults[j, i] = (float)testTimer.ElapsedTicks * 1000 / (float)Stopwatch.Frequency;
                }
            }

            float[] simulationAvg = new float[simulation.Length];
            float[] simulationMin = new float[simulation.Length];
            float[] simulationMax = new float[simulation.Length];
            for (int i = 0; i < simulation.Length; i++)
            {
                simulationAvg[i] = 0;
                simulationMax[i] = 0;
                simulationMin[i] = float.PositiveInfinity;
            }

            for (int j = 0; j < simulation.Length; j++)
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

            for (int i = 0; i < simulation.Length; i++)
            {
                simulationAvg[i] /= NumTimesToRunTest;
                Console.WriteLine("   " + simulation[i] + " " + simulationAvg[i] + "   " + simulationMin[i] + "   " + simulationMax[i]);
            }

            return Result.Passed;
        }

        #endregion

        public SystemPerformanceTester()
        {

        }
    }
}
