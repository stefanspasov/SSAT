using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using TestEnvironment.Entities;

namespace Orchestrator
{
    public class TestAccess : ITestAccess
    {
        public IList<TestCase> LoadTestCases()
        {
            var serializer = new XmlSerializer(typeof(TestSuite));
            // TODO: locate the test-suite file using OpenFileDialog
            var ts = (TestSuite) serializer.Deserialize(new StreamReader("test-suite.xml"));

            // Get clients' locations based on their names
            var clientCollection = ConfigurationManager.GetSection("clientCollection") as NameValueCollection;
            if (clientCollection != null) {
                foreach (var client in ts.TestCases.SelectMany(t => t.Steps).SelectMany(s => s.Actions).Select(a => a.TargetClient)) {
                    client.IpAddress = IPAddress.Parse(clientCollection[client.Name]);
                }
            }
            
            return ts.TestCases;
        }

        //private TestCase CreateFakeTestCase1()
        //{
        //    TestCase testCase = new TestCase() { 
        //        Id = new Random().Next().ToString(),
        //        Name = "t_esc_1 Check menu display",
        //        Description = "Verifies the specifications for the input devices (Pen, software keyboard and software keypad)"
        //    };
        //    Client My_PC = new Client(IPAddress.Loopback, "My PC");
        //    Step tc1step1 = new Step();

        //    Operation o1 = new Operation("<start>", TestTechnology.Sim);
        //    TestAction ta1 = new TestAction(My_PC, o1, false) { Description = "Start the Sim" };

        //    Operation o2 = new Operation("7000^call 0056.txt", TestTechnology.Sim);
        //    TestAction ta2 = new TestAction(My_PC, o2, false) { Description = "0056.txt on the sim" };

        //    Operation o3 = new Operation("<stop>", TestTechnology.Sim);
        //    TestAction ta3 = new TestAction(My_PC, o3, false) { Description = "Stop the sim" };

        //    //Operation o4 = new Operation("reusableScriptTest1.sikuli", TestTechnology.Sikuli);
        //    Operation o4 = new Operation("t_estr_41_r1.sikuli", TestTechnology.Sikuli);
        //    TestAction ta4 = new TestAction(My_PC, o4, true) { Description = "Five departures and an arrival appear on the strip board." };

        //    Operation o5 = new Operation("t_estr_41_a3.sikuli", TestTechnology.Sikuli);
        //    TestAction ta5 = new TestAction(My_PC, o5, true) { Description = "Right click the action menu" };

        //    tc1step1.Actions.Enqueue(ta1);
        //    tc1step1.Actions.Enqueue(ta2);
        //    tc1step1.Actions.Enqueue(ta3);
        //    tc1step1.Actions.Enqueue(ta4);
        //    tc1step1.Actions.Enqueue(ta5);

        //    //Operation o_ac1st1 = new Operation("reusableScriptTest1.sikuli", TestTechnology.Sikuli);
        //    //Operation o_ac2st1 = new Operation("do!", TestTechnology.Human);
        //    //Operation o_ac3st1 = new Operation("reusableScriptTest2.sikuli", TestTechnology.Sikuli);

        //    //TestAction step1action1 = new TestAction(My_PC, o_ac1st1, true) { Description = "Click on the arrow button" };
        //    //TestAction step1action2 = new TestAction(My_PC, o_ac2st1, false) { Description = o_ac2st1.Directive };
        //    //TestAction step1action3 = new TestAction(My_PC, o_ac3st1, true) { Description = "Click on the arrow button" };

        //    //tc1step1.Actions.Enqueue(step1action1);
        //    //tc1step1.Actions.Enqueue(step1action2);
        //    //tc1step1.Actions.Enqueue(step1action3);

        //    testCase.Steps.Enqueue(tc1step1);
        //    return testCase;
        //}

        //private TestCase CreateFakeTestCase2() {
        //    TestCase testCase = new TestCase() {
        //        Id = new Random().Next().ToString(),
        //        Name = "t_estr_103",
        //        Description = "Tests that flights disappear if they are delayed beyond the activation time threshold."
        //    };
        //    Client My_PC = new Client(IPAddress.Loopback, "My PC");
        //    Step tc1step1 = new Step();


        //    Operation o1 = new Operation("call 0157.txt", TestTechnology.Sim);
        //    TestAction a1 = new TestAction(My_PC, o1, false) { Description = "Run 0157.txt" };
        //    tc1step1.Actions.Enqueue(a1);

        //    Operation o2 = new Operation("reusableScriptTest1.sikuli", TestTechnology.Sikuli);
        //    TestAction a2 = new TestAction(My_PC, o2, true) { Description = "Check for DEP 1 - 4" };
        //    tc1step1.Actions.Enqueue(a2);

        //    Operation o3 = new Operation("reusableScriptTest1.sikuli", TestTechnology.Sikuli);
        //    TestAction a3 = new TestAction(My_PC, o3, true) { Description = "Assume DEP002" };
        //    tc1step1.Actions.Enqueue(a3);

        //    Operation o4 = new Operation("go", TestTechnology.Sim);
        //    TestAction a4 = new TestAction(My_PC, o4, false) { Description = "Continue with the simulator scanario" };
        //    tc1step1.Actions.Enqueue(a4);

        //    Operation o5 = new Operation("All strips have EOBT delayed 10 minutes. DEP001 disappears.", TestTechnology.Human);
        //    TestAction a5 = new TestAction(My_PC, o5, false) { Description = "All strips have EOBT delayed 10 minutes." };
        //    tc1step1.Actions.Enqueue(a5);

        //    Operation o6 = new Operation("go", TestTechnology.Sim);
        //    TestAction a6 = new TestAction(My_PC, o6, false) { Description = "Continue with the simulator scanario" };
        //    tc1step1.Actions.Enqueue(a6);

        //    Operation o7 = new Operation("All strips have EOBT delayed 10 minutes. DEP002 does not disappear, even though it has an EOBT further than activation time in the future.", TestTechnology.Human);
        //    TestAction a7 = new TestAction(My_PC, o7, false) { Description = "All strips have EOBT delayed 10 minutes." };
        //    tc1step1.Actions.Enqueue(a7);

        //    Operation o8 = new Operation("go", TestTechnology.Sim);
        //    TestAction a8 = new TestAction(My_PC, o8, false) { Description = "Continue with the simulator scanario" };
        //    tc1step1.Actions.Enqueue(a8);

        //    Operation o9 = new Operation("<stop>", TestTechnology.Sim);
        //    TestAction a9 = new TestAction(My_PC, o9, false) { Description = "Stop the sim" };
        //    tc1step1.Actions.Enqueue(a9);

        //    Operation o10 = new Operation("reusableScriptTest1.sikuli", TestTechnology.Sikuli);
        //    TestAction a10 = new TestAction(My_PC, o10, true) { Description = "DEP001 appears within 1 min" };
        //    tc1step1.Actions.Enqueue(a10);

        //    //// Step 2
        //    //Step tc1step2 = new Step();
        //    //var utInfo = new UnitTestInfo() {
        //    //    Tests = new List<string> { "TestHeapSort" },
        //    //    AssemblyName = "CrackingInterviewTest",
        //    //    WorkingDirectory = "C:/Temp"
        //    //};
        //    //Operation o_ac1st2 = new Operation(JsonConvert.SerializeObject(utInfo), TestTechnology.UnitTest);
        //    //TestAction step2action1 = new TestAction(My_PC, o_ac1st2, false) { Description = "Run all unit tests" };
        //    //tc1step2.Actions.Enqueue(step2action1);

        //    //testCase.Steps.Enqueue(tc1step2);
        //    testCase.Steps.Enqueue(tc1step1);
        //    return testCase;
        //}
    }
}
