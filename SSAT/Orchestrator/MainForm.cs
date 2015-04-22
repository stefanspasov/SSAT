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
        private IList<TestCase> _testCases = new List<TestCase>();
        private TestCase _selectedTestCase;
        private TestCase _runningTestCase;
        private string _formResult = string.Empty;
        private BackgroundWorker _bw = new BackgroundWorker();
        private Dictionary<TestCase, TreeNode> _testNodeDict = new Dictionary<TestCase, TreeNode>();
        public MainForm()
        {
            InitializeComponent();

            _bw.WorkerReportsProgress = true;
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            _bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            _stepTv.ImageList = _testTv.ImageList = new ImageList();
            _testTv.ImageList.Images.Add("pass", Image.FromFile("Resources/Images/pass.png"));
            _testTv.ImageList.Images.Add("fail", Image.FromFile("Resources/Images/fail.png"));
            _testTv.ImageList.Images.Add("circle", Image.FromFile("Resources/Images/circle.png"));
            _testTv.ImageList.Images.Add("play", Image.FromFile("Resources/Images/play.png"));
            _testTv.ImageList.Images.Add("pause", Image.FromFile("Resources/Images/pause.png"));
        }

        private void LoadTestCases()
        {
            _testCases = new TestAccess().LoadTestCases();

            _testTv.BeginUpdate();
            _testTv.Nodes.Clear();
            _testNodeDict.Clear();
            foreach (var testCase in _testCases) {
                var node = new TreeNode(testCase.Name, 2, 2) { Tag = testCase };
                _testNodeDict[testCase] = node;
                _testTv.Nodes.Add(node);
            }
            _testTv.EndUpdate();
        }

        private void OnBtnReloadClicked(object sender, EventArgs e)
        {
            LoadTestCases();
        }

        private void OnBtnRunClicked(object sender, EventArgs e)
        {
            if (_bw.IsBusy != true) {
                ResetTestCaseStatuses();
                _pb.Style = ProgressBarStyle.Marquee;
                _bw.RunWorkerAsync();
            }
        }

        private void OnBtnCancelClicked(object sender, EventArgs e) {
            if (_bw.WorkerSupportsCancellation == true) {
                _bw.CancelAsync();
                _pb.Style = ProgressBarStyle.Blocks;
            }
        }

        private void RunTestCase(TestCase testCase, BackgroundWorker worker, DoWorkEventArgs e)
        {
            _runningTestCase = testCase;
            testCase.Status = TestStatus.Running;
            worker.ReportProgress(1, testCase.Id);

            var steps = new Queue<Step>(testCase.Steps);
            var files = steps.SelectMany(t => t.Actions.Where(a => a.FileState == FileState.NotReady)).ToList();
            Thread runScriptThread = new Thread(new ThreadStart(() => SendFilesHandler(files)));  
            runScriptThread.Start();

            while (steps.Any())
            {
                var actions = new Queue<TestAction>(steps.Dequeue().Actions);
                while (actions.Any())
                {
                    if ((worker.CancellationPending == true)) {
                        e.Cancel = true;
                        return;
                    }
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
                        
                        // TODO check failed/passed steps
                        testCase.Response = currentAction.Response;
                        if (currentAction.Response.Contains("failed")) {
                            testCase.Status = TestStatus.Failed;
                            worker.ReportProgress(1, testCase.Id);
                            return;
                        }
                        
                        clientSocket.Close();

                        worker.ReportProgress(1, testCase.Id);
                    }
                }
            }
            testCase.Status = TestStatus.Passed;
            worker.ReportProgress(1, testCase.Id);
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
                    _resTb.Text = _selectedTestCase.Response;
                    _stepTv.BeginUpdate();
                    _stepTv.Nodes.Clear();
                    _stepTv.Nodes.AddRange(
                        _selectedTestCase.Steps.SelectMany(s => s.Actions)
                                         .Select(a => new TreeNode(a.Description, 2, 2) { Tag = a }).ToArray());
                    _stepTv.EndUpdate();
                }
            }
        }

        private void ResetTestCaseStatuses() {
            _testTv.BeginUpdate();
            foreach (var testCase in _testCases) {
                testCase.Response = _resTb.Text = string.Empty;
                if (_testNodeDict[testCase].Checked) {
                    testCase.Status = TestStatus.Pending;
                } else {
                    testCase.Status = TestStatus.NotRun;
                }
                _testNodeDict[testCase].ImageKey = _testNodeDict[testCase].SelectedImageKey = GetImgKey(testCase.Status);
            }
            _testTv.EndUpdate();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;
            var pendings = _testCases.Where(t => t.Status == TestStatus.Pending).ToArray();
            // TODO Check the environment setup (with many consecutive runs)
            TestEnvironment.Environment.Instance.Setup(pendings);
            foreach (var testCase in pendings) {
                if ((worker.CancellationPending == true)) {
                    e.Cancel = true;
                    break;
                } else {
                    RunTestCase(testCase, worker, e);
                }
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if ((e.Cancelled == true)) {
                //this.tbProgress.Text = "Canceled!";
            } else if (!(e.Error == null)) {
                //this.tbProgress.Text = ("Error: " + e.Error.Message);
            } else {
                //this.tbProgress.Text = "Done!";
            }
            _pb.Style = ProgressBarStyle.Blocks;
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            var testCase = _testCases.First(t => t.Id == e.UserState);
            _testNodeDict[testCase].ImageKey = _testNodeDict[testCase].SelectedImageKey = GetImgKey(testCase.Status);
            if (_selectedTestCase == _runningTestCase) {
                _resTb.Text = testCase.Response;
            }
            //this.tbProgress.Text = (e.ProgressPercentage.ToString() + "%");
        }

        private static string GetImgKey(TestStatus status) {
            switch (status) {
                case TestStatus.NotRun: return "circle";
                case TestStatus.Pending: return "pause";
                case TestStatus.Running: return "play";
                case TestStatus.Passed: return "pass";
                case TestStatus.Failed: return "fail";
                default: return "circle";
            }
        }

        private void OnSplitterMoved(object sender, SplitterEventArgs e) {
            
        }
    }
}
