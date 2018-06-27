namespace TradingCardGame
{
	public interface IGame
	{
		void BeginTurn();
		void EndTurn();
		Player GetWinner();
	}
}