using System.Collections.Generic;
using System.Linq;

namespace TradingCardGame.Strategies
{
	public abstract class Strategy : IStrategy
	{

		public abstract Move NextMove(int availableMana, int currentHealth, IEnumerable<Card> availableCards);

		protected Card HighestCard(int availableMana, IEnumerable<Card> availableCards)
		{
			return availableCards.Where(card => card.Value <= availableMana).Max();
		}

		protected Card LowestCard(int availableMana, IEnumerable<Card> availableCards)
		{
			return availableCards.Where(card => card.Value <= availableMana).Min();
		}

	}
}