using Godot;
using System;
using Asteroids.Interfaces;

public partial class Ship : Area2D, IDamageable
{
   [Export]
   public float ThrustPower = 1000f;
   [Export]
   public float RotationSpeed = 5f;
   [Export]
   public float Friction = 0.1f;
   [Export]
   public float MaxSpeed = 500f;
   private Vector2 _velocity = Vector2.Zero;
   private const int OutOfBoundsBuffer = 32;

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }

   public override void _PhysicsProcess(double delta)
   {
      HandleRotation(delta);
      HandleThrust(delta);
      HandleFriction(delta);
      LimitVelocity();

      Position += _velocity * (float)delta;

      HandleScreenWarp();

      //GD.Print($"Position: {Position}, Speed: {_velocity.Length()}");
   }

   public void TakeDamage(int amount)
   {
      GD.Print($"Took {amount} damage");
   }

   private void HandleRotation(double delta)
   {
      if (Input.IsActionPressed("turn_left") && Input.IsActionPressed("turn_right"))
      {
         return;
      }

      if (Input.IsActionPressed("turn_left"))
      {
         Rotation -= RotationSpeed * (float)delta;
      }
      else if (Input.IsActionPressed("turn_right"))
      {
         Rotation += RotationSpeed * (float)delta;
      }
   }

   private void HandleThrust(double delta)
   {
      if (Input.IsActionPressed("thrust"))
      {
         var direction = Vector2.Up.Rotated(Rotation);
         _velocity += direction * ThrustPower * (float)delta;
      }
   }

   private void HandleFriction(double delta)
   {
      var velocityDirection = _velocity.Normalized();
      var oppositeDirection = -velocityDirection;
      _velocity += oppositeDirection * Friction * (float)delta;
   }

   private void HandleScreenWarp()
   {
      var screenSize = GetViewportRect().Size;
      if (Position.X < -OutOfBoundsBuffer)
      {
         Position = new Vector2(screenSize.X + OutOfBoundsBuffer, Position.Y);
      }
      else if (Position.X > screenSize.X + OutOfBoundsBuffer)
      {
         Position = new Vector2(-OutOfBoundsBuffer, Position.Y);
      }

      if (Position.Y < -OutOfBoundsBuffer)
      {
         Position = new Vector2(Position.X, screenSize.Y + OutOfBoundsBuffer);
      }
      else if (Position.Y > screenSize.Y + OutOfBoundsBuffer)
      {
         Position = new Vector2(Position.X, -OutOfBoundsBuffer);
      }
   }

   private void LimitVelocity()
   {
      if (_velocity.Length() > MaxSpeed)
      {
         _velocity = _velocity.Normalized() * MaxSpeed;
      }
   }
}
