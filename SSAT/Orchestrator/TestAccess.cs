using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestEnvironment.Entities;

namespace Orchestrator
{
    public class TestAccess : ITestAccess
    {
        public IList<TestCase> LoadTestCases()
        {
            return new List<TestCase>() { CreateFakeTestCase1(), CreateFakeTestCase2() };
        }

        private TestCase CreateFakeTestCase1()
        {
            TestCase testCase = new TestCase() { 
                Id = new Random().Next().ToString(),
                Name = "t_esc_1 Check menu display",
                Description = "Verifies the specifications for the input devices (Pen, software keyboard and software keypad)"
            };
            Client My_PC = new Client(IPAddress.Loopback, "My PC");
            Step tc1step1 = new Step();

            Operation o_ac1st1 = new Operation("reusableScriptTest1.sikuli", TestTechnology.Sikuli);
            Operation o_ac2st1 = new Operation("do!", TestTechnology.Human);
            Operation o_ac3st1 = new Operation("reusableScriptTest2.sikuli", TestTechnology.Sikuli);

            TestAction step1action1 = new TestAction(My_PC, o_ac1st1, true) { Description = "Click on the arrow button" };
            TestAction step1action2 = new TestAction(My_PC, o_ac2st1, false) { Description = o_ac2st1.Directive };
            TestAction step1action3 = new TestAction(My_PC, o_ac3st1, true) { Description = "Click on the arrow button" };

            tc1step1.Actions.Enqueue(step1action1);
            tc1step1.Actions.Enqueue(step1action2);
            tc1step1.Actions.Enqueue(step1action3);

            testCase.Steps.Enqueue(tc1step1);
            return testCase;
        }

        private TestCase CreateFakeTestCase2() {
            TestCase testCase = new TestCase() {
                Id = new Random().Next().ToString(),
                Name = "t_esc_2 Test test test",
                Description = "Verifies the specifications for the input devices (Pen, software keyboard and software keypad)"
            };
            Client My_PC = new Client(IPAddress.Loopback, "My PC");
            Step tc1step1 = new Step();

            Operation o_ac1st1 = new Operation("reusableScriptTest1.sikuli", TestTechnology.Sikuli);
            Operation o_ac2st1 = new Operation("do!", TestTechnology.Human);
            Operation o_ac3st1 = new Operation("reusableScriptTest2.sikuli", TestTechnology.Sikuli);

            TestAction step1action1 = new TestAction(My_PC, o_ac1st1, true) { Description = "Click on the arrow button" };
            TestAction step1action2 = new TestAction(My_PC, o_ac2st1, false) { Description = o_ac2st1.Directive };
            TestAction step1action3 = new TestAction(My_PC, o_ac3st1, true) { Description = "Click on the arrow button" };

            tc1step1.Actions.Enqueue(step1action1);
            tc1step1.Actions.Enqueue(step1action2);
            tc1step1.Actions.Enqueue(step1action3);

            // Step 2
            Step tc1step2 = new Step();
            var utInfo = new UnitTestInfo() {
                Tests = new List<string> { "TestHeapSort" },
                AssemblyName = "CrackingInterviewTest",
                WorkingDirectory = "C:/Temp"
            };
            Operation o_ac1st2 = new Operation(JsonConvert.SerializeObject(utInfo), TestTechnology.UnitTest);
            TestAction step2action1 = new TestAction(My_PC, o_ac1st2, false) { Description = "Run all unit tests" };
            tc1step2.Actions.Enqueue(step2action1);

            testCase.Steps.Enqueue(tc1step2);
            testCase.Steps.Enqueue(tc1step1);
            return testCase;
        }
    }
}
