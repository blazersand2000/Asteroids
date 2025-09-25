using Godot;
using System;

public partial class AsteroidBase : Node2D
{
   private Node _visualsContainer;
   private AsteroidComponent _asteroidComponent;

   public override void _Ready()
   {
      _visualsContainer = GetNode("VisualsContainer");
      _asteroidComponent = GetNode<AsteroidComponent>("AsteroidComponent");
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }

   public void SetVisualScene(PackedScene visualScene)
   {
      foreach (var child in _visualsContainer.GetChildren())
      {
         child.QueueFree();
      }

      var visuals = visualScene.Instantiate<Area2D>();
      _visualsContainer.AddChild(visuals);
      _asteroidComponent.AsteroidArea2D = visuals;
   }
}
