using Godot;
using System;

[Tool]
public partial class Polygon2d : Polygon2D
{
   [Export]
   public ShipPolygonResource ShipPolygonResource;

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      if (ShipPolygonResource != null)
      {
         Polygon = ShipPolygonResource.Points;
      }
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }
}
