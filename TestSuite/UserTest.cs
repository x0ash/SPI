using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamAPI;

namespace TestSuite
{
    internal class UserTest
    {
        private User user;

        [SetUp]
        public void Setup()
        {
            user = new User();
            Game game1 = new Game();
            Game game2 = new Game();
            game1.SetTotalPlaytime(60);
            game2.SetTotalPlaytime(60);
            user.AddGame(game1);
            user.AddGame(game2);
        }

        [Test]
        public void TestPlaytime()
        {
            Assert.AreEqual(user.TotalPlaytimeInHours(), 2);
        }
    }
}
