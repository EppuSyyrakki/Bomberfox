using Bomberfox.Player;

namespace Bomberfox.PowerUp
{
	public class RemoteBomb : PowerUpBase
	{
		public override Bomb.Type Type { get; } = Bomb.Type.Special;

		public override void AddToPlayer(PlayerController pc)
		{
			pc.ReceiveNewBomb(this, Type);
		}
	}
}
