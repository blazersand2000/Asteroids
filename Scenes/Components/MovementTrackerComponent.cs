using Godot;
using System;

public partial class MovementTrackerComponent : Node
{
   private Vector2 _position;
   private Vector2 _velocity;

   public override void _Ready()
   {
      _position = GetParent<Node2D>().Position;
      _velocity = Vector2.Zero;
   }

   public override void _PhysicsProcess(double delta)
   {
      var newPosition = GetParent<Node2D>().Position;

      if (newPosition == _position)
      {
         return;
      }

      _velocity = newPosition - _position;
      _position = newPosition;
   }

   public Vector2 GetVelocity() => _velocity;
}
