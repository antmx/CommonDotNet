using NUnit.Framework;
using Netricity.Common;
using System;

namespace Netricity.Common.Tests
{
	[TestFixture()]
	public class SystemTimeTests
	{
		[Test]
		public void NowTest()
		{
			var now = new DateTime(2001, 2, 3, 4, 5, 6, 789);

			SystemTime.NowFunction = () => now;

			var expected = now;
			var actual = SystemTime.Now;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TodayTest()
		{
			var now = new DateTime(2001, 2, 3, 4, 5, 6, 789);

			SystemTime.NowFunction = () => now;

			var expected = now.Date;
			var actual = SystemTime.Today;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ResetNowFunctonTest()
		{
			SystemTime.ResetNowFuncton();

			var originalNow = SystemTime.NowFunction;

			SystemTime.NowFunction = () => new DateTime(2001, 1, 1);

			Assert.AreNotEqual(originalNow, SystemTime.NowFunction);

			SystemTime.ResetNowFuncton();

			Assert.AreEqual(originalNow, SystemTime.NowFunction);
		}
	}
}