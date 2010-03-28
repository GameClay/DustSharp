
using System;

namespace GameClay.Dust
{


   public class BoxEmitter : EmitterImpl, IEmitter
   {
      
      #region IEmitter implementation      
      
      public new ISimulation Simulation
      {
         get
         {
            throw new System.NotImplementedException();
         }
         set 
         {
            throw new System.NotImplementedException();
         }
      }
      
      
      public new object Transform 
      {
         get 
         {
            throw new System.NotImplementedException();
         }
         set 
         {
            throw new System.NotImplementedException();
         }
      }
      
      
      public new int Seed 
      {
         get 
         {
            throw new System.NotImplementedException();
         }
         set 
         {
            throw new System.NotImplementedException();
         }
      }
      
      
      public new bool Active 
      {
         get
         {
            throw new System.NotImplementedException();
         }
         set
         {
            throw new System.NotImplementedException();
         }
      }
      
      #endregion
      
      protected override int _EmitParticles(int numParticlesToEmit)
      {
         return 0;
      }
  
      public BoxEmitter()
         : base()
      {
      }
      
      #region Data
      
      #endregion
   }
}
