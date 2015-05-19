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
        private string _currentFile = "test-suite.xml"; 
        public MainForm()
        {
            InitializeComponent();

            foreach (var client in Constants.ClientCollection.Keys)
            {
                _clientDdl.Items.Add(client);
            }

            foreach (var tech in Constants.TestTechnologies.Keys)
            {
                _executorDdl.Items.Add(tech);
            }

            cancelToolStripMenuItem.Enabled = false;
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
            if (!File.Exists(_currentFile)) { return; }
            LoadTestCases();
        }

        private void LoadTestCases() {
            if (!File.Exists(_currentFile)) {
                MessageBox.Show("The test suite does not exist.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }
            _testCases = new TestAccess().LoadTestCases(_currentFile);

            UpdateView(_testCases.FirstOrDefault());
        }

        private void UpdateView(TestCase selectedTestCase = null, TestAction selectedAction = null)
        {
            _testResultLb.Text = "Tests run (0/0) — Tests passed (0/0)";
            _resTb.Text = _descTb.Text = string.Empty;
            _testCaseGb.Enabled = _stepGb.Enabled = false;
            _stepTv.Nodes.Clear();

            _testTv.BeginUpdate();
            _testTv.Nodes.Clear();
            _testNodeDict.Clear();
            foreach (var testCase in _testCases)
            {
                var node = new TreeNode(testCase.Name, 2, 2) { Tag = testCase };
                _testNodeDict[testCase] = node;
                _testTv.Nodes.Add(node);
            }
            _testTv.EndUpdate();

            if (selectedTestCase != null)
            {
                _testTv.SelectedNode = _testNodeDict[selectedTestCase];
                BeginInvoke(new MethodInvoker(() =>
                {
                    foreach (TreeNode node in _stepTv.Nodes)
                    {
                        if (node.Tag == selectedAction)
                        {
                            _stepTv.SelectedNode = node;
                            break;
                        }
                    }
                }));
            }
        }

        private void Run() {
            if (_bw.IsBusy != true) {
                ResetTestCaseStatuses();
                _pb.Value = 0;
                cancelToolStripMenuItem.Enabled = true;
                reloadToolStripMenuItem.Enabled = runToolStripMenuItem1.Enabled = _runBtn.Enabled = false;
                _bw.RunWorkerAsync();
            }
        }

        private void RunTestCase(TestCase testCase, BackgroundWorker worker, DoWorkEventArgs e)
        {
            testCase.Status = TestStatus.Running;
            worker.ReportProgress(1, testCase.Id);

            var actions = new Queue<TestAction>(testCase.TestActions);
            var files = actions.Where(a => a.FileState == FileState.NotReady).ToList();
            Thread runScriptThread = new Thread(new ThreadStart(() => SendFilesHandler(files)));  
            runScriptThread.Start();

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
                    if (currentAction.Operation.Executor == "Human")
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
                        string.Format("{0} @ {1} : {2}\r\nAction: {3}\r\nResponse: {4}\r\n\r\n", 
                                        DateTime.Now, currentAction.TargetClient.Name, 
                                        currentAction.TargetClient.IpAddress, currentAction.Description, 
                                        string.IsNullOrEmpty(currentAction.Response) ? "<no response>" : currentAction.Response);
                    // TODO This is the temporary check for failed actions and should be changed
                    if (currentAction.Response.Contains("failed") && currentAction.IsCritical) {
                        currentAction.Status = TestStatus.Failed;
                        testCase.Status = TestStatus.Failed;
                        worker.ReportProgress(1, testCase.Id);
                        return;

                    } else if (currentAction.Response.Contains("failed") && !currentAction.IsCritical)
	                {
                        currentAction.Status = TestStatus.Failed;
                        worker.ReportProgress(1, testCase.Id);
	                }
                    else {
                        currentAction.Status = TestStatus.Passed;
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

        private void OnTestCaseSelectionChanged(object sender, TreeViewEventArgs e) {
            _selectedTestCase = _testTv.SelectedNode.Tag as TestCase;
            _stepGb.Enabled = false;
            if (_selectedTestCase != null) {
                _testNameTb.Text = _selectedTestCase.Name;
                _descTb.Text = _selectedTestCase.Description;
                _resTb.Text = _selectedTestCase.Response;
                _stepTv.BeginUpdate();
                _stepTv.Nodes.Clear();
                _stepTv.Nodes.AddRange(
                    _selectedTestCase.TestActions
                                     .Select(a => new TreeNode(a.Description) {
                                         Tag = a,
                                         ImageKey = GetImgKey(a.Status),
                                         SelectedImageKey = GetImgKey(a.Status)
                                     })
                                     .ToArray());
                _stepTv.EndUpdate();
            }
            _testCaseGb.Enabled = _selectedTestCase != null;
            _descriptionTb.Text = string.Empty;
            _directiveTb.Text = string.Empty;
            _directiveFileCb.Checked = false;
            _clientDdl.SelectedItem = null;
            _IsCriticalCb.Checked = false;
            _executorDdl.SelectedItem = null;
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
                testCase.TestActions.ToList()
                    .ForEach(a => { a.Status = TestStatus.NotRun; a.Response = string.Empty; });
            }
            _testTv.EndUpdate();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;
            var pendings = _testCases.Where(t => t.Status == TestStatus.Pending).ToArray();
            TestEnvironment.Environment.Instance.Setup(pendings);
            try {
                foreach (var testCase in pendings) {
                    if ((worker.CancellationPending == true)) {
                        e.Cancel = true;
                        break;
                    } else {
                        _runningTestCase = testCase;
                        RunTestCase(testCase, worker, e);
                    }
                }
            } finally {
                TestEnvironment.Environment.Instance.TearDown(pendings);
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            cancelToolStripMenuItem.Enabled = false;
            reloadToolStripMenuItem.Enabled = runToolStripMenuItem1.Enabled = _runBtn.Enabled = true;
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

        private void _stepTv_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _stepTv.SelectedNode = e.Node;
            }
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedNodeIndex = -1;
            if (_stepTv.SelectedNode != null)
            {
                selectedNodeIndex = _stepTv.SelectedNode.Index;
            }

            TestAction newAction = new TestAction { TargetClient = new Client(), Operation = new Operation() };

            TreeNode newNode = new TreeNode()
            {
                Tag = newAction,
                ImageKey = GetImgKey(newAction.Status),
                SelectedImageKey = GetImgKey(newAction.Status)
            };
            _stepTv.Nodes.Insert(selectedNodeIndex + 1, newNode);
            _selectedTestCase.TestActions.Insert(selectedNodeIndex + 1, newAction);
            _stepTv.SelectedNode = newNode;
            _clientDdl.SelectedItem = _clientDdl.Items[0];
            _executorDdl.SelectedItem = _executorDdl.Items[0];

            new TestAccess().SaveTestCases(_testCases, _currentFile);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = _stepTv.SelectedNode;
            if (selectedNode == null)
            {
                return;
            }
            _stepTv.Nodes.Remove(selectedNode);
            _selectedTestCase.TestActions.Remove((TestAction)selectedNode.Tag);
            new TestAccess().SaveTestCases(_testCases, _currentFile);
        }

        private void _saveBt_Click(object sender, EventArgs e)
        {
            TestCase testCase = null;
            TestAction selectedAction = null;
            if (_testTv.SelectedNode != null) {
                testCase = (TestCase)_testTv.SelectedNode.Tag;
                testCase.Name = _testNameTb.Text;
                testCase.Description = _descTb.Text;
            }
            if (_stepTv.SelectedNode != null)
            {
                selectedAction = (TestAction)_stepTv.SelectedNode.Tag;
                selectedAction.Description = _descriptionTb.Text;
                selectedAction.Operation.Directive = _directiveTb.Text;
                selectedAction.HasFile = _directiveFileCb.Checked;
                selectedAction.TargetClient.Name = _clientDdl.SelectedItem.ToString();
                selectedAction.IsCritical = _IsCriticalCb.Checked;
                selectedAction.Operation.Executor = _executorDdl.SelectedItem.ToString();
            }

            new TestAccess().SaveTestCases(_testCases, _currentFile);
            UpdateView(testCase, selectedAction);
        }

        private void _stepTv_AfterSelect(object sender, TreeViewEventArgs e) {
            TestAction selectedAction = (TestAction)_stepTv.SelectedNode.Tag;
            _stepGb.Enabled = true;
            _descriptionTb.Text = selectedAction.Description;
            _directiveTb.Text = selectedAction.Operation.Directive;
            _directiveFileCb.Checked = selectedAction.HasFile;
            _clientDdl.SelectedItem = selectedAction.TargetClient.Name;
            _IsCriticalCb.Checked = selectedAction.IsCritical;
            _executorDdl.SelectedItem = selectedAction.Operation.Executor;
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = _stepTv.SelectedNode;
            if (selectedNode == null)
            {
                return;
            }
            int selectedNodeIndex = selectedNode.Index;
            _stepTv.Nodes.RemoveAt(selectedNodeIndex);
            _stepTv.Nodes.Insert(selectedNodeIndex - 1, selectedNode);
            _stepTv.SelectedNode = selectedNode;
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = _stepTv.SelectedNode;
            if (selectedNode == null)
            {
                return;
            }
            int selectedNodeIndex = selectedNode.Index;
            _stepTv.Nodes.RemoveAt(selectedNodeIndex);
            _stepTv.Nodes.Insert(selectedNodeIndex + 1, selectedNode);
            _stepTv.SelectedNode = selectedNode;
        }

        private void _testTv_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _testTv.SelectedNode = e.Node;
            }
        }

        private void createToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int selectedNodeIndex = -1;
            if (_testTv.SelectedNode != null)
            {
                selectedNodeIndex = _testTv.SelectedNode.Index;
            }
            TestCase newTestCase = new TestCase { Id = new Random().Next().ToString() };
            TreeNode newNode = new TreeNode()
            {
                Tag = newTestCase,
                ImageKey = GetImgKey(newTestCase.Status),
                SelectedImageKey = GetImgKey(newTestCase.Status)
            };
            _testTv.Nodes.Insert(selectedNodeIndex + 1, newNode);
            _testCases.Insert(selectedNodeIndex + 1, newTestCase);
            _testTv.SelectedNode = newNode;
            new TestAccess().SaveTestCases(_testCases, _currentFile);
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = _testTv.SelectedNode;
            if (selectedNode == null)
            {
                return;
            }
            _testTv.Nodes.Remove(selectedNode);
            _testCases.Remove((TestCase)selectedNode.Tag);
            new TestAccess().SaveTestCases(_testCases, _currentFile);
        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e) {
            Run();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadTestCases();
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_bw.WorkerSupportsCancellation == true)
            {
                _bw.CancelAsync();
            }
        }

        private void openTestSuiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _currentFile = openFileDialog1.SafeFileName;
                    LoadTestCases();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void newTestSuiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    (new TestAccess()).SaveTestCases(_testCases, Path.GetFileName(sfd.FileName));
                    _currentFile = Path.GetFileName(sfd.FileName);
                    UpdateView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void _runBtn_Click(object sender, EventArgs e) {
            Run();
        }

        private void exportTestResultsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK) {
                try {
                    var sb = new StringBuilder();
                    sb.AppendLine(  "*****************************TEST RESULT*****************************");
                    sb.AppendFormat("Test suite: {0}\r\n", _currentFile);
                    sb.AppendFormat("Run time:   {0}\r\n", DateTime.Now);
                    sb.AppendFormat("Status:     {0}\r\n", _testResultLb.Text);
                    sb.AppendLine(  "*********************************************************************");
                    foreach (var test in _testCases) {
                        sb.AppendFormat("Test name: {0}\r\n", test.Name);
                        sb.AppendFormat("Status: {0}\r\n\r\n", test.Status);
                        sb.AppendLine(test.Response);
                        sb.AppendLine("---------------------------------------------------------------------");
                    }
                    File.WriteAllText(sfd.FileName, sb.ToString());
                } catch (Exception ex) {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}
