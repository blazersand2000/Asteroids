namespace Asteroids.Interfaces;

public interface IDamageable
{
   /// <summary>
   /// Applies damage to the object.
   /// </summary>
   /// <param name="amount">The amount of damage to apply.</param>
   void TakeDamage(int amount);
}