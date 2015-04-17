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
        IList<TestCase> _testCases = new List<TestCase>();
        TestCase _selectedTestCase;
        string _formResult = string.Empty;
        public MainForm()
        {
            InitializeComponent();
        }

        private void LoadTestCases()
        {
            _testCases = new TestAccess().LoadTestCases();

            _testTv.BeginUpdate();
            _testTv.Nodes.Clear();
            _testTv.Nodes.AddRange(_testCases.Select(t => new TreeNode(t.Name) { Tag = t }).ToArray());
            _testTv.EndUpdate();
        }

        private void OnBtnReloadClicked(object sender, EventArgs e)
        {
            LoadTestCases();
        }

        private void OnBtnRunClicked(object sender, EventArgs e)
        {
            // TODO Check
            TestEnvironment.Environment.Instance.Setup(_testCases);
            foreach (var testCase in _testCases)
            {
                RunTestCase(testCase);
            }
        }

        private void RunTestCase(TestCase testCase)
        {
            var steps = new Queue<Step>(testCase.Steps);
            var files = steps.SelectMany(t => t.Actions.Where(a => a.FileState == FileState.NotReady)).ToList();
            Thread runScriptThread = new Thread(new ThreadStart(() => SendFilesHandler(files)));  
            runScriptThread.Start();

            while (steps.Any())
            {
                var actions = new Queue<TestAction>(steps.Dequeue().Actions);
                while (actions.Any())
                {
                    TestAction currentAction = actions.Peek();
                    if (currentAction.FileState == FileState.Ready || currentAction.FileState == FileState.NoFile)
                    {
                        currentAction = actions.Dequeue();
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
                        // TODO
                        _resTb.Text = currentAction.Response;
                        clientSocket.Close();                     
                    }
                }
            }
        }

        private void SendFilesHandler(IList<TestAction> actions)
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

        private void LocalMessageHandler(ManualForm frmMan, IPAddress ip_add)
        {
            frmMan.ShowDialog();
            _formResult = frmMan.Answer + "  Comment: " + frmMan.Comment;
            TcpClient clientSocketManual = new TcpClient();
            clientSocketManual.Connect(ip_add, Constants.MANUAL_ANSWER_PORT);
            Connectivity.SendData(clientSocketManual, "done");
            clientSocketManual.Close();
        }

        private void OnTestCaseSelectionChanged(object sender, TreeViewEventArgs e)
        {
            if (_selectedTestCase != _testTv.SelectedNode.Tag)
            {
                _selectedTestCase = _testTv.SelectedNode.Tag as TestCase;
                if (_selectedTestCase != null)
                {
                    _descTb.Text = _selectedTestCase.Description;
                    _stepTv.BeginUpdate();
                    _stepTv.Nodes.Clear();
                    _stepTv.Nodes.AddRange(
                        _selectedTestCase.Steps.SelectMany(s => s.Actions)
                                         .Select(a => new TreeNode(a.Description) { Tag = a }).ToArray());
                    _stepTv.EndUpdate();
                }
            }
        }
    }
}
