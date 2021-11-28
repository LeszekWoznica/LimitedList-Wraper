using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MadSolution.DataAccessLayer.Tests
{
	class LimitedListWrapper_Tests
	{
		[Test]
		public void Foo() {

			var list = new LimitedListWrapper<int>(2);
		}
	}
}
