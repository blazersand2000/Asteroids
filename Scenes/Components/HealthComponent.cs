using Godot;
using System;

public partial class HealthComponent : Node
{
   [Signal]
   public delegate void DiedEventHandler();
   [Export]
   public int MaxHealth = 100;
   private int currentHealth;

   public override void _Ready()
   {
      currentHealth = MaxHealth;
   }

   /// <summary>
   /// Applies the amount of damage to drain health, killing the entity
   /// </summary>
   public void Kill()
   {
      TakeDamage(currentHealth);
   }

   public void TakeDamage(int amount)
   {
      currentHealth -= amount;

      if (currentHealth <= 0)
      {
         EmitSignal(SignalName.Died);
      }
   }
}
