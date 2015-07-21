using Netricity.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tests
{
    public class StringUtilityTests
	{
		[Test]
		public void Convert_BadInt_ReturnsDefault()
		{
			string str = "0.";
			int defaultVal = -1;
			int expected = -1;
			int actual = StringUtility.Convert<int>(str, defaultVal);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Convert_NullableBool()
		{
			string str = null;
			bool? boolExpected = null;
			bool? boolActual = StringUtility.Convert<bool?>(str);
			Assert.AreEqual(boolExpected, boolActual);

			str = string.Empty;
			boolExpected = null;
			boolActual = StringUtility.Convert<bool?>(str);
			Assert.AreEqual(boolExpected, boolActual);

			str = "true";
			boolExpected = true;
			boolActual = StringUtility.Convert<bool?>(str);
			Assert.AreEqual(boolExpected, boolActual);

			str = "FALSE";
			boolExpected = false;
			boolActual = StringUtility.Convert<bool?>(str);
			Assert.AreEqual(boolExpected, boolActual);

			str = "Monkey";
			boolExpected = null;
			boolActual = StringUtility.Convert<bool?>(str);
			Assert.AreEqual(boolExpected, boolActual);
		}

		[Test]
		public void Convert_BadNullableInt_ReturnsDefault()
		{
			string str = "0.";
			int? defaultVal = -1;
			int? expected = -1;
			int? actual = StringUtility.Convert<int?>(str, defaultVal);

			Assert.AreEqual(expected, actual);

			str = "0.";
			defaultVal = null;
			expected = null;
			actual = StringUtility.Convert<int?>(str, defaultVal);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// A test for Convert<int?>
		/// </summary>
		[Test]
		public void ConvertNullableInt()
		{
			string str = null;
			int? intExpected = null;
			int? intActual = StringUtility.Convert<int?>(str);
			Assert.AreEqual(intExpected, intActual);

			str = string.Empty;
			intExpected = null;
			intActual = StringUtility.Convert<int?>(str);
			Assert.AreEqual(intExpected, intActual);

			str = "1";
			intExpected = 1;
			intActual = StringUtility.Convert<int?>(str);
			Assert.AreEqual(intExpected, intActual);

			str = "000";
			intExpected = 0;
			intActual = StringUtility.Convert<int?>(str);
			Assert.AreEqual(intExpected, intActual);

			//str = "Monkey";
			//intExpected = null;
			//intActual = StringUtility.Convert<int?>(str);
			//Assert.AreEqual(intExpected, intActual);
		}

		/// <summary>
		/// A test for FormatCmAsFeetAndInches
		///</summary>
		[Test]
		public void FormatCmAsFeetAndInchesTest()
		{
			double? cm = null;
			var strDefault = "-";
			string expected = strDefault;
			string actual = StringUtility.FormatCmAsFeetAndInches(cm, strDefault);
			Assert.AreEqual(expected, actual);

			cm = 0;
			strDefault = "-";
			expected = strDefault;
			actual = StringUtility.FormatCmAsFeetAndInches(cm, strDefault);
			Assert.AreEqual(expected, actual);

			cm = 30.48;
			strDefault = "-";
			expected = "1' 0\"";
			actual = StringUtility.FormatCmAsFeetAndInches(cm, strDefault);
			Assert.AreEqual(expected, actual);

			cm = 180.34;
			strDefault = "-";
			expected = "5' 11\"";
			actual = StringUtility.FormatCmAsFeetAndInches(cm, strDefault);
			Assert.AreEqual(expected, actual);

			cm = 180.1;
			strDefault = "-";
			expected = "5' 11\"";
			actual = StringUtility.FormatCmAsFeetAndInches(cm, strDefault);
			Assert.AreEqual(expected, actual);

			cm = 182;
			strDefault = "-";
			expected = "6' 0\"";
			actual = StringUtility.FormatCmAsFeetAndInches(cm, strDefault);
			Assert.AreEqual(expected, actual);

			cm = 183;
			strDefault = "-";
			expected = "6' 0\"";
			actual = StringUtility.FormatCmAsFeetAndInches(cm, strDefault);
			Assert.AreEqual(expected, actual);

			cm = 184;
			strDefault = "-";
			expected = "6' 0\"";
			actual = StringUtility.FormatCmAsFeetAndInches(cm, strDefault);
			Assert.AreEqual(expected, actual);

			cm = 185;
			strDefault = "-";
			expected = "6' 1\"";
			actual = StringUtility.FormatCmAsFeetAndInches(cm, strDefault);
			Assert.AreEqual(expected, actual);

			cm = 200;
			strDefault = "-";
			expected = "6' 7\"";
			actual = StringUtility.FormatCmAsFeetAndInches(cm, strDefault);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for FormatKilosAsStonesAndOunces
		///</summary>
		[Test]
		public void FormatKilosAsStonesAndPoundsTest()
		{
			double? kilos = null;
			var strDefault = "-";
			string expected = strDefault;
			string actual = StringUtility.FormatKilosAsStonesAndPounds(kilos, strDefault);
			Assert.AreEqual(expected, actual);

			kilos = 0;
			strDefault = "-";
			expected = strDefault;
			actual = StringUtility.FormatKilosAsStonesAndPounds(kilos, strDefault);
			Assert.AreEqual(expected, actual);

			kilos = 100;
			strDefault = "-";
			expected = "15st 10lb";
			actual = StringUtility.FormatKilosAsStonesAndPounds(kilos, strDefault);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// A test for GetColNameFromIndex
		///</summary>
		[Test]
		public void GetColNameFromNumberTest()
		{
			int columnNumber = 1;
			string expected = "A";
			string actual = StringUtility.GetColNameFromNumber(columnNumber);
			Assert.AreEqual(expected, actual);

			columnNumber = 2;
			expected = "B";
			actual = StringUtility.GetColNameFromNumber(columnNumber);
			Assert.AreEqual(expected, actual);

			columnNumber = 26;
			expected = "Z";
			actual = StringUtility.GetColNameFromNumber(columnNumber);
			Assert.AreEqual(expected, actual);

			columnNumber = 27;
			expected = "AA";
			actual = StringUtility.GetColNameFromNumber(columnNumber);
			Assert.AreEqual(expected, actual);

			columnNumber = 28;
			expected = "AB";
			actual = StringUtility.GetColNameFromNumber(columnNumber);
			Assert.AreEqual(expected, actual);

			columnNumber = 703;
			expected = "AAA";
			actual = StringUtility.GetColNameFromNumber(columnNumber);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ChopStart_MatchFound_ReturnsStartRemoved()
		{
			var str = "foobar";
			var result = StringUtility.ChopStart(str, "foo");

			Assert.AreEqual("bar", result);
		}
   }
}
