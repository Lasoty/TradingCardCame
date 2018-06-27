using System.Collections.Generic;
using System.Linq;

namespace TradingCardGame.Strategies
{
	public class AiStrategy : Strategy
	{
		public override Move NextMove(int availableMana, int currentHealth, IEnumerable<Card> availableCards)
		{
			if (currentHealth < 10)
			{
				return new Move(HighestCard(availableMana, availableCards), ActionType.HEALING);
			}
			else
			{
				return new Move(BestCard(availableMana, availableCards), ActionType.DAMAGE);
			}
		}

		private Card BestCard(int availableMana, IEnumerable<Card> availableCards)
		{
			IList<IList<Card>> cardCombos = new List<IList<Card>>();
			IList<Card> remainingCards = new List<Card>(availableCards);
			remainingCards = remainingCards.OrderByDescending(card => card.Value).ToList(); // highest mana costs first
			while (!(remainingCards.Count < 1))
			{
				IList<Card> selectedCards = new List<Card>();
				CollectMaxDamageCardCombo(selectedCards, availableMana, remainingCards);
				cardCombos.Add(selectedCards);
				remainingCards.RemoveAt(0);
			}

			IList<Card> bestCombo = new List<Card>();
			int maxDamage = 0;
			foreach (IList<Card> combo in cardCombos)
			{
				int comboDamage = combo.Sum(card => card.Value);
				if (comboDamage > maxDamage || comboDamage == maxDamage && combo.Count > bestCombo.Count)
				{
					maxDamage = comboDamage;
					bestCombo = combo;
				}
			}

			return bestCombo.Max();
		}

		private void CollectMaxDamageCardCombo(ICollection<Card> selectedCards, int availableMana,
																					IList<Card> availableCards)
		{
			foreach (Card card in availableCards)
			{
				IList<Card> remainingCards = new List<Card>(availableCards);
				if (selectedCards.Sum(c => c.Value) + card.Value <= availableMana)
				{
					selectedCards.Add(card);
					remainingCards.Remove(card);
					CollectMaxDamageCardCombo(selectedCards, availableMana - card.Value, remainingCards);
				}
			}
		}


	}
}