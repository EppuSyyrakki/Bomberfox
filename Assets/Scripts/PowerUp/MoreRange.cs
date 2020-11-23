using Bomberfox.Player;

namespace Bomberfox.PowerUp
{
	public class MoreRange : PowerUpBase
	{
		public override Bomb.Type Type { get; } = Bomb.Type.Normal;

		public override bool AddToPlayer(PlayerController pc)
		{
			pc.AddMoreRange();
			return true;
		}
	}
}
