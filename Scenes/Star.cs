using Godot;
using System;
using System.Linq;

public partial class Star : Node2D
{
   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }

   public void SetProminence(float size, float brightness)
   {
      Scale = new Vector2(size, size);
      Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, Mathf.Clamp(brightness, 0f, 1f));
   }
}
