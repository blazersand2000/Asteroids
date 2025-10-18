using Asteroids;
using Godot;
using System;

public partial class ExplodableComponent : Node2D
{
   [Export]
   public HurtboxComponent HurtboxComponent { get; set; }
   [Export]
   public VelocityComponent VelocityComponent { get; set; }

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
      var globalBus = GetNode<GlobalBus>("/root/GlobalBus");
      globalBus.RequestExplosion(GlobalPosition, VelocityComponent.Velocity);
   }
}
