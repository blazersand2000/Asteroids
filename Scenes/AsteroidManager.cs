using Asteroids;
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
   public PackedScene AsteroidBaseScene { get; set; }
   [Export]
   public PackedScene[] SpecificAsteroidScenes { get; set; }
   [Export]
   public Rect2 InitialNoSpawnZone { get; set; }

   private const int OutOfBoundsBuffer = 50;
   private const int SpawnBuffer = 30;

   private Timer _spawnTimer;

   public override void _Ready()
   {
      _spawnTimer = GetNode<Timer>("SpawnTimer");
      _spawnTimer.Timeout += OnSpawnTimerTimeout;

      SpawnInitialAsteriods();
   }

   public override void _Process(double delta)
   {

   }

   private void SpawnInitialAsteriods()
   {
      for (int i = 0; i < NumberOfAsteroids; i++)
      {
         SpawnRandomAsteroidInMainArea();
      }
   }

   private void OnSpawnTimerTimeout()
   {
      var currentAsteroids = GetChildren().OfType<Node2D>().Where(IsAsteroid).GroupBy(a => InBounds(a.Position));
      var outOfBoundsAsteroids = currentAsteroids.FirstOrDefault(g => g.Key == false)?.ToList() ?? new();
      var inBoundsAsteroids = currentAsteroids.FirstOrDefault(g => g.Key == true)?.ToList() ?? new();

      foreach (var outOfBoundsAsteroid in outOfBoundsAsteroids)
      {
         outOfBoundsAsteroid.QueueFree();
      }

      var asteroidsToSpawn = NumberOfAsteroids - inBoundsAsteroids.Count;

      for (int i = 0; i < asteroidsToSpawn; i++)
      {
         SpawnRandomAsteroidAlongScreenPerimiter();
      }
   }

   private bool InBounds(Vector2 position)
   {
      return GetViewportRect().Abs().Grow(OutOfBoundsBuffer).HasPoint(position);
   }

   private Vector2 GetRandomPerimiterSpawnLocation()
   {
      var outer = GetViewportRect().Grow(OutOfBoundsBuffer);
      var inner = GetViewportRect().Grow(SpawnBuffer);

      return GetRandomPointBetweenRects(outer, inner);
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

      // GD.Print($"top: {top}");
      // GD.Print($"bottom: {bottom}");
      // GD.Print($"left: {left}");
      // GD.Print($"right: {right}");

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

   private void SpawnRandomAsteroidAlongScreenPerimiter()
   {
      var position = GetRandomPerimiterSpawnLocation();
      SpawnRandomAsteroid(position);
   }

   private void SpawnRandomAsteroidInMainArea()
   {
      var position = GetRandomMainAreaSpawnLocation();
      SpawnRandomAsteroid(position);
   }

   private void SpawnRandomAsteroid(Vector2 position)
   {
      var asteroid = AsteroidBaseScene.Instantiate<AsteroidBase>();
      asteroid.CallDeferred(AsteroidBase.MethodName.SetVisuals, SpecificAsteroidScenes[GD.Randi() % SpecificAsteroidScenes.Length]);
      asteroid.AddToGroup(Groups.Enemy.ToString());

      if (asteroid.TryGetComponent<AsteroidComponent>(out var asteroidComponent))
      {
         var speed = GD.RandRange(100d, 500d);
         var direction = GD.Randf() * Mathf.Tau;
         asteroidComponent.Velocity = Vector2.Up.Rotated(direction) * (float)speed;
         asteroidComponent.RotationSpeedRadians = (float)GD.RandRange(-2d, 2d);
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
      GD.Print("Asteroid died@!");
      asteroid.QueueFree();
   }

   private static bool IsAsteroid(Node thing)
   {
      return thing.TryGetComponent<AsteroidComponent>(out _);
   }
}
