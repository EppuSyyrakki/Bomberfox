using Bomberfox.Player;

namespace Bomberfox.PowerUp
{
	public class AddHealth : PowerUpBase
	{
		public override Bomb.Type Type { get; } = Bomb.Type.Normal;

		public override bool AddToPlayer(PlayerController pc)
		{
			if (pc.healthSystem.GetHealth() < pc.healthSystem.GetHealthMax())
			{
				pc.AddHealth();
				return true;
			}

			return false;
		}
	}
}
