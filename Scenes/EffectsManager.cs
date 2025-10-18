using Asteroids;
using Godot;
using System;
using System.Linq;

public partial class EffectsManager : Node2D
{
   [Export]
   public PackedScene ExplosionScene { get; set; }

   public override void _Ready()
   {
      var globalBus = GetNode<GlobalBus>("/root/GlobalBus");

      globalBus.ExplosionRequested += OnExplosionRequested;
   }

   public void OnExplosionRequested(Vector2 position, Vector2 velocity)
   {
      var explosion = ExplosionScene.Instantiate<Explosion>();
      AddChild(explosion);
      explosion.GlobalPosition = position;
      explosion.VelocityComponent.Velocity = velocity;
   }
}
