using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Task_10.Controllers;

namespace Task_10.Tests
{
    public class MainLogicControllerTests
    {
        MainLogicController mainLogic = new MainLogicController();

        [TestCase("a", "aa")]
        [TestCase("abcdef", "cbafed")]
        [TestCase("abcde", "edcbaabcde")]
        public void LogicTask1_abcdef_cbafed_or_abcde_edcbaabcde(string mainLine, string result)
        {
            string actual = mainLogic.LogicTask1(mainLine);
            Assert.That(result, Is.EqualTo(actual));
        }
    }
}