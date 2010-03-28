
using System;

namespace GameClay.Dust
{


   public class EmitterConfiguration
   {

      #region Properties
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
      #endregion
      
      public EmitterConfiguration ()
      {
      }
      
      #region Data
      protected float _particlesPerSecond;
      protected float _initialLifespan;
      protected float _initialSpeed;
      protected float _initialMass;
      protected bool _persistent;
      #endregion
      
      
   }
}
