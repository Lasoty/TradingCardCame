using System;

namespace TradingCardGame
{
	public class Card : IComparable<Card>, ICard
	{
		public int Value { get; }

		public Card(int value)
		{
			Value = value;
		}

		#region Overrides of Object

		public override string ToString()
		{
			return Value.ToString();
		}

		public override bool Equals(object obj)
		{
			if (this == obj) return true;

			if (obj == null || obj.GetType().Name != GetType().Name)
				return false;

			if (Value != ((Card) obj).Value) return false;

			return true;
		}

		#region Equality members

		public override int GetHashCode()
		{
			return Value;
		}

		#endregion

		#endregion

		public int CompareTo(Card other)
		{
			return Value - other.Value;
		}
	}
}
