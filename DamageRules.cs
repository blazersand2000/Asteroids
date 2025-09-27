using System;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids;

public static class DamageRules
{
   public static bool CanDamage(IEnumerable<string> attackerGroups, IEnumerable<string> targetGroups)
   {
      return attackerGroups.Any(g => CanDamage(g, targetGroups));
   }

   private static bool CanDamage(string attackerGroup, IEnumerable<string> targetGroups)
   {
      var attacker = ToGroupEnum(attackerGroup);
      var target = ToGroupEnumSet(targetGroups);

      return attacker switch
      {
         Groups.Enemy when target.Contains(Groups.Player) => true,
         Groups.PlayerProjectile when target.Contains(Groups.Enemy) => true,
         _ => false
      };
   }

   private static Groups ToGroupEnum(string group)
   {
      return (Groups)Enum.Parse(typeof(Groups), group);
   }

   private static HashSet<Groups> ToGroupEnumSet(IEnumerable<string> groups)
   {
      return groups.Select(ToGroupEnum).ToHashSet();
   }
}
