using Asteroids;
using Godot;
using System;

public partial class AsteroidComponent : Node
{
   [Export]
   public Area2D AsteroidArea2D { get; set; }
   [Export]
   public AsteroidSize Size { get; set; } = AsteroidSize.Large;
   public float RotationSpeedRadians { get; set; }

   public override void _Ready()
   {
      AsteroidArea2D.AreaEntered += OnAreaEntered;
   }

   private void OnAreaEntered(Area2D other)
   {
      if (other is HurtboxComponent hurtboxComponent)
      {
         hurtboxComponent.Kill(other.Position);
      }
   }

   public override void _Process(double delta)
   {

   }

   public override void _PhysicsProcess(double delta)
   {
      var parent = GetParent<Node2D>();
      parent.Rotate(RotationSpeedRadians * (float)delta);
   }
}
