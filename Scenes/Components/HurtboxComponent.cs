using Asteroids;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class HurtboxComponent : Area2D
{
   [Signal]
   public delegate void HitEventHandler(Vector2 position);
   [Export]
   public HealthComponent HealthComponent { get; set; }
   // [Export]
   // public Godot.Collections.Array<Groups> DamageableBy { get; set; }

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      //AreaEntered += (_) => GD.Print("Hurtbox entered!");
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }

   /// <summary>
   /// Triggers the hurtbox's kill logic, killing the associated HealthComponent.
   /// </summary>
   /// <param name="killPoint">The position where the hit occurred.</param>
   public void Kill(Vector2 killPoint)
   {
      EmitSignal(nameof(Hit), killPoint);
      HealthComponent?.Kill();
   }
}
