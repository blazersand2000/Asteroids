using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class AsteroidManager : Node2D
{
   [Export]
   public int NumberOfAsteroids { get; set; }
   [Export]
   public PackedScene[] AsteroidScenes { get; set; }

   private Timer _spawnTimer;

   public override void _Ready()
   {
      _spawnTimer = GetNode<Timer>("SpawnTimer");
      _spawnTimer.Timeout += OnSpawnTimerTimeout;
   }

   public override void _Process(double delta)
   {

   }

   private void OnSpawnTimerTimeout()
   {
      var currentAsteroids = GetChildren().Where(IsAsteroid);
      var asteroidsToSpawn = NumberOfAsteroids - currentAsteroids.Count();

      for (int i = 0; i < asteroidsToSpawn; i++)
      {
         SpawnRandomAsteroid();
      }
   }

   private void SpawnRandomAsteroid()
   {
      var asteroid = AsteroidScenes[GD.Randi() % AsteroidScenes.Length].Instantiate<Node2D>();
      var asteroidComponent = asteroid.GetChildren().OfType<AsteroidComponent>().FirstOrDefault();
      if (asteroidComponent != null)
      {
         asteroidComponent.Velocity = new Vector2((float)GD.RandRange(1d, 5d), (float)GD.RandRange(1d, 5d));
         asteroidComponent.RotationSpeedRadians = (float)GD.RandRange(0.25d, 2d);
      }
      var screenSize = GetViewportRect().Abs().Size;
      asteroid.Position = new Vector2((float)GD.RandRange(0d, screenSize.X), (float)GD.RandRange(0, screenSize.Y));

      AddChild(asteroid);
   }

   private static bool IsAsteroid(Node thing)
   {
      return thing.GetChildren().OfType<AsteroidComponent>().Count() > 0;
   }
}
