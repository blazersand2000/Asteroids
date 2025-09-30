using Godot;
using System;

public partial class ScreenWarpComponent : Node
{
   [Export]
   public int OutOfBoundsBuffer { get; set; }
   private Vector2 _screenSize;

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      _screenSize = GetViewport().GetVisibleRect().Abs().Size;
      GetViewport().SizeChanged += OnViewportSizeChanged;
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }

   public override void _PhysicsProcess(double delta)
   {
      HandleScreenWarp();
   }

   private void OnViewportSizeChanged()
   {
      _screenSize = GetViewport().GetVisibleRect().Abs().Size;
   }

   private void HandleScreenWarp()
   {
      var parent = GetParent<Node2D>();

      if (parent.Position.X < -OutOfBoundsBuffer)
      {
         parent.Position = new Vector2(_screenSize.X + OutOfBoundsBuffer, parent.Position.Y);
      }
      else if (parent.Position.X > _screenSize.X + OutOfBoundsBuffer)
      {
         parent.Position = new Vector2(-OutOfBoundsBuffer, parent.Position.Y);
      }

      if (parent.Position.Y < -OutOfBoundsBuffer)
      {
         parent.Position = new Vector2(parent.Position.X, _screenSize.Y + OutOfBoundsBuffer);
      }
      else if (parent.Position.Y > _screenSize.Y + OutOfBoundsBuffer)
      {
         parent.Position = new Vector2(parent.Position.X, -OutOfBoundsBuffer);
      }
   }
}
