﻿using Bomberfox.Player;

namespace Bomberfox.PowerUp
{
	public class AddHealth : PowerUpBase
	{
		public override Bomb.Type Type { get; } = Bomb.Type.Normal;

		public override void AddToPlayer(PlayerController pc)
		{
			pc.AddHealth();
		}
	}
}