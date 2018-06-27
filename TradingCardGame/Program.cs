using TradingCardGame.Strategies;

namespace TradingCardGame
{
	class Program
	{
		static void Main(string[] args)
		{
			new Game(new Player("Human", new ConsoleInputStrategy()), new Player("CPU", new AiStrategy())).Run();
		}
	}
}
