using Microsoft.AspNetCore.Components.Forms;
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
        public void LogicTask1_abcdef_cbafed_OR_abcde_edcbaabcde(string inputLine, string result)
        {
            string actual = mainLogic.LogicTask1(inputLine);
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


        //[TestCase("cbafed",)]
        public void LogicTask3_abcddf_a1b1c1d2f1return(string inputLine, Dictionary<char, int> result)
        {
            new Dictionary<char, int> { { 'a', 1 }, { 'b', 1 } };
            Dictionary<char, int> actual = mainLogic.LogicTask3(inputLine);
            Assert.That(result, Is.EqualTo(actual));
        }

        [TestCase("aa", "aa")]
        [TestCase("cbafed", "afe")]
        [TestCase("edcbaabcde", "edcbaabcde")]
        public void LogicTask4_abcdef_afe(string inputLine, string result)
        {
            string actual = mainLogic.LogicTask4(inputLine);
            Assert.That(result, Is.EqualTo(actual));
        }
    }
}