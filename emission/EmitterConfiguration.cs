
using System;

namespace GameClay.Dust
{

   /// <summary>
   /// The base configuration class for an emitter.
   /// </summary>
   public abstract class EmitterConfiguration
   {

      #region Properties
      /// <summary>
      /// If false, this emitter will emit it's <see cref="ParticlesPerSecond"/> once, and then deactivate.
      /// </summary>
      /// 
      /// <remarks>
      /// This property allows you to make "one-shot" emitters, which emits a number of particles equal
      /// to the value of <see cref="ParticlesPerSecond"/> during the first call to <see cref="IEmitter.advanceTime"/>,
      /// and then deactivates (<see cref="IEmitter.Active"/>).
      /// </remarks>
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

      /// <summary>
      /// The number of particles to emit per second. 
      /// </summary>
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

      /// <summary>
      /// The initial speed of the particles emitted.
      /// </summary>
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

      /// <summary>
      /// The initial mass of the particles emitted.
      /// </summary>
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

      /// <summary>
      /// Initial lifespan of the particles emitted.
      /// </summary>
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

      public EmitterConfiguration()
      {
         _particlesPerSecond = 1.0f;
         _initialLifespan = 1.0f;
         _initialSpeed = 1.0f;
         _initialMass = 1.0f;
         _persistent = true;
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
