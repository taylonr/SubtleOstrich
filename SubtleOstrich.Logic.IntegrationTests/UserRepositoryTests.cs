using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Should;

namespace SubtleOstrich.Logic.IntegrationTests
{
	[TestFixture]
	public class UserRepositoryTests
	{
		[Test]
		public void Should_Return_User()
		{
			var r = new UserRepository();
			var user = r.GetBySourceAndId("facebook", "100005591252164");
			user.Name.ShouldContain("Sadansky");
		}
	}
}
