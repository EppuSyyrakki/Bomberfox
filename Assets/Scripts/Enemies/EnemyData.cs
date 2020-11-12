namespace Bomberfox.Enemies
{
	public class EnemyData
	{
		public float Speed { get; set; }
		public int SpecialChance { get; set; }
		public float SpecialCoolDown { get; set; }

		public EnemyData(float speed, int specialChance, float specialCoolDown)
		{
			Speed = speed;
			SpecialChance = specialChance;
			SpecialCoolDown = specialCoolDown;
		}
	}
}
