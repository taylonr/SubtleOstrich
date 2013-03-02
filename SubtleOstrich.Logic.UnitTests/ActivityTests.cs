using System;
using System.Linq;
using NUnit.Framework;
using Should;

namespace SubtleOstrich.Logic.UnitTests
{
    [TestFixture]
    public class ActivityTests
    {
        private Activity _activity;

        [SetUp]
        public void SetUp()
        {
            _activity = new Activity("technique");
        }

        [Test]
        public void Constructor_Should_Set_Name()
        {
            _activity.Name.ShouldEqual("technique");
        }

        [Test]
        public void AddRecord_Should_Add_Record_To_List()
        {
            _activity.AddRecord(new Record(DateTime.Today));
            _activity.Records.Any().ShouldBeTrue();
        }

        [Test]
        public void GetMonthlyTotal_Should_Return_Number_Of_Records_In_Month()
        {
            for(var i = 1; i <= 10; i++)
                _activity.AddRecord(new Record(new DateTime(2013, 3, i)));

            _activity.GetMonthlyTotal(3).ShouldEqual(10);
        }

        [Test]
        public void GetYearlyTotal_Should_Return_Number_Of_Records_In_Year()
        {
            for(var i = 1; i <= 10; i++)
                _activity.AddRecord(new Record(new DateTime(2013, i, 1)));

            _activity.GetYearlyTotal(2013).ShouldEqual(10);
        }

        [Test]
        public void GetNotesForMonth_Should_Only_Return_Records_With_Notes_In_Month()
        {
            for(var i =1; i <= 10; i++)
                _activity.AddRecord(new Record(new DateTime(2013, 3, i)));

            _activity.AddRecord(new Record(new DateTime(2013, 3, 1), "A"));

            _activity.GetNotesForMonth(3).Count().ShouldEqual(1);
        }
    }
}