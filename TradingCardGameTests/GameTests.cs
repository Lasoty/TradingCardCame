using NUnit.Framework;
using TradingCardGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

namespace TradingCardGame.Tests
{
	[TestFixture]
	public class GameTests
	{
		private Game game;
		[SetUp]
		public void SetUp()
		{
			Player player = new Player("TestPlayer", null);
			Player opponent = new Player("TestOpponent", null);
			game = new Game(player, opponent);
		}

		[Test]
		public void GameShouldHaveTwoPlayersTest()
		{
			PrivateObject gameO = new PrivateObject(game);
			Player activePlayer = gameO.GetField("activePlayer") as Player;
			Player opponentPlayer = gameO.GetField("opponentPlayer") as Player;

			Assert.IsNotNull(activePlayer);
			Assert.IsNotNull(opponentPlayer);
		}

		[Test]
		public void StartingPlayerShouldHaveStartingHandOfThreeCardsFromHisDeckTest()
		{
			PrivateObject gameO = new PrivateObject(game);
			Player activePlayer = gameO.GetField("activePlayer") as Player;

			Assert.IsNotNull(activePlayer);
			Assert.AreEqual(3, activePlayer.GetNumberOfHandCards());
			Assert.AreEqual(17, activePlayer.GetNumberOfDeckCards());
		}

		[Test]
		public void NonStartingPlayerShouldHaveStartingHandOfFourCardsFromHisDeckTest()
		{
			PrivateObject gameO = new PrivateObject(game);
			Player opponentPlayer = gameO.GetField("opponentPlayer") as Player;

			Assert.IsNotNull(opponentPlayer);
			Assert.AreEqual(4, opponentPlayer.GetNumberOfHandCards());
			Assert.AreEqual(16, opponentPlayer.GetNumberOfDeckCards());
		}

		[Test]
		public void ActivePlayerShouldSwitchOnEndOfTurnTest()
		{
			Player player = new Player("TestPlayer", null);
			Player opponent = new Player("TestOpponent", null);
			Game game = new Game(player, opponent);
			PrivateObject gameO = new PrivateObject(game);
			gameO.SetField("activePlayer", player);
			gameO.SetField("opponentPlayer", opponent);

			game.EndTurn();
			Player actual = gameO.GetField("activePlayer") as Player;
			Assert.AreEqual("TestOpponent", actual?.Name);

			game.EndTurn();
			actual = gameO.GetField("activePlayer") as Player;
			Assert.AreEqual("TestPlayer", actual?.Name);
		}

		[Test]
		public void ActivePlayerShouldReceiveOneManaSlotOnBeginningOfTurnTest()
		{
			game.BeginTurn();
			PrivateObject gameO = new PrivateObject(game);
			Player actual = gameO.GetField("activePlayer") as Player;
			Assert.AreEqual(1, actual.ManaSlots);
		}

		[Test]
		public void ActivePlayerShouldRefillManaOnBeginningOfTurnTest()
		{
			Player player1 = new Player("TestPlayer", null, new List<Card>(), new List<Card>(), manaSlots: 3);
			game = new Game(player1, new Player("TestOpp", null));
			PrivateObject gameO = new PrivateObject(game);
			gameO.SetField("activePlayer", player1);
			game.BeginTurn();

			Assert.AreEqual(player1.ManaSlots, player1.Mana);
		}

		[Test]
		public void ActivePlayerShouldDrawCardOnBeginningOfTurnTest()
		{
			Player player1 = new Player("TestPlayer", null);
			game = new Game(player1, new Player("TestOpp", null));
			PrivateObject gameO = new PrivateObject(game);
			gameO.SetField("activePlayer", player1);
			int numberOfInitialHandCards = player1.GetNumberOfHandCards();

			game.BeginTurn();

			Assert.AreEqual(numberOfInitialHandCards + 1, player1.GetNumberOfHandCards());
		}

		[Test]
		public void PlayerWithOneHealthAndEmptyDeckShouldDieFromBleedingOutOnBeginningOfTurnTest()
		{
			Player player1 = new Player("player1", null, new List<Card>(), new List<Card>(), health:1);
			Player player2 = new Player("TestOpponent", null);
			Game game = new Game(player1, player2);
			PrivateObject gameO = new PrivateObject(game);
			gameO.SetField("activePlayer", player1);
			gameO.SetField("opponentPlayer", player2);

			game.BeginTurn();	

			Assert.AreEqual(player2, game.GetWinner());
		}

		[Test]
		public void OpponentLoosesWhenHealthIsZeroTest()
		{
			IList<Card> hand = new List<Card>
			{
				new Card(4),
				new Card(6),
			};
			Player player1 = new Player("player1", null, new List<Card>(), hand, mana:10);
			Player player2 = new Player("player2", null, new List<Card>(), new List<Card>(), health:10);
			Game game = new Game(player1, player2);
			PrivateObject gameO = new PrivateObject(game);
			gameO.SetField("activePlayer", player1);
			gameO.SetField("opponentPlayer", player2);
			PrivateObject player1O = new PrivateObject(player1);
			player1O.Invoke("PlayCard", new Card(6), player2, ActionType.DAMAGE);
			player1O.Invoke("PlayCard", new Card(4), player2, ActionType.DAMAGE);

			Assert.AreEqual(player1, game.GetWinner());
		}

		[Test]
		public void OngoingGameHasNoWinnerTest()
		{
			IList<Card> hand = new List<Card>
			{
				new Card(4),
				new Card(6),
			};
			Player player1 = new Player("player1", null, new List<Card>(), hand, mana: 10);
			Player player2 = new Player("player2", null, new List<Card>(), new List<Card>(), health:30);
			Game game = new Game(player1, player2);
			PrivateObject gameO = new PrivateObject(game);
			gameO.SetField("activePlayer", player1);
			gameO.SetField("opponentPlayer", player2);

			PrivateObject player1O = new PrivateObject(player1);
			player1O.Invoke("PlayCard", new Card(4), player2, ActionType.DAMAGE);

			Assert.IsNull(game.GetWinner());
		}
	}
}