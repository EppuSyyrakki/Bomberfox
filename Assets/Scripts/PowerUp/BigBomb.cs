using Bomberfox.Player;

namespace Bomberfox.PowerUp
{
	public class BigBomb : PowerUpBase
	{
		public override Bomb.Type Type { get; } = Bomb.Type.Special;

		public override void AddToPlayer(PlayerController pc)
		{
			pc.ReceiveNewBomb(this, Type);
		}
	}
}
