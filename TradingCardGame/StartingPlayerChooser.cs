using System;

namespace TradingCardGame
{
	public class StartingPlayerChooser
	{

		private readonly Random random = new Random();

		public Player ChooseBetween(Player player1, Player player2)
		{
			return random.Next() % 2 > 0 ? player1 : player2;
		}
	}
}