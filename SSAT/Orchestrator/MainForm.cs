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
using System.Configuration;
using System.Collections.Specialized;
using TestEnvironment.CommonForms;
namespace Orchestrator
{
    public partial class MainForm : Form
    {
        private IList<TestCase> _testCases = new List<TestCase>();
        private TestCase _selectedTestCase;
        private TestCase _runningTestCase;
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

        private void OnLoaded(object sender, EventArgs e) {
            LoadTestCases();
        }

        private void LoadTestCases()
        {
            _testCases = new TestAccess().LoadTestCases();

            _testResultLb.Text = "Tests run (0/0) — Tests passed (0/0)";
            _resTb.Text = _descTb.Text = string.Empty;
            _stepTv.Nodes.Clear();

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
                _pb.Value = 0;
                _cancelBt.Enabled = true;
                _reloadBt.Enabled = _runBt.Enabled = false;
                _bw.RunWorkerAsync();
            }
        }

        private void OnBtnCancelClicked(object sender, EventArgs e) {
            if (_bw.WorkerSupportsCancellation == true) {
                _bw.CancelAsync();
            }
        }

        private void RunTestCase(TestCase testCase, BackgroundWorker worker, DoWorkEventArgs e)
        {
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
                            ManualForm formManual = new ManualForm() { 
                                Text = "Orchestrator", 
                                TestAction = currentAction,
                                Instruction = currentAction.Operation.Directive 
                            };
                            this.Invoke(new MethodInvoker(() => ShowManualForm(formManual)));
                            string response = Connectivity.GetData(clientSocket); 
                            if (!formManual.IsDisposed) this.Invoke(new MethodInvoker(() => formManual.Close()));
                            currentAction.Response = response;
                        }
                        else
                        {
                            currentAction.Response = Connectivity.GetData(clientSocket);
                        }
                        
                        clientSocket.Close();

                        // Save responses to the test and the action
                        // Break the operation if the action is failed.
                        testCase.Response += 
                            string.Format("{0} @ {1} : {2}\r\nAction: {3}\r\nResponse: {4}\r\n", 
                                          DateTime.Now, currentAction.TargetClient.Name, 
                                          currentAction.TargetClient.IpAddress, currentAction.Description, 
                                          string.IsNullOrEmpty(currentAction.Response) ? "<no response>" : currentAction.Response);
                        // TODO This is the temporary check for failed actions and should be changed
                        if (currentAction.Response.Contains("failed")) {
                            // TODO make it better
                            var clientCollection = ConfigurationManager.GetSection("clientCollection") as NameValueCollection;
                            foreach (var key in clientCollection.AllKeys) {
                                try {
                                    Operation o3 = new Operation("<stop>", TestTechnology.Sim);
                                    TestAction ta3 = new TestAction { TargetClient = new Client { IpAddress = IPAddress.Parse(clientCollection[key]), Name = "ESTRIP_1" }, Operation = o3, HasFile = false, Description = "Stop the sim" };
                                    TcpClient cs = new TcpClient();
                                    cs.Connect(currentAction.TargetClient.IpAddress, Constants.RUN_SCRIPT_PORT);
                                    Connectivity.SendData(cs, JsonConvert.SerializeObject(o3));
                                    clientSocket.Close();
                                } catch { }
                            }

                            currentAction.Status = TestStatus.Failed;
                            testCase.Status = TestStatus.Failed;
                            worker.ReportProgress(1, testCase.Id);
                            return;
                        } else {
                            currentAction.Status = TestStatus.Passed;
                            worker.ReportProgress(1, testCase.Id);
                        }
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

        private void ShowManualForm(ManualForm frmMan) {
            frmMan.Show();
            frmMan.ManualAssertionRaised += OnManualAssertionRaised;
        }

        private void OnManualAssertionRaised(object sender, EventArgs e) {
            var frmMan = (ManualForm)sender;
            var formResult = frmMan.Answer + "  Comment: " + frmMan.Comment;
            TcpClient clientSocketManual = new TcpClient();
            clientSocketManual.Connect(frmMan.TestAction.TargetClient.IpAddress, Constants.MANUAL_ANSWER_PORT);
            Connectivity.SendData(clientSocketManual, formResult);
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
                                         .Select(a => new TreeNode(a.Description) { 
                                                          Tag = a, 
                                                          ImageKey = GetImgKey(a.Status),
                                                          SelectedImageKey = GetImgKey(a.Status)
                                                      })
                                         .ToArray());
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
                // Reset actions' statuses also
                testCase.Steps.SelectMany(s => s.Actions).ToList()
                    .ForEach(a => { a.Status = TestStatus.NotRun; a.Response = string.Empty; });
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
                    _runningTestCase = testCase;
                    RunTestCase(testCase, worker, e);
                }
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            _cancelBt.Enabled = false;
            _reloadBt.Enabled = _runBt.Enabled = true;
            _testResultLb.Text =
                string.Format("Tests run ({0}/{1}) — Tests passed ({2}/{0}) ",
                    _testCases.Count(t => t.Status != TestStatus.NotRun), _testCases.Count,
                    _testCases.Count(t => t.Status == TestStatus.Passed));
            if ((e.Cancelled == true)) {
                _testResultLb.Text += "— The tests have been cancelled"; 
            } else if (!(e.Error == null)) {
                _testResultLb.Text += "— Some errors occurred during the execution"; 
            }
            _pb.Value = 100;
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            if (!_testCases.Any(t => t.Status != TestStatus.NotRun)) return;
            var testCase = _testCases.FirstOrDefault(t => t.Id == e.UserState);
            if (testCase == null) return;
            _testNodeDict[testCase].ImageKey = _testNodeDict[testCase].SelectedImageKey = GetImgKey(testCase.Status);
            if (_selectedTestCase == testCase) {
                _resTb.Text = testCase.Response;
                foreach (TreeNode node in _stepTv.Nodes) {
                    var action = node.Tag as TestAction;
                    if (action != null) {
                        node.ImageKey = node.SelectedImageKey = GetImgKey(action.Status);
                    }
                }
            }
            // TODO Granularize the progress to steps
            _pb.Value = _testCases.Count(t => t.Status == TestStatus.Passed || t.Status == TestStatus.Failed) * 100
                      / _testCases.Count(t => t.Status != TestStatus.NotRun);
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
