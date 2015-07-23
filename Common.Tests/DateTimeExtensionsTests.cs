using NUnit.Framework;
using Netricity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netricity.Common.Tests
{
	[TestFixture()]
	public class DateTimeExtensionsTests
	{
		[Test()]
		public void ToJsDateCtorTest()
		{
			var date = new DateTime(2007, 4, 24, 9, 30, 59, 123);
			var expected = "new Date(2007, 3, 24, 9, 30, 59, 123)";
			var actual = date.ToJsDateCtor();

			Assert.AreEqual(expected, actual);
		}

		[Test()]
		public void AgeNowTest()
		{
			// Override how SystemTime calculates the current DateTime
			SystemTime.NowFunction = () => new DateTime(2010, 6, 11, 10, 15, 16, 178);

			var expected = 3;
			var actual = DateTimeExtensions.AgeNow(new DateTime(2007, 4, 24));

			Assert.AreEqual(expected, actual);
		}

		[Test()]
		public void AgeAtTest()
		{
			Assert.Inconclusive();
		}

		[Test()]
		public void DobFromAgeTest()
		{
			Assert.Inconclusive();
		}

		[Test()]
		public void DobFromAgeTest1()
		{
			Assert.Inconclusive();
		}

		[Test()]
		public void ApproxAgeFromYobTest()
		{
			Assert.Inconclusive();
		}

		[Test()]
		public void ApproxYobFromAgeTest()
		{
			Assert.Inconclusive();
		}

		[Test()]
		public void GetDayOfMonthSuffixTest()
		{
			Assert.Inconclusive();
		}

		[Test()]
		public void TruncateToWholeMinutesTest()
		{
			Assert.Inconclusive();
		}
	}
}