using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SATFUtilities;
using System.Net;
using Newtonsoft.Json;
using TestEnvironment.Entities;
using System.IO;
using TestEnvironment;
namespace Orchestrator
{
    public partial class MainForm : Form
    {
        TestCase testCase1;
        string _formResult = "";
        public MainForm()
        {
            InitializeComponent();
            InitializeSteps();
            TestEnvironment.Environment.Instance.Setup(testCase1); 
        }


        private void Run(object sender, EventArgs e)
        {
            var files = testCase1.Steps.SelectMany(t => t.Actions.Where(a => a.FileState == FileState.NotReady)).ToList();
            Thread runScriptThread = new Thread(new ThreadStart(() => SendFilesHandler(files)));  
            runScriptThread.Start();

            while (testCase1.Steps.Any())
            {
                Step currentStep = testCase1.Steps.Dequeue();
                while (currentStep.Actions.Any())
                {
                    TestAction currentAction = currentStep.Actions.Peek();
                    if (currentAction.FileState == FileState.Ready || currentAction.FileState == FileState.NoFile)
                    {
                        currentAction = currentStep.Actions.Dequeue();
                        TcpClient clientSocket = new TcpClient();
                        clientSocket.Connect(currentAction.TargetClient.IpAddress, Constants.RUN_SCRIPT_PORT);
                        Connectivity.SendData(clientSocket, JsonConvert.SerializeObject(currentAction.Operation));
                        if (currentAction.Operation.Executor == TestTechnology.Human)
                        {
                            ManualForm formManual = new ManualForm("Orchestrator", currentAction.Operation.Directive);
                            Thread LThread = new Thread(new ThreadStart(() => LocalMessageHandler(formManual, currentAction.TargetClient.IpAddress)));
                            LThread.Start();
                            string response = Connectivity.GetData(clientSocket);
                            currentAction.Response = _formResult;
                            if (LThread.IsAlive)
                            {
                                currentAction.Response = response;
                                formManual.Invoke(new MethodInvoker(() => formManual.Close()));
                                LThread.Suspend();
                            }    
                        }
                        else
                        {
                            currentAction.Response = Connectivity.GetData(clientSocket);
                        }
                        Console.WriteLine(currentAction.Response);
                        clientSocket.Close();                     
                    }
                }
            }
        }

        public void SendFilesHandler(IList<TestAction> actions)
        {           
            foreach (var action in actions)
            {
                Process.CompressFolder(Path.Combine(Constants.UnzippedScriptFolderOrchestrator, action.File), Constants.ZippedScriptFolder , true);
                if (Connectivity.SendFileWithMetadata(action.TargetClient.IpAddress, Path.Combine(Constants.ZippedScriptFolder, action.File + ".zip")))
                {
                    action.FileState = FileState.Ready;
                }
            } 
        }

        private void InitializeSteps()
        {
            testCase1 = new TestCase();
            Client My_PC = new Client(IPAddress.Loopback, "My PC");
            Step tc1step1 = new Step();

            Operation o_ac1st1 = new Operation("reusableScriptTest1.sikuli", TestTechnology.Sikuli);
            Operation o_ac2st1 = new Operation("do!", TestTechnology.Human);
            Operation o_ac3st1 = new Operation("reusableScriptTest2.sikuli", TestTechnology.Sikuli);

            TestAction step1action1 = new TestAction(My_PC, o_ac1st1, true);
            TestAction step1action2 = new TestAction(My_PC, o_ac2st1, false);
            TestAction step1action3 = new TestAction(My_PC, o_ac3st1, true);

            tc1step1.Actions.Enqueue(step1action1);
            tc1step1.Actions.Enqueue(step1action2);
            tc1step1.Actions.Enqueue(step1action3);

            testCase1.Steps.Enqueue(tc1step1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitializeSteps();
        }

        void LocalMessageHandler(ManualForm frmMan, IPAddress ip_add)
        {
            frmMan.ShowDialog();
            _formResult = frmMan.Answer + "  Comment: " + frmMan.Comment;
            TcpClient clientSocketManual = new TcpClient();
            clientSocketManual.Connect(ip_add, Constants.MANUAL_ANSWER_PORT);
            Connectivity.SendData(clientSocketManual, "done");
            clientSocketManual.Close();
        }
    }
}







    //        {

    //    private void SendFiles(object sender, EventArgs e)
    //    {
    //        //foreach (var script in scriptsStandingQueue)
    //        //{
    //        //CHECK IF SCRIPTS EXISTS
    //        //    Process.CompressFolder(script);
    //        //}
    //        while (testCase1.Any())
    //        {
    //            //    if (!ScriptIsRunning)
    //            //  {
    //            TcpClient clientSocket = new TcpClient();
    //            TcpClient clientSocketResult = new TcpClient();
    //            clientSocket.Connect(IPAddress.Loopback, Constants.WRITE_SCRIPT_PORT);
    //            Connectivity.SendData(clientSocket, scriptsStandingQueue.Peek());
    //            if (Connectivity.GetData(clientSocket) == Constants.FILENAME_RECEIVED_MSG)
    //            {
    //                Connectivity.SendFile(clientSocket, scriptsStandingQueue.Dequeue());
    //            }
    //            clientSocket.Close();
    //            clientSocketResult.Connect(IPAddress.Loopback, Constants.WRITE_SCRIPT_PORT);
    //            if (Connectivity.GetData(clientSocketResult) == Constants.FILE_RECEIVED_MSG)
    //            {
    //                //SCRIPT READY TO BE RUN
    //            }
    //            clientSocketResult.Close();
    //            // ScriptIsRunning = true;
    //        }
    //        Thread.Sleep(500);
    //    }

    //    private void RunScripts(object sender, EventArgs e)
    //    {
    //        while (scriptsRUNQueue.Any())
    //        {
    //            TcpClient clientSocket = new TcpClient();
    //            clientSocket.Connect(IPAddress.Loopback, Constants.RUN_SCRIPT_PORT);
    //            Connectivity.SendData(clientSocket, scriptsRUNQueue.Dequeue());
    //            labelResult.Text += "    " + Connectivity.GetData(clientSocket);
    //            clientSocket.Close();
    //        }
    //    }
    //}



    //}

