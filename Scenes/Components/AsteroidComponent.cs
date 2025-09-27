using Asteroids;
using Godot;
using System;

public partial class AsteroidComponent : Node
{
   [Export]
   public Node2D AsteroidNode2D { get; set; }
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
      if (area is HurtboxComponent hurtboxComponent)
      {
         hurtboxComponent.TryKill(this.GetParentGroups());
      }
   }

   public override void _Process(double delta)
   {

   }

   public override void _PhysicsProcess(double delta)
   {
      AsteroidNode2D.Rotate(RotationSpeedRadians * (float)delta);
      AsteroidNode2D.Position += Velocity * (float)delta;
   }
}
