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
        public void LogicTask1_abcdef_cbafed_OR_abcde_edcbaabcde(string mainLine, string result)
        {
            string actual = mainLogic.LogicTask1(mainLine);
            Assert.That(result, Is.EqualTo(actual));
        }

        [TestCase("abcd456", "456")]
        [TestCase("Abcd", "A")]
        [TestCase("HelloПривет", "HПривет")]
        [TestCase("       ", "")]
        [TestCase("abcdef", null)]
        public void LogicTask2_abcd456_456return(string inputLine, string result)
        {
            string actual = mainLogic.LogicTask2(inputLine);
            Assert.That(result, Is.EqualTo(actual));
        }
    }
}