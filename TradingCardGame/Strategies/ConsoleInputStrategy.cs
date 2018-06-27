using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TradingCardGame.Strategies
{
	public class ConsoleInputStrategy : Strategy
	{
		public override Move NextMove(int availableMana, int currentHealth, IEnumerable<Card> availableCards)
		{
			try
			{
				int card = -1;
				ActionType action = ActionType.DAMAGE;
				while (card < 0 || card > 8 || card > availableMana || !availableCards.Contains(new Card(card)))
				{
					try
					{
						string input = Console.ReadLine();
						if (input.EndsWith("h"))
						{
							action = ActionType.HEALING;
							input = input.Replace("h", "");
						}
						card = int.Parse(input);
					}
					catch (FormatException e)
					{
						Console.WriteLine("Invalid input: " + e.Message);
					}
				}
				return new Move(new Card(card), action);
			}
			catch (IOException e)
			{
				Console.WriteLine("Could not read console input: " + e.Message);
			}
			return new Move(new Card(0), null);
		}
	}
}