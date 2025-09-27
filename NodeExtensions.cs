using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
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

   public static HashSet<string> GetParentGroups(this Node node)
   {
      return node.GetParent().GetGroups().Select(g => g.ToString()).ToHashSet();
   }
}
