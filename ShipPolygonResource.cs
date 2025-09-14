using Godot;
using System;

[Tool]
[GlobalClass]
public partial class ShipPolygonResource : Resource
{
   [Export]
   public Vector2[] Points = new Vector2[0];
}
