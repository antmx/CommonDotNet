using NUnit.Framework;
using Netricity.Common;
using System;
using System.Collections.Specialized;

namespace Netricity.Common.Tests
{
	[TestFixture()]
	public class WebUtilityTests
	{
		[Test()]
		public void ParseRequestParamTest()
		{
			// Arrange
			var query = new NameValueCollection();
			query.Add("foo", "1");
			query.Add("bar", "true");

			// Act
			var foo = WebUtility.ParseRequestParam(query, "foo", 1);
			var bar = WebUtility.ParseRequestParam(query, "bar", false);
			var baz = WebUtility.ParseRequestParam(query, "baz", (float?)null);

			// Assert
			Assert.AreEqual(1, foo);
			Assert.AreEqual(true, bar);
			Assert.IsNull(baz);
		}
	}
}