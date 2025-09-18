using Godot;
using System;
using System.Reflection.Metadata;

public partial class Laser : Area2D
{
   [Export]
   public float Speed = 50;
   private const float OutOfBoundsBuffer = 200f;

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
      if (ShouldDespawn())
      {
         QueueFree();
      }
   }

   public override void _PhysicsProcess(double delta)
   {
      var direction = Vector2.Up.Rotated(Rotation);
      Position += direction * Speed * (float)delta;
   }

   private bool ShouldDespawn()
   {
      var screenSize = GetViewportRect().Abs().Size;
      return GlobalPosition.X < -OutOfBoundsBuffer
          || GlobalPosition.X > screenSize.X + OutOfBoundsBuffer
          || GlobalPosition.Y < -OutOfBoundsBuffer
          || GlobalPosition.Y > screenSize.Y + OutOfBoundsBuffer;
   }
}
