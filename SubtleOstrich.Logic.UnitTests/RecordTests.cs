using System;
using NUnit.Framework;
using Should;

namespace SubtleOstrich.Logic.UnitTests
{
    [TestFixture]
    public class RecordTests
    {
        [Test]
        public void Constructor_Should_Set_Date()
        {
            var r = new Record(DateTime.Today);
            r.Date.ShouldEqual(DateTime.Today);
        }

        [Test]
        public void Constructor_Should_Set_Note()
        {
            var r = new Record(DateTime.Today, "A");
            r.Note.ShouldEqual("A");
        }
    }
}