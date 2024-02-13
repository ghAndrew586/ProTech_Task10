using Task_9.Controllers;

namespace Task_10.Tests
{
    public class MainLogicControllerTests
    {
        MainLogicController mainLogic = new MainLogicController();


        [TestCase(5, 2, 7)]
        [TestCase(10, 5, 15)]
        public void SumNum_5plus2_7return(int x, int y, int result)
        {
            int actual = mainLogic.SumNum(x, y);

            Assert.That(result, Is.EqualTo(actual));
        }
    }
}