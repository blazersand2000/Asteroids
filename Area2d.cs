using Godot;
using System;

public partial class Area2d : Area2D
{
   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      AreaEntered += OnAreaEntered;
   }

   private void OnAreaEntered(Area2D area)
   {
      GD.Print($"Area entered: {area.Name}");
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }
}
