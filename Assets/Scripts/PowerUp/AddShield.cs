using Bomberfox.Player;

namespace Bomberfox.PowerUp
{
	public class AddShield : PowerUpBase
	{
		public override Bomb.Type Type { get; } = Bomb.Type.Normal;

		public override bool AddToPlayer(PlayerController pc)
		{
			if (!pc.hasShield)
			{
				pc.AddShield();
				return true;
			}

			return false;
		}
	}
}
