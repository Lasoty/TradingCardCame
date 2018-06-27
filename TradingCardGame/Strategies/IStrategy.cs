using System.Collections.Generic;

namespace TradingCardGame.Strategies
{
	public interface IStrategy
	{
		Move NextMove(int availableMana, int currentHealth, IEnumerable<Card> availableCards);
	}
}