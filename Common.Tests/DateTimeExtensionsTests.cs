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
			var expected = 3;
			var actual = DateTimeExtensions.AgeAt(
				new DateTime(2007, 4, 24),
				new DateTime(2010, 6, 11));

			Assert.AreEqual(expected, actual);
		}

		[Test()]
		public void DobFromAgeTest()
		{
			SystemTime.NowFunction = () => new DateTime(2010, 6, 11, 10, 15, 16, 178);

			var expected = new DateTime(2007, 1, 1);
			var actual = DateTimeExtensions.DobFromAge(3);

			Assert.AreEqual(expected, actual);
		}

		[Test()]
		public void DobFromAgeTest1()
		{
			var atDate = new DateTime(2010, 6, 11, 10, 15, 16, 178);

			var expected = new DateTime(2007, 1, 1);
			var actual = DateTimeExtensions.DobFromAge(3, atDate);

			Assert.AreEqual(expected, actual);
		}

		[Test()]
		public void ApproxAgeFromYobTest()
		{
			SystemTime.NowFunction = () => new DateTime(2010, 6, 11, 10, 15, 16, 178);

			// Null yob
			var expected = (int?)null;
			var actual = DateTimeExtensions.ApproxAgeFromYob(null);
			Assert.IsNull(actual);

			// Future yob
			expected = 0;
			actual = DateTimeExtensions.ApproxAgeFromYob(2011);
			Assert.IsNull(actual);

			// Current yob
			expected = 0;
			actual = DateTimeExtensions.ApproxAgeFromYob(2010);
			Assert.AreEqual(expected, actual);

			// Past yob
			expected = 3;
			actual = DateTimeExtensions.ApproxAgeFromYob(2007);
			Assert.AreEqual(expected, actual);
		}

		[Test()]
		public void ApproxYobFromAgeTest()
		{
			SystemTime.NowFunction = () => new DateTime(2010, 6, 11, 10, 15, 16, 178);

			// Null age
			var expected = (int?)null;
			var actual = DateTimeExtensions.ApproxYobFromAge(null);
			Assert.IsNull(actual);

			// Negative age
			expected = null;
			actual = DateTimeExtensions.ApproxAgeFromYob(-1);
			Assert.IsNull(actual);

			// Future yob
			expected = null;
			actual = DateTimeExtensions.ApproxAgeFromYob(2011);
			Assert.IsNull(actual);

			// Zero age
			expected = 0;
			actual = DateTimeExtensions.ApproxAgeFromYob(2010);
			Assert.AreEqual(expected, actual);

			// Future yob
			expected = 3;
			actual = DateTimeExtensions.ApproxAgeFromYob(2007);
			Assert.AreEqual(expected, actual);
		}

		[Test()]
		public void GetDayOfMonthSuffixTest()
		{
			var date = new DateTime(2010, 6, 1);
         var expected = "st";
			var actual = date.GetDayOfMonthSuffix();
			Assert.AreEqual(expected, actual);

			date = new DateTime(2010, 6, 2);
			expected = "nd";
			actual = date.GetDayOfMonthSuffix();
			Assert.AreEqual(expected, actual);

			date = new DateTime(2010, 6, 3);
			expected = "rd";
			actual = date.GetDayOfMonthSuffix();
			Assert.AreEqual(expected, actual);

			date = new DateTime(2010, 6, 12);
			expected = "th";
			actual = date.GetDayOfMonthSuffix();
			Assert.AreEqual(expected, actual);
		}

		[Test()]
		public void TruncateToWholeMinutesTest()
		{
			var date = new DateTime(2010, 6, 11, 10, 15, 59, 999);
			var expected = new DateTime(2010, 6, 11, 10, 15, 0);
			var actual = date.TruncateToWholeMinutes();
			Assert.AreEqual(expected, actual);
		}

      [Test]
      public void DateTimeTryParseExactSingleTest()
      {
         var strDate = "24/04/2007";
         var expected = new DateTime(2007, 4, 24);
         DateTime actual = new DateTime();

         Assert.IsTrue(DateTimeExtensions.DateTimeTryParseExact(strDate, "dd/MM/yyyy", ref actual));

         Assert.AreEqual(expected, actual);
      }

      [Test]
      public void DateTimeTryParseExactMultipleTest()
      {
         var strDate = "11/6/2010";
         var expected = new DateTime(2010, 6, 11);
         DateTime actual = new DateTime();

         var formats = new string[] { "dd/M/yyyy", "d/MM/yyyy" };

         Assert.IsTrue(DateTimeExtensions.DateTimeTryParseExact(strDate, formats, ref actual));

         Assert.AreEqual(expected, actual);
      }

      [Test]
      public void ToJavaScriptDateTest()
      {
         var date = new DateTime(2009, 5, 25);

         var actual = date.ToJavaScriptDate();
         var expected = "1243209600000";

         Assert.AreEqual(actual, expected);
      }
   }
}