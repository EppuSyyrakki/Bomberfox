﻿using Bomberfox.Player;

namespace Bomberfox.PowerUp
{
	public class BigBomb : PowerUpBase
	{
		public override Bomb.Type Type { get; } = Bomb.Type.Mega;

		public override bool AddToPlayer(PlayerController pc)
		{
			if (pc.megaBomb == null)
			{
				pc.ReceiveNewBomb(this);
				return true;
			}

			return false;
		}
	}
}
