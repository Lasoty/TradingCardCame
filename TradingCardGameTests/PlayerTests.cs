using NUnit.Framework;
using TradingCardGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.NUnit3;
using Moq;
using TradingCardGame.Strategies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

namespace TradingCardGame.Tests
{
	[TestFixture()]
	public class PlayerTests
	{
		private Player testPlayer;

		[SetUp]
		public void SetupPlayer()
		{
			IFixture fixture = new Fixture();
			fixture.Customizations.Add(
																 new TypeRelay(
																							 typeof(Strategy),
																							 typeof(ConsoleInputStrategy)));
			Strategy strategy = fixture.Create<Strategy>();
			testPlayer = new Player("Test", strategy);
		}

		[Test]
		public void PlayerShouldHave30InitialHealthTest()
		{
			Assert.AreEqual(30, testPlayer.Health);
		}

		[Test]
		public void PlayerShouldHaveZeroInitialManaTest()
		{

			Assert.AreEqual(0, testPlayer.Mana);
		}

		[TestCase(0, 2)]
		[TestCase(1, 2)]
		[TestCase(2, 3)]
		[TestCase(3, 4)]
		[TestCase(4, 3)]
		[TestCase(5, 2)]
		[TestCase(6, 2)]
		[TestCase(7, 1)]
		[TestCase(8, 1)]
		public void CardDeckShouldContainInitialCardsTest(int manaCost, int expected)
		{
			Assert.AreEqual(expected, testPlayer.GetNumberOfDeckCardsWithManaCost(manaCost));
		}

		[Test]
		public void PlayerStartsWithEmptyHandTest()
		{
			Assert.AreEqual(0, testPlayer.GetNumberOfHandCards());
		}

		[Test]
		public void DrawingACardShouldMoveOneCardFromDeckIntoHandTest()
		{
			IList<Card> deck = new List<Card>
			{
				new Card(1),
				new Card(1),
				new Card(2),
			};
			Player player = new Player("Test", null, deck, new List<Card>());

			player.DrawCard();
			Assert.AreEqual(2, player.GetNumberOfDeckCards());
			Assert.AreEqual(1, player.GetNumberOfHandCards());
		}

		[Test]
		public void PlayerShouldTakeOneDamageWhenDrawingFromEmptyDeckTest()
		{
			Player player = new Player("Test", null, new List<Card>(), new List<Card>(), 30);
			player.DrawCard();
			Assert.AreEqual(player.Health, 29);
		}

		[Test]
		public void ShouldDiscardDrawnCardWhenHandSizeIsFiveTest()
		{
			IList<Card> deck = new List<Card>
			{
				new Card(1),
			};
			IList<Card> hand = new List<Card>
			{
				new Card(1),
				new Card(2),
				new Card(3),
				new Card(4),
				new Card(5),
			};
			Player player = new Player("Test", null, deck, hand);

			player.DrawCard();

			Assert.AreEqual(player.GetNumberOfHandCards(), 5);
			Assert.AreEqual(player.GetNumberOfDeckCards(), 0);
		}

		[Test]
		public void PlayingCardsReducesPlayersManaTest()
		{
			IList<Card> hand = new List<Card>
			{
				new Card(8),
				new Card(1),
			};
			Player player = new Player("Test", null, new List<Card>(), hand, mana: 10);
			PrivateObject playerO = new PrivateObject(player);
			playerO.Invoke("PlayCard", new Card(8), new StartingPlayerChooser().ChooseBetween(testPlayer, testPlayer), ActionType.DAMAGE);
			playerO.Invoke("PlayCard", new Card(1), new StartingPlayerChooser().ChooseBetween(testPlayer, testPlayer), ActionType.DAMAGE);

			Assert.AreEqual(1, player.Mana);
		}

		[Test]
		public void PlayingCardsRemovesThemFromHandTest()
		{
			IList<Card> hand = new List<Card>
			{
				new Card(0),
				new Card(2),
				new Card(2),
				new Card(3),
			};
			Player player = new Player("Test", null, new List<Card>(), hand, mana: 5);

			PrivateObject playerO = new PrivateObject(player);
			playerO.Invoke("PlayCard", new Card(3), new StartingPlayerChooser().ChooseBetween(testPlayer, testPlayer), ActionType.DAMAGE);
			playerO.Invoke("PlayCard", new Card(2), new StartingPlayerChooser().ChooseBetween(testPlayer, testPlayer), ActionType.DAMAGE);

			Assert.AreEqual(0, player.GetNumberOfHandCardsWithManaCost(3));
			Assert.AreEqual(1, player.GetNumberOfHandCardsWithManaCost(2), 1);
		}

		[Test]
		//[ExpectedException(typeof(System.Reflection.TargetInvocationException))]
		public void PlayingCardWithInsufficientManaShouldFailTest()
		{
			IList<Card> hand = new List<Card>
			{
				new Card(4),
				new Card(4),
				new Card(4),
			};

			Player player = new Player("Test", null, new List<Card>(), hand, mana: 3);

			PrivateObject playerO = new PrivateObject(player);
			try
			{
				playerO.Invoke("PlayCard", new Card(4), new StartingPlayerChooser().ChooseBetween(testPlayer, testPlayer), ActionType.DAMAGE);
			}
			catch (Exception ex)
			{
				if (ex.InnerException.Message.Contains("Insufficient Mana")) Assert.Pass();
				else Assert.Fail("Wskazany błąd nie zawiera frazy 'Insufficient Mana'");
			}
		}

		[Test]
		public void PlayingCardCausesDamageToOpponentTest()
		{
			IList<Card> hand = new List<Card>
			{
				new Card(3),
				new Card(2),
			};
			Player player = new Player("TestPlayer", null, new List<Card>(), hand, mana: 10);
			Player opponent = new Player("TestOpponent", null, new List<Card>(), hand, mana: 10, health: 30);
			PrivateObject playerO = new PrivateObject(player);
			playerO.Invoke("PlayCard", new Card(3), opponent, ActionType.DAMAGE);
			playerO.Invoke("PlayCard", new Card(2), opponent, ActionType.DAMAGE);

			Assert.AreEqual(25, opponent.Health);
		}

		[Test]
		public void PlayerWithSufficientManaCanPlayCardsTest()
		{
			IList<Card> hand = new List<Card>
			{
				new Card(3),
				new Card(2),
			};
			Player player = new Player("TestPlayer", null, new List<Card>(), hand, mana: 2);

			Assert.IsTrue(player.CanPlayCards());
		}

		[Test]
		public void PlayerWithInsufficientManaCannotPlayCardsTest()
		{
			IList<Card> hand = new List<Card>
			{
				new Card(3),
				new Card(2),
			};
			Player player = new Player("TestPlayer", null, new List<Card>(), hand, mana: 1);

			Assert.IsFalse(player.CanPlayCards());
		}

		[Test]
		public void PlayerWithEmptyHandCannotPlayCardsTest()
		{
			Player player = new Player("TestPlayer", null, new List<Card>(), new List<Card>());

			Assert.IsFalse(player.CanPlayCards());
		}

		[Test]
		public void PlayingCardAsHealingRestoresHealthTest()
		{
			IList<Card> hand = new List<Card>
			{
				new Card(3),
				new Card(4),
			};
			Player player = new Player("TestPlayer", null, new List<Card>(), hand, 10, 10);

			PrivateObject playerO = new PrivateObject(player);
			playerO.Invoke("PlayCard", new Card(3), player, ActionType.HEALING);
			playerO.Invoke("PlayCard", new Card(4), player, ActionType.HEALING);

			Assert.AreEqual(17, player.Health);
		}

		[Test]
		public void PlayerCannotHealAbove30HealthTest()
		{
			IList<Card> hand = new List<Card>
			{
				new Card(4),
			};
			Player player = new Player("TestPlayer", null, new List<Card>(), hand, 27, 10);
			PrivateObject playerO = new PrivateObject(player);
			playerO.Invoke("PlayCard", new Card(4), player, ActionType.HEALING);
			Assert.AreEqual(30, player.Health);
		}
	}
}