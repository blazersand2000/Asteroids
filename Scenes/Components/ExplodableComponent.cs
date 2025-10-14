using Asteroids;
using Godot;
using System;

public partial class ExplodableComponent : Node2D
{
   [Signal]
   public delegate void ExplosionRequestedEventHandler(Vector2 position, Vector2 velocity);
   [Export]
   public HurtboxComponent HurtboxComponent { get; set; }
   [Export]
   public MovementTrackerComponent MovementTrackerComponent { get; set; }
   [Export]
   public PackedScene ExplosionScene { get; set; }

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      AddToGroup(Groups.Explodable.ToString());
      HurtboxComponent.Hit += OnHurtboxHit;
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }

   private void OnHurtboxHit(Vector2 hitPoint)
   {

      EmitSignal(nameof(SignalName.ExplosionRequested), GlobalPosition, MovementTrackerComponent.GetVelocity());
      var parent = GetParent<Node2D>();
      // Not sure how best to do this, but for now adding child to grandparent since presumably parent will despawn since it's exploding
      var explosionOwner = parent.GetParent<Node2D>();
      var explosion = ExplosionScene.Instantiate<Node2D>();
      // TODO: Give it a velocity same as the parent
      explosion.GlobalPosition = GlobalPosition;
      explosionOwner.AddChild(explosion);
   }
}
