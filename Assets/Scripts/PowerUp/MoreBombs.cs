using Bomberfox.Player;

namespace Bomberfox.PowerUp
{
	public class MoreBombs : PowerUpBase
	{
		public override Bomb.Type Type { get; } = Bomb.Type.Normal;

		public override bool AddToPlayer(PlayerController pc)
		{
			pc.AddMaxBombs();
			return true;
		}
	}
}
