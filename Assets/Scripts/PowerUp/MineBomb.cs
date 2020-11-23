using Bomberfox.Player;

namespace Bomberfox.PowerUp
{
	public class MineBomb : PowerUpBase
	{
		public override Bomb.Type Type { get; } = Bomb.Type.Mine;

		public override bool AddToPlayer(PlayerController pc)
		{
			if (pc.mineBomb == null)
			{
				pc.ReceiveNewBomb(this);
				return true;
			}

			return false;
		}
	}
}
