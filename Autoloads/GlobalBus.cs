using Godot;
using System;

public partial class GlobalBus : Node
{
   [Signal]
   public delegate void ExplosionRequestedEventHandler(Vector2 position, Vector2 velocity);

   public void RequestExplosion(Vector2 position, Vector2 velocity)
   {
      EmitSignal(nameof(SignalName.ExplosionRequested), position, velocity);
   }

}
