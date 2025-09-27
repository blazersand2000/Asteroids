using Godot;
using System;
using System.Linq;

public partial class AsteroidBase : Node2D
{
   private AsteroidComponent _asteroidComponent;
   private Sprite2D _asteroidSprite;
   private CollisionShape2D _collisionShape2D;

   public override void _Ready()
   {
      _asteroidComponent = GetNode<AsteroidComponent>("AsteroidComponent");
      _asteroidSprite = GetNode<Sprite2D>("AsteroidSprite");
      _collisionShape2D = GetNode<CollisionShape2D>("HurtboxComponent/AsteroidCollisionShape");
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }

   public void SetVisuals(PackedScene visualScene)
   {
      var visuals = visualScene.Instantiate<Node2D>();
      var sprite = visuals.GetChildren().OfType<Sprite2D>().First();
      var collisionShape = visuals.GetChildren().OfType<CollisionShape2D>().First();

      _asteroidSprite.Texture = sprite.Texture;
      _collisionShape2D.Shape = collisionShape.Shape;

      visuals.QueueFree();
   }
}
