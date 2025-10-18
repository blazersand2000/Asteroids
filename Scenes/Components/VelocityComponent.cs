using Godot;
using System;

public partial class VelocityComponent : Node
{
   [Export]
   public Vector2 Velocity { get; set; } = Vector2.Zero;

   public override void _PhysicsProcess(double delta)
   {
      if (Velocity == Vector2.Zero)
      {
         return;
      }

      var parent = GetParent<Node2D>();

      parent.Position += Velocity * (float)delta;
   }
}
