namespace TradingCardGame
{
	public interface ICard
	{
		int Value { get; }

		int CompareTo(Card other);
		bool Equals(object obj);
		int GetHashCode();
		string ToString();
	}
}