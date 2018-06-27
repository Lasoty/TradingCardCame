using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingCardGame.Strategies;

namespace TradingCardGame
{
	public class Player : IPlayer
	{
		private const int StartingHandSize = 3;
		private const int MaximumHandSize = 5;
		private const int MaximumHealth = 30;
		private const int MaximumManaSlots = 10;
		private readonly Strategy strategy;
		public int Health { get; private set; } = MaximumHealth;
		public int ManaSlots { get; set; } = 0;
		public int Mana { get; private set; } = 0;

		private Random rand = new Random();

		private IList<Card> deck = new List<Card>
		{
			new Card(0),
			new Card(0),
			new Card(1),
			new Card(1),
			new Card(2),
			new Card(2),
			new Card(2),
			new Card(3),
			new Card(3),
			new Card(3),
			new Card(3),
			new Card(4),
			new Card(4),
			new Card(4),
			new Card(5),
			new Card(5),
			new Card(6),
			new Card(6),
			new Card(7),
			new Card(8),
		};

		private IList<Card> hand = new List<Card>();

		public string Name { get; set; }

		public Player(string name, Strategy strategy)
		{
			Name = name;
			this.strategy = strategy;
		}

		/// <summary>
		/// Tylko do celów testów jednostkowych
		/// </summary>
		/// <param name="name"></param>
		/// <param name="strategy"></param>
		/// <param name="deck"></param>
		/// <param name="hand"></param>
		/// <param name="health"></param>
		/// <param name="mana"></param>
		/// <param name="manaSlots"></param>
		public Player(string name, Strategy strategy, IList<Card> deck, IList<Card> hand, int health = MaximumHealth, int mana = 0, int manaSlots = 0) : this(name, strategy)
		{
			Name = name;
			this.strategy = strategy;
			this.deck = deck;
			this.hand = hand;
			Health = health;
			Mana = mana;
			ManaSlots = manaSlots;
		}

		public int GetNumberOfDeckCardsWithManaCost(int manaCost)
		{
			return deck.Count(card => card.Value.Equals(manaCost));
		}

		public int GetNumberOfDeckCards()
		{
			return deck.Count;
		}

		public int GetNumberOfHandCardsWithManaCost(int manaCost)
		{
			return hand.Count(card => card.Value.Equals(manaCost));
		}

		public int GetNumberOfHandCards()
		{
			return hand.Count;
		}

		public void DrawCard()
		{
			if (GetNumberOfDeckCards() == 0)
			{
				Console.WriteLine($"{this} bleeds out!");
				Health--;
			}
			else
			{
				Card card = deck[rand.Next(deck.Count)];
				deck.Remove(card);
				Console.WriteLine($"{this} draws card: {card}");
				if (GetNumberOfHandCards() < MaximumHandSize)
				{
					hand.Add(card);
				}
				else
				{
					Console.WriteLine($"{this} drops card {card} from overload!");
				}
			}
		}

		public void GiveManaSlot()
		{
			if (ManaSlots < MaximumManaSlots)
			{
				ManaSlots++;
			}
		}
		public void RefillMana()
		{
			Mana = ManaSlots;
		}

		public void DrawStartingHand()
		{
			for (int i = 0; i < StartingHandSize; i++)
			{
				DrawCard();
			}
		}

		private void Heal(int amount)
		{
			Health = Math.Min(Health + amount, MaximumHealth);
		}

		private void ReceiveDamage(int damage)
		{
			Health -= damage;
		}

		public bool CanPlayCards()
		{
			return hand.Count(card => card.Value <= Mana) > 0;
		}

		public void PlayCard(Player opponent)
		{
			Move move = strategy.NextMove(Mana, Health, hand);
			Card card = move.Card;
			if (card != null)
			{
				PlayCard(card, opponent, move.Action);
			}
			else
			{
				//IllegalMoveException
				throw new Exception("No card can be played from hand " + hand + " with (" + Mana + ") mana.");
			}
		}

		private void PlayCard(Card card, Player opponent, ActionType? action)
		{
			if (Mana < card.Value)
			{
				throw new Exception($"Insufficient Mana ({Mana}) to pay for card {card}.");
			}
			Console.WriteLine($"{this} plays card {card} for {action}");
			Mana -= card.Value;
			hand.Remove(card);
			switch (action)
			{
				case ActionType.DAMAGE:
					opponent.ReceiveDamage(card.Value);
					break;
				case ActionType.HEALING:
					Heal(card.Value);
					break;
				default:
					throw new Exception($"Unrecognized game action: {action}");
			}
		}

		#region Overrides of Object

		public override string ToString()
		{
			return $"Player:{Name}{{health={Health}, mana={Mana}/{ManaSlots}, hand={ShowCards(hand)}, deck={ShowCards(deck)}{'}'}";
		}

		private string ShowCards(IEnumerable<Card> cards)
		{
			StringBuilder result = new StringBuilder();
			foreach (Card card in cards)
			{
				result.Append(card + ",");
			}

			return result.ToString();
		}

		#endregion
	}
}
