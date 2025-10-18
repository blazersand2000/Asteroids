using Asteroids;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public partial class AsteroidManager : Node2D
{
   [Export]
   public int NumberOfAsteroids { get; set; }
   [Export]
   public PackedScene AsteroidBaseScene { get; set; }
   [Export]
   public PackedScene[] LargeAsteroidScenes { get; set; }
   [Export]
   public PackedScene[] MediumAsteroidScenes { get; set; }
   [Export]
   public PackedScene[] SmallAsteroidScenes { get; set; }
   [Export]
   public int NumMedium { get; set; }
   [Export]
   public int NumSmall { get; set; }
   [Export]
   public Rect2 InitialNoSpawnZone { get; set; }

   private const int SpawnBuffer = 30;

   public override void _Ready()
   {
      SpawnInitialAsteriods();
   }

   public override void _Process(double delta)
   {

   }

   private void SpawnInitialAsteriods()
   {
      for (int i = 0; i < NumberOfAsteroids; i++)
      {
         var position = GetRandomMainAreaSpawnLocation();
         var visuals = LargeAsteroidScenes[GD.Randi() % LargeAsteroidScenes.Length];
         var speed = GD.RandRange(100d, 300d);
         var direction = GD.Randf() * Mathf.Tau;
         var velocity = Vector2.Up.Rotated(direction) * (float)speed;
         var rotationSpeedRadians = (float)GD.RandRange(-2d, 2d);

         SpawnAsteroid(position, visuals, AsteroidSize.Large, velocity, rotationSpeedRadians);
      }
   }

   private Vector2 GetRandomMainAreaSpawnLocation()
   {
      var outer = GetViewportRect().Grow(SpawnBuffer);
      var inner = InitialNoSpawnZone;

      return GetRandomPointBetweenRects(outer, inner);
   }

   private Vector2 GetRandomPointBetweenRects(Rect2 outer, Rect2 inner)
   {
      if (!outer.Encloses(inner))
      {
         throw new InvalidOperationException("Outer must enclose inner");
      }

      // top and bottom go full width
      var top = new Rect2(outer.Position, new Vector2(outer.Size.X, inner.Position.Y - outer.Position.Y));
      var bottom = new Rect2(new Vector2(outer.Position.X, inner.End.Y), new Vector2(outer.Size.X, outer.End.Y - inner.End.Y));
      //left and right don't go full height since top and bottom took care of that
      var left = new Rect2(new Vector2(outer.Position.X, inner.Position.Y), new Vector2(inner.Position.X - outer.Position.X, inner.Size.Y));
      var right = new Rect2(new Vector2(inner.End.X, inner.Position.Y), new Vector2(outer.End.X - inner.End.X, inner.End.Y - inner.Position.Y));

      var weightedRects = new List<Rect2> { top, bottom, left, right }.Select(r => (r, r.Area));
      var rect = GetWeightedRandom(weightedRects);
      var randX = (float)GD.RandRange(rect.Position.X, rect.End.X);
      var randY = (float)GD.RandRange(rect.Position.Y, rect.End.Y);

      return new Vector2(randX, randY);
   }

   private T GetWeightedRandom<T>(IEnumerable<(T Item, float Weight)> items)
   {
      var threshold = GD.Randf() * items.Sum(i => i.Weight);
      var cumulative = 0f;
      foreach (var (Item, Weight) in items)
      {
         cumulative += Weight;
         if (cumulative > threshold)
         {
            return Item;
         }
      }
      return items.Last().Item;
   }

   private void SpawnRandomAsteroidInMainArea()
   {
      var position = GetRandomMainAreaSpawnLocation();
      var visuals = LargeAsteroidScenes[GD.Randi() % LargeAsteroidScenes.Length];
      var speed = GD.RandRange(100d, 300d);
      var direction = GD.Randf() * Mathf.Tau;
      var velocity = Vector2.Up.Rotated(direction) * (float)speed;
      var rotationSpeedRadians = (float)GD.RandRange(-2d, 2d);

      SpawnAsteroid(position, visuals, AsteroidSize.Large, velocity, rotationSpeedRadians);
   }

   private void SpawnAsteroid(Vector2 position, PackedScene visuals, AsteroidSize size, Vector2 velocity, float rotationSpeedRadians)
   {
      var asteroid = AsteroidBaseScene.Instantiate<AsteroidBase>();
      asteroid.CallDeferred(AsteroidBase.MethodName.SetVisuals, visuals);
      asteroid.AddToGroup(Groups.Enemy.ToString());

      if (asteroid.TryGetComponent<AsteroidComponent>(out var asteroidComponent))
      {
         asteroidComponent.Size = size;
         asteroidComponent.RotationSpeedRadians = rotationSpeedRadians;
      }

      if (asteroid.TryGetComponent<VelocityComponent>(out var velocityComponent))
      {
         velocityComponent.Velocity = velocity;
      }

      if (asteroid.TryGetComponent<HealthComponent>(out var healthComponent))
      {
         healthComponent.Died += () => OnAsteroidDestroyed(asteroid);
      }

      asteroid.Position = position;

      AddChild(asteroid);
   }

   private void OnAsteroidDestroyed(Node2D asteroid)
   {
      asteroid.QueueFree();

      if (!asteroid.TryGetComponent<AsteroidComponent>(out var asteroidComponent))
      {
         return;
      }

      if (!asteroid.TryGetComponent<VelocityComponent>(out var velocityComponent))
      {
         return;
      }

      if (asteroidComponent.Size == AsteroidSize.Small)
      {
         return;
      }

      var nextSize = asteroidComponent.Size == AsteroidSize.Medium ? AsteroidSize.Small : AsteroidSize.Medium;
      var numberToSpawn = nextSize == AsteroidSize.Medium ? NumMedium : NumSmall;
      for (int i = 0; i < numberToSpawn; i++)
      {
         var visuals = GetRandomAsteroidVisuals(nextSize);
         var rotationChange = (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
         var velocityChange = (float)GD.RandRange(1, 1.5);
         var velocity = velocityComponent.Velocity.Rotated(rotationChange) * velocityChange;
         var rotationSpeedRadians = (float)GD.RandRange(-2d, 2d);

         SpawnAsteroid(asteroid.Position, visuals, nextSize, velocity, rotationSpeedRadians);
      }
   }

   private PackedScene GetRandomAsteroidVisuals(AsteroidSize size)
   {
      var randi = GD.Randi();
      PackedScene[] scenes = size switch
      {
         AsteroidSize.Large => LargeAsteroidScenes,
         AsteroidSize.Medium => MediumAsteroidScenes,
         AsteroidSize.Small => SmallAsteroidScenes,
         _ => LargeAsteroidScenes,
      };
      return scenes[randi % scenes.Length];
   }
}
