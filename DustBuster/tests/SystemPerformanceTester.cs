#define DUST_MONO
using System;
using System.Diagnostics;

namespace GameClay.DustBuster
{

   public class SystemPerformanceTester : ITest
   {
      const int NumTestParticles = 1000000;
      const float TimeStep = 0.01f;
      const int NumTimesToRunTest = 50;
      
      #region ITest implementation
      public Result RunTest()
      {
         Dust.ISimulation simulation = null;

#if DUST_MONO
         simulation = new Dust.Mono.SimdSimulation(NumTestParticles);
#else
         simulation = new Dust.StandardSimulation(NumTestParticles);
#endif
         Dust.BoxEmitter emitter = new Dust.BoxEmitter();
         Stopwatch testTimer = new Stopwatch();
         
         // Blast out some particles into the simulation
         emitter.Configuration.ParticlesPerSecond = NumTestParticles;
         emitter.Configuration.Persistent = false;
         emitter.Simulation = simulation;
         
         float[] emitterResults = new float[NumTimesToRunTest];
         float[] simulationResults = new float[NumTimesToRunTest];
         
         for (int i = 0; i < NumTimesToRunTest; i++)
         {
            simulation.RemoveAllParticles();
            emitter.Active = true;
            
            testTimer.Reset();
            testTimer.Start();
            emitter.AdvanceTime(TimeStep, TimeStep);
            testTimer.Stop();
            emitterResults[i] = (float)testTimer.ElapsedTicks * 1000 / (float)Stopwatch.Frequency;
            
            testTimer.Reset();
            testTimer.Start();
            simulation.AdvanceTime(TimeStep);
            testTimer.Stop();
            simulationResults[i] = (float)testTimer.ElapsedTicks * 1000 / (float)Stopwatch.Frequency;
         }
         
         float emitterAvg = 0;
         float simulationAvg = 0;
         float emitterMin = float.PositiveInfinity;
         float emitterMax = 0;
         float simulationMin = float.PositiveInfinity;
         float simulationMax = 0;
         
         for (int i = 0; i < NumTimesToRunTest; i++)
         {
            emitterAvg += emitterResults[i];
            simulationAvg += simulationResults[i];
            
            emitterMin = Math.Min(emitterMin, emitterResults[i]);
            simulationMin = Math.Min(simulationMin, simulationResults[i]);
            
            emitterMax = Math.Max(emitterMax, emitterResults[i]);
            simulationMax = Math.Max(simulationMax, simulationResults[i]);
         }
         
         emitterAvg /= NumTimesToRunTest;
         simulationAvg /= NumTimesToRunTest;
         
         Console.WriteLine("   [timer        avg        min        max]");
         Console.WriteLine("   emitter     " + emitterAvg + "   " + emitterMin + "   " + emitterMax);
         Console.WriteLine("   simulation  " + simulationAvg + "   " + simulationMin + "   " + simulationMax);
         
         return Result.Passed;
      }
      
      #endregion
  
      public SystemPerformanceTester ()
      {
         
      }
   }
}
