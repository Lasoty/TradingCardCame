using NUnit.Framework;
using TradingCardGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCardGame.Tests
{
	[TestFixture]
	public class CardTests
	{
		[Test]
		public void CompareToShouldReturnTrueTest()
		{
			Assert.AreEqual(new Card(1), new Card(1));
		}

		[Test]
		public void EqualsShouldReturnFalseForNullTest()
		{
			Assert.IsFalse(new Card(1).Equals(null));
		}

		[Test]
		public void EqualsShouldReturnFalseForDiferentTypeTest()
		{
			Assert.IsFalse(new Card(1).Equals(1));
		}
	}
}