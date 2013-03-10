using System;
using FakeItEasy;
using NUnit.Framework;
using Should;
using MongoRepository;

namespace SubtleOstrich.Logic.UnitTests
{
    [TestFixture]
    public class UserTests
    {
        private User _user;
        private IUserRepository _repo;

        [SetUp]
        public void SetUp()
        {
            _repo = A.Fake<IUserRepository>();
            _user = new User("A", "test user", "facebook", _repo);
        }

        [Test]
        public void AddActivity_Should_Add_Activity()
        {
            _user.AddActivity(new Activity("technique"));
            _user.Activities.Count.ShouldEqual(1);
        }

        [Test]
        public void AddRecord_Should_Add_Record_To_Activity()
        {
            _user.AddActivity(new Activity("technique"));
            _user.AddRecord("technique", new Record(new DateTime(2013, 3, 1)));
            _user.GetMonthTotal(3).ShouldEqual(1);
        }

        [Test]
        public void AddRecord_Should_Create_New_Activity_When_Activity_Does_Not_Exist()
        {
            _user.AddRecord("technique", new Record(new DateTime(2013, 3, 1)));
            _user.Activities.Count.ShouldEqual(1);
        }

        [Test]
        public void GetMonthTotal_Should_Sum_All_Activities_For_Month()
        {
            var activity = new Activity("technique");
            AddRecordsToActivity(activity);
            _user.AddActivity(activity);

            var secondActivity = new Activity("sparring");
            AddRecordsToActivity(secondActivity);
            _user.AddActivity(secondActivity);

            _user.GetMonthTotal(3).ShouldEqual(20);
        }

        private static void AddRecordsToActivity(Activity activity)
        {
            for (var i = 1; i <= 10; i++)
                activity.AddRecord(new Record(new DateTime(2013, 3, i)));
        }

        [Test]
        public void GetYearTotal_Should_Sum_All_Activities_For_Year()
        {
            AddRecordForYear("technique");
            AddRecordForYear("sparring");
            _user.GetYearTotal(2013).ShouldEqual(20);
        }

        private void AddRecordForYear(string activityName)
        {
            for (var i = 1; i <= 10; i++)
                _user.AddRecord(activityName, new Record(new DateTime(2013, i, 1)));
        }

        [Test]
        public void Save_Should_Save_In_Database()
        {
            _user.AddRecord("technique", new Record(new DateTime(2013, 3, 1)));
            _user.Save();
            A.CallTo(() => _repo.Save(A<User>.That.Matches(x => x.Activities.Count == 1))).MustHaveHappened();
        }

        [Test]
        public void Constructor_Should_Set_Id_To_Uid_And_Provider()
        {
            _user.Id.ShouldEqual("facebook:A");
        }

        [Test]
        public void Constructor_Should_Set_Name()
        {
            _user.Name.ShouldEqual("test user");
        }

        [Test]
        public void GetActivities_Should_Call_Database_With_Date()
        {
            _user.GetActivities(DateTime.Today);
            A.CallTo(() => _repo.GetActivities(A<string>._, DateTime.Today)).MustHaveHappened();
        }
    }
}