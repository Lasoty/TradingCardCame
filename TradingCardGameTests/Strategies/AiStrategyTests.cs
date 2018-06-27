using NUnit.Framework;
using TradingCardGame.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

namespace TradingCardGame.Strategies.Tests
{
	[TestFixture]
	public class AiStrategyTests
	{
		private Strategy strategy;

		[SetUp]
		public void Setup()
		{
			strategy = new AiStrategy();
		}

		[Test]
		public void ShouldMaximizeDamageOutputInCurrentTurnTest()
		{
			IEnumerable<Card> cards = new[]
			{
				new Card(7), 
				new Card(6), 
				new Card(4), 
				new Card(3), 
				new Card(2), 
			};
			Move move = strategy.NextMove(8, 30, cards);
			if (move.Card.Value == 2 || move.Card.Value == 6) Assert.Pass();
			else Assert.Fail("Strategy not attacking with card 2 or 6");
		}

		[Test]
		public void ShouldPlayAsManyCardsAsPossibleForMaximumDamageTest()
		{
			IEnumerable<Card> cards = new[]
			{
				new Card(1),
				new Card(2),
				new Card(3),
			};
			Move move = strategy.NextMove(3, 30, cards);
			if (move.Card.Value == 1 || move.Card.Value == 2) Assert.Pass();
			else Assert.Fail("Strategy not attacking with card 1 or 2");
		}

		[Test]
		public void ShouldPickHighestAffordableCardWhenNoComboIsPossibleTest()
		{
			IEnumerable<Card> cards = new[]
			{
				new Card(1),
				new Card(2),
				new Card(3),
			};
			Move move = strategy.NextMove(2, 30, cards);
			Assert.AreEqual(2, move.Card.Value);
		}

		[Test]
		public void ShouldUseHealingUntilHealthIsAtLeast10Test()
		{
			IList<Card> cards = new List<Card>
			{
				new Card(1),
				new Card(1),
				new Card(1),
			};
			Move move = strategy.NextMove(3, 8, cards);
			Assert.AreEqual(ActionType.HEALING,move.Action);
			Assert.AreEqual(1, move.Card.Value);

			cards.RemoveAt(0);
			move = strategy.NextMove(2, 9, cards);
			Assert.AreEqual(ActionType.HEALING, move.Action);
			Assert.AreEqual(1, move.Card.Value);

			cards.RemoveAt(0);
			move = strategy.NextMove(1, 10, cards);
			Assert.AreEqual(ActionType.DAMAGE, move.Action);
			Assert.AreEqual(1, move.Card.Value);
		}

		[Test]
		public void ShouldReturnNoCardIfInsufficientManaForAnyHandCardTest()
		{
			IList<Card> cards = new List<Card>
			{
				new Card(2),
				new Card(3),
				new Card(8),
			};
			Move move = strategy.NextMove(1, 30, cards);
			Assert.IsNull(move.Card);
		}
	}
}