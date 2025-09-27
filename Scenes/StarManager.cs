using Godot;
using System;

public partial class StarManager : Node2D
{
   [Export]
   public PackedScene StarScene { get; set; }
   [Export]
   public int StarDensity { get; set; }

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      GenerateStars();
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }

   private void GenerateStars()
   {
      var viewportRect = GetViewportRect().Abs();

      var n = StarDensity * (int)viewportRect.Area / 10000;

      for (int i = 0; i < n; i++)
      {
         var star = StarScene.Instantiate<Star>();
         AddChild(star);
         star.Position = new Vector2((float)GD.RandRange(0, viewportRect.Size.X), (float)GD.RandRange(0, viewportRect.Size.Y));
         var size = (float)GD.RandRange(0.8f, 1.2f);
         var brightness = (float)GD.RandRange(0f, 1f);
         star.SetProminence(size, brightness);
      }
   }
}
