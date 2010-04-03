
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
                    _positionStreamX[i] = (float)rand.NextDouble();
                    _positionStreamY[i] = (float)rand.NextDouble();
                    _positionStreamZ[i] = (float)rand.NextDouble();

                    _lifespanStream[i] = (float)rand.NextDouble();

                    _velocityStreamX[i] = (float)rand.NextDouble();
                    _velocityStreamY[i] = (float)rand.NextDouble();
                    _velocityStreamZ[i] = (float)rand.NextDouble();

                    _massStream[i] = 1.0f;

                    passed &= (PositionX[i] == _positionStreamX[i]);
                    passed &= (PositionY[i] == _positionStreamY[i]);
                    passed &= (PositionZ[i] == _positionStreamZ[i]);

                    passed &= (Lifespan[i] == _lifespanStream[i]);

                    passed &= (VelocityX[i] == _velocityStreamX[i]);
                    passed &= (VelocityY[i] == _velocityStreamY[i]);
                    passed &= (VelocityZ[i] == _velocityStreamZ[i]);

                    passed &= (Mass[i] == _massStream[i]);

                    // Initialize the second SoAData
                    d2._positionStreamX[i] = (float)rand.NextDouble();
                    d2._positionStreamY[i] = (float)rand.NextDouble();
                    d2._positionStreamZ[i] = (float)rand.NextDouble();

                    d2._lifespanStream[i] = (float)rand.NextDouble();

                    d2._velocityStreamX[i] = (float)rand.NextDouble();
                    d2._velocityStreamY[i] = (float)rand.NextDouble();
                    d2._velocityStreamZ[i] = (float)rand.NextDouble();

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

                    passed &= (PositionX[src] == PositionX[dst]);
                    passed &= (PositionY[src] == PositionY[dst]);
                    passed &= (PositionZ[src] == PositionZ[dst]);

                    passed &= (Lifespan[src] == Lifespan[dst]);

                    passed &= (VelocityX[src] == VelocityX[dst]);
                    passed &= (VelocityY[src] == VelocityY[dst]);
                    passed &= (VelocityZ[src] == VelocityZ[dst]);

                    passed &= (Mass[src] == Mass[dst]);

                    if (!passed)
                        break;
                }

                // Test CopyFrom
                _numParticles = 0;
                Dust.ISystemData isd = (Dust.ISystemData)d2;
                int copyFromResult = CopyFrom(0, ref isd, 0, isd.NumParticles);
                passed &= (copyFromResult == isd.NumParticles);

                for (int i = 0; i < NumParticles; i++)
                {
                    passed &= (PositionX[i] == d2.PositionX[i]);
                    passed &= (PositionY[i] == d2.PositionY[i]);
                    passed &= (PositionZ[i] == d2.PositionZ[i]);

                    passed &= (Lifespan[i] == d2.Lifespan[i]);

                    passed &= (VelocityX[i] == d2.VelocityX[i]);
                    passed &= (VelocityY[i] == d2.VelocityY[i]);
                    passed &= (VelocityZ[i] == d2.VelocityZ[i]);

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
