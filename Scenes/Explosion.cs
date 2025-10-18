using Godot;
using System;

public partial class Explosion : Node2D
{
   public VelocityComponent VelocityComponent { get; set; }
   private GpuParticles2D _particle;
   private Timer _despawnTimer;

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      VelocityComponent = GetNode<VelocityComponent>("VelocityComponent");
      _particle = GetNode<GpuParticles2D>("GPUParticles2D");
      _despawnTimer = GetNode<Timer>("DespawnTimer");

      _particle.Emitting = true;
      _despawnTimer.Timeout += () => QueueFree();

   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }
}
