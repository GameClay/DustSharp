
using System;

namespace GameClay
{
   namespace DustBuster
   {
      public class SoADataTest : Dust.SoAData, ITest
      {
         #region ITest implementation
         public Result RunTest()
         {
            Random rand = new Random();
            bool passed = true;

            // Initialize arrays, and test the properties
            Dust.SoAData d2 = new Dust.SoAData(MaxNumParticles);
            _numParticles = MaxNumParticles;
            d2._numParticles = MaxNumParticles;
            for (int i = 0; i < MaxNumParticles; i++)
            {
               //_positionStream[i] = null;
               _lifespanStream[i] = (float)rand.NextDouble();
               //_velocityStream[i] = null;
               _massStream[i] = 1.0f;

               //passed &= (Position[i] == _positionStream[i]);
               passed &= (Lifespan[i] == _lifespanStream[i]);
               //passed &= (Velocity[i] == _velocityStream[i]);
               passed &= (Mass[i] == _massStream[i]);

               // Initialize the second SoAData
               //d2._positionStream[i] = null;
               d2._lifespanStream[i] = (float)rand.NextDouble();
               //d2._velocityStream[i] = null;
               d2._massStream[i] = float.PositiveInfinity;

               if (!passed)
                  break;
            }

            // Test CopyElement
            int numCopyTests = NumParticles % 7;
            for (int i = 0; i < numCopyTests; i++)
            {
               int src = rand.Next(NumParticles - 1);
               int dst = rand.Next(NumParticles - 1);

               CopyElement(src, dst);

               //passed &= (Position[src] == Position[dst]);
               passed &= (Lifespan[src] == Lifespan[dst]);
               //passed &= (Velocity[src] == Velocity[dst]);
               passed &= (Mass[src] == Mass[dst]);

               if (!passed)
                  break;
            }

            // Test CopyFrom
            _numParticles = 0;
            Dust.ISystemData isd = (Dust.ISystemData)d2;
            int copyFromResult = CopyFrom(0, ref isd, NumParticles, NumParticles);
            passed &= (copyFromResult == NumParticles);

            for (int i = 0; i < NumParticles; i++)
            {
               //passed &= (Position[i] == d2.Position[i]);
               passed &= (Lifespan[i] == d2.Lifespan[i]);
               //passed &= (Velocity[i] == d2.Velocity[i]);
               passed &= (Mass[i] == d2.Mass[i]);

               if (!passed)
                  break;
            }

            return passed ? Result.Passed : Result.Failed;
         }

         #endregion

         public SoADataTest()
            : base(200)
         {

         }
      }
   }
}
