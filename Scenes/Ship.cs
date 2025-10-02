using Godot;
using System;
using System.Text.RegularExpressions;
using Asteroids;

public partial class Ship : Node2D
{
   [Export]
   public float ThrustPower = 1000f;
   [Export]
   public float RotationSpeed = 5f;
   [Export]
   public float Friction = 0.1f;
   [Export]
   public float MaxSpeed = 500f;
   [Export]
   public PackedScene LaserScene;
   [Export]
   public Node2D LaserParent;
   [Export]
   public HealthComponent HealthComponent;
   private Marker2D laserCannon;
   private Timer shootCooldownTimer;
   private AudioStreamPlayer audioStreamPlayer;
   private AnimatedSprite2D animatedSprite2D;
   private Vector2 _velocity = Vector2.Zero;
   private bool canShoot = true;

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      laserCannon = GetNode<Marker2D>("LaserCannon");
      shootCooldownTimer = GetNode<Timer>("ShootCooldownTimer");
      audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
      animatedSprite2D = GetNode<AnimatedSprite2D>("EngineBlastSprite");

      shootCooldownTimer.Timeout += OnShootCooldownTimeout;

      HealthComponent.Died += OnDied;

      AddToGroup(Groups.Player.ToString());
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }

   public override void _PhysicsProcess(double delta)
   {
      HandleRotation(delta);
      HandleThrust(delta);
      HandleShoot();
      HandleFriction(delta);
      LimitVelocity();

      Position += _velocity * (float)delta;

      //GD.Print($"Position: {Position}, Speed: {_velocity.Length()}");
   }

   private void HandleRotation(double delta)
   {
      if (Input.IsActionPressed("turn_left") && Input.IsActionPressed("turn_right"))
      {
         return;
      }

      if (Input.IsActionPressed("turn_left"))
      {
         Rotation -= RotationSpeed * (float)delta;
      }
      else if (Input.IsActionPressed("turn_right"))
      {
         Rotation += RotationSpeed * (float)delta;
      }
   }

   private void HandleThrust(double delta)
   {
      if (Input.IsActionPressed("thrust"))
      {
         var direction = Vector2.Up.Rotated(Rotation);
         _velocity += direction * ThrustPower * (float)delta;

         animatedSprite2D.Visible = true;
         animatedSprite2D.Play("default");
      }
      else
      {
         animatedSprite2D.Visible = false;
         animatedSprite2D.Stop();
      }
   }

   private void HandleShoot()
   {
      if (Input.IsActionPressed("shoot"))
      {
         if (canShoot)
         {
            Shoot();
            canShoot = false;
            shootCooldownTimer.Start();
         }
      }
   }

   private void Shoot()
   {
      var laser = LaserScene.Instantiate<Node2D>();
      laser.GlobalPosition = laserCannon.GlobalPosition;
      laser.GlobalRotation = laserCannon.GlobalRotation;
      LaserParent.AddChild(laser);
      audioStreamPlayer.Play(0.26f);
   }

   private void OnShootCooldownTimeout()
   {
      canShoot = true;
   }

   private void HandleFriction(double delta)
   {
      var velocityDirection = _velocity.Normalized();
      var oppositeDirection = -velocityDirection;
      _velocity += oppositeDirection * Friction * (float)delta;
   }

   private void LimitVelocity()
   {
      if (_velocity.Length() > MaxSpeed)
      {
         _velocity = _velocity.Normalized() * MaxSpeed;
      }
   }

   private void OnDied()
   {
      GD.Print("Ship died!");
   }
}
