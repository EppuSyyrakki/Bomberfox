using Bomberfox.Player;

namespace Bomberfox.PowerUp
{
	public class RemoteBomb : PowerUpBase
	{
		public override Bomb.Type Type { get; } = Bomb.Type.Remote;

		public override bool AddToPlayer(PlayerController pc)
		{
			if (pc.remoteBomb == null)
			{
				pc.ReceiveNewBomb(this);
				return true;
			}

			return false;
		}
	}
}
