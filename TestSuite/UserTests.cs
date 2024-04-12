using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamAPI;

namespace TestSuite
{
    [TestFixture]
    internal class UserTests
    {
        private User user;

        [SetUp]
        public void Setup()
        {
            user = new User();
            Game game1 = new Game();
            Game game2 = new Game();
            game1.SetTotalPlaytimeInMinutes(60);
            game2.SetTotalPlaytimeInMinutes(60);
            game1.SetRecentPlaytimeInMinutes(30);
            game2.SetRecentPlaytimeInMinutes(30);

            user.SetJoinDate(DateTime.MinValue);

            user.AddGame(game1);
            user.AddGame(game2);
        }

        [Test]
        public void TestPlaytime()
        {
            // 60 + 60 min = 2hr
            Assert.AreEqual(user.TotalPlaytimeInHours(), 2);
        }

        [Test]
        public void TestRecentPlaytime()
        {
            // 30 + 30 min = 1hr
            Assert.AreEqual(user.RecentPlaytimeInHours(), 1);
        }

        [Test]
        public void TestAccountLifetime()
        {
            TimeSpan lifeTime = DateTime.Now - DateTime.MinValue;
            Assert.AreEqual(user.AccountLifeTimeInDays(), lifeTime.Days);

        }
    }
}
