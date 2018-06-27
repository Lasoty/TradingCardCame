using System;

namespace TradingCardGame
{
	public class Move
	{
		public Card Card { get; }
		public ActionType? Action { get; }

		public Move(Card card, ActionType? action)
		{
			this.Card = card;
			this.Action = action;
		}

		internal object GetAction()
		{
			throw new NotImplementedException();
		}

		#region Overrides of Object

		public override bool Equals(object o)
		{
			if (this == o) return true;
			if (o == null || GetType().Name != o.GetType().Name) return false;

			Move move = (Move)o;

			if (Action != move.Action) return false;
			return !(!Card?.Equals(move.Card) ?? move.Card != null);
		}

		#region Equality members

		public override int GetHashCode()
		{
			unchecked
			{
				int result = Card != null ? Card.GetHashCode() : 0;
				result = 31 * result + (Action != null ? Action.GetHashCode() : 0);
				return result;
			}
		}

		#endregion

		public override string ToString()
		{
			return $"Move{{card={Card}, action={Action}{'}'}";
		}

		#endregion
	}
}