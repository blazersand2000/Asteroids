using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Godot;

namespace Asteroids;

public static class NodeExtensions
{
   public static T? GetComponent<T>(this Node node) where T : Node
   {
      return node.GetChildren().OfType<T>().FirstOrDefault();
   }

   public static bool TryGetComponent<T>(this Node node, [MaybeNullWhen(false)] out T component) where T : Node
   {
      component = node.GetComponent<T>();
      return component != null;
   }
}
