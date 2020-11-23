using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;

namespace Bomberfox.Player
{
	public class Health
	{
		private int health;
		private int healthMax;

		public Health(int healthMax, int startingHealth)
		{
			this.healthMax = healthMax;
			health = startingHealth;
		}

		public int GetHealth()
		{
			return health;
		}
		public int GetHealthMax()
		{
			return healthMax;
		}

		public void Damage(int damageAmount)
		{
			health -= damageAmount;
		}

		public void Heal(int healAmount)
		{
			health += healAmount;
			if (health > healthMax) health = healthMax;
		}
	}
}
