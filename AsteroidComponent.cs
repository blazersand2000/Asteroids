using Asteroids.Interfaces;
using Godot;
using System;

public partial class AsteroidComponent : Node
{
   [Export]
   public Area2D AsteroidArea2D { get; set; }
   public float RotationSpeedRadians { get; set; } = 0f;
   public Vector2 Velocity { get; set; } = Vector2.Zero;

   public override void _Ready()
   {
      AsteroidArea2D.AreaEntered += OnAreaEntered;
   }

   private void OnAreaEntered(Area2D area)
   {
      GD.Print($"Area entered: {area.Name}");
      if (area is IDamageable damageable)
      {
         damageable.TakeDamage(50);
      }
   }

   public override void _Process(double delta)
   {

   }

   public override void _PhysicsProcess(double delta)
   {
      AsteroidArea2D.Rotate(RotationSpeedRadians * (float)delta);
      AsteroidArea2D.Position += Velocity * (float)delta;
   }
}
