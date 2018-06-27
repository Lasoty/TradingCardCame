using System;

namespace TradingCardGame
{
	public class Game : IGame
	{
		private Player activePlayer;
		private Player opponentPlayer;

		public Game(Player player1, Player player2) : this(player1, player2, new StartingPlayerChooser())
		{

		}

		private Game(Player player1, Player player2, StartingPlayerChooser startingPlayerChooser)
		{
			activePlayer = startingPlayerChooser.ChooseBetween(player1, player2);
			opponentPlayer = activePlayer == player1 ? player2 : player1;
			activePlayer.DrawStartingHand();
			opponentPlayer.DrawStartingHand();
			opponentPlayer.DrawCard(); // extra card to reduce disadvantage from being second to play
		}

		public void BeginTurn()
		{
			activePlayer.GiveManaSlot();
			activePlayer.RefillMana();
			activePlayer.DrawCard();
			Console.WriteLine($"{activePlayer} plays turn...");
		}

		private void SwitchPlayer()
		{
			Player previouslyActivePlayer = activePlayer;
			activePlayer = opponentPlayer;
			opponentPlayer = previouslyActivePlayer;
		}

		public void EndTurn()
		{
			Console.WriteLine($"{activePlayer} ends turn.");
			SwitchPlayer();
		}

		public Player GetWinner()
		{
			if (activePlayer.Health < 1)
			{
				return opponentPlayer;
			}

			return opponentPlayer.Health < 1 ? activePlayer : null;
		}

		internal void Run()
		{
			while (true)
			{
				if (GetWinner() == null)
				{
					BeginTurn();
					while (activePlayer.CanPlayCards())
					{
						activePlayer.PlayCard(opponentPlayer);
					}
					EndTurn();
				}
				else
				{
					Console.WriteLine($"{GetWinner()} wins the game!");
					break;
				}
			}
		}
	}
}
