namespace TradingCardGame
{
	public interface IPlayer
	{
		int Health { get; }
		int ManaSlots { get; set; }
		string Name { get; set; }
		int Mana { get; }

		bool CanPlayCards();
		void DrawCard();
		void DrawStartingHand();
		int GetNumberOfDeckCards();
		int GetNumberOfDeckCardsWithManaCost(int manaCost);
		int GetNumberOfHandCards();
		int GetNumberOfHandCardsWithManaCost(int manaCost);
		void GiveManaSlot();
		void PlayCard(Player opponent);
		void RefillMana();
		string ToString();
	}
}