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
	public class CustomExceptionTests
	{
		[Test()]
		public void CustomExceptionTest()
		{
			var ex1 = new CustomException("foo");
			var ex2 = new CustomException("bar {0} {1}", 1, 2);

			Assert.AreEqual("foo", ex1.Message);
			Assert.AreEqual("bar 1 2", ex2.Message);
		}
	}
}