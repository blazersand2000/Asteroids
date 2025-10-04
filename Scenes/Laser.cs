using Asteroids;
using Godot;
using System;
using System.Linq;
using System.Reflection.Metadata;

public partial class Laser : Area2D
{
   [Export]
   public float Speed = 50;
   private const float OutOfBoundsBuffer = 200f;
   private Sprite2D _sprite;

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      _sprite = GetNode<Sprite2D>("Sprite2D");

      AddToGroup(Groups.PlayerProjectile.ToString());
      ApplyInitialEffects();
      AreaEntered += OnAreaEntered;
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

   private void OnAreaEntered(Area2D area)
   {
      if (area is HurtboxComponent hurtboxComponent)
      {
         hurtboxComponent.Kill();
         QueueFree();
      }
   }

   private void ApplyInitialEffects()
   {
      var originalColor = _sprite.Modulate;
      _sprite.Modulate = new Color(4, 4, 4);
      var tween = GetTree().CreateTween();
      tween.TweenProperty(_sprite, "modulate", originalColor, 0.3f);
   }
}
