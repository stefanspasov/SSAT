namespace Orchestrator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._resTb = new System.Windows.Forms.TextBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._saveBt = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._pb = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this._testResultLb = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTestSuiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTestSuiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportTestResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._stepTv = new System.Windows.Forms.TreeView();
            this._descTb = new System.Windows.Forms.TextBox();
            this._testNameTb = new System.Windows.Forms.TextBox();
            this._stepGb = new System.Windows.Forms.GroupBox();
            this._descriptionTb = new System.Windows.Forms.TextBox();
            this._clientDdl = new System.Windows.Forms.ComboBox();
            this._directiveTb = new System.Windows.Forms.TextBox();
            this._directiveFileCb = new System.Windows.Forms.CheckBox();
            this._IsCriticalCb = new System.Windows.Forms.CheckBox();
            this._executorDdl = new System.Windows.Forms.ComboBox();
            this._testTv = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._runBtn = new System.Windows.Forms.Button();
            this._testCaseGb = new System.Windows.Forms.GroupBox();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this._stepGb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this._testCaseGb.SuspendLayout();
            this.SuspendLayout();
            // 
            // _resTb
            // 
            this._resTb.Location = new System.Drawing.Point(793, 27);
            this._resTb.Multiline = true;
            this._resTb.Name = "_resTb";
            this._resTb.ReadOnly = true;
            this._resTb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._resTb.Size = new System.Drawing.Size(450, 443);
            this._resTb.TabIndex = 7;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem1,
            this.deleteToolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(109, 48);
            // 
            // createToolStripMenuItem1
            // 
            this.createToolStripMenuItem1.Name = "createToolStripMenuItem1";
            this.createToolStripMenuItem1.Size = new System.Drawing.Size(108, 22);
            this.createToolStripMenuItem1.Text = "Create";
            this.createToolStripMenuItem1.Click += new System.EventHandler(this.createToolStripMenuItem1_Click);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(108, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem1_Click);
            // 
            // _saveBt
            // 
            this._saveBt.Location = new System.Drawing.Point(721, 51);
            this._saveBt.Name = "_saveBt";
            this._saveBt.Size = new System.Drawing.Size(59, 23);
            this._saveBt.TabIndex = 16;
            this._saveBt.Text = "Save";
            this._saveBt.UseVisualStyleBackColor = true;
            this._saveBt.Click += new System.EventHandler(this._saveBt_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 92);
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.createToolStripMenuItem.Text = "Create";
            this.createToolStripMenuItem.Click += new System.EventHandler(this.createToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
            // 
            // _pb
            // 
            this._pb.Location = new System.Drawing.Point(15, 27);
            this._pb.Name = "_pb";
            this._pb.Size = new System.Drawing.Size(765, 23);
            this._pb.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._pb.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Test Result:";
            // 
            // _testResultLb
            // 
            this._testResultLb.AutoSize = true;
            this._testResultLb.Location = new System.Drawing.Point(84, 61);
            this._testResultLb.Name = "_testResultLb";
            this._testResultLb.Size = new System.Drawing.Size(178, 13);
            this._testResultLb.TabIndex = 12;
            this._testResultLb.Text = "Tests run (0/0) — Tests passed (0/0)";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.runToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1255, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTestSuiteToolStripMenuItem,
            this.openTestSuiteToolStripMenuItem,
            this.exportTestResultsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newTestSuiteToolStripMenuItem
            // 
            this.newTestSuiteToolStripMenuItem.Name = "newTestSuiteToolStripMenuItem";
            this.newTestSuiteToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.newTestSuiteToolStripMenuItem.Text = "New Test Suite";
            this.newTestSuiteToolStripMenuItem.Click += new System.EventHandler(this.newTestSuiteToolStripMenuItem_Click);
            // 
            // openTestSuiteToolStripMenuItem
            // 
            this.openTestSuiteToolStripMenuItem.Name = "openTestSuiteToolStripMenuItem";
            this.openTestSuiteToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.openTestSuiteToolStripMenuItem.Text = "Open Test Suite";
            this.openTestSuiteToolStripMenuItem.Click += new System.EventHandler(this.openTestSuiteToolStripMenuItem_Click);
            // 
            // exportTestResultsToolStripMenuItem
            // 
            this.exportTestResultsToolStripMenuItem.Name = "exportTestResultsToolStripMenuItem";
            this.exportTestResultsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.exportTestResultsToolStripMenuItem.Text = "Export Test Results";
            this.exportTestResultsToolStripMenuItem.Click += new System.EventHandler(this.exportTestResultsToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem1,
            this.reloadToolStripMenuItem,
            this.cancelToolStripMenuItem});
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.runToolStripMenuItem.Text = "Test";
            // 
            // runToolStripMenuItem1
            // 
            this.runToolStripMenuItem1.Name = "runToolStripMenuItem1";
            this.runToolStripMenuItem1.Size = new System.Drawing.Size(110, 22);
            this.runToolStripMenuItem1.Text = "Run";
            this.runToolStripMenuItem1.Click += new System.EventHandler(this.runToolStripMenuItem1_Click);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.cancelToolStripMenuItem.Text = "Cancel";
            this.cancelToolStripMenuItem.Click += new System.EventHandler(this.cancelToolStripMenuItem_Click);
            // 
            // _stepTv
            // 
            this._stepTv.ContextMenuStrip = this.contextMenuStrip1;
            this._stepTv.Location = new System.Drawing.Point(6, 102);
            this._stepTv.Name = "_stepTv";
            this._stepTv.Size = new System.Drawing.Size(441, 134);
            this._stepTv.TabIndex = 8;
            this._stepTv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._stepTv_AfterSelect);
            this._stepTv.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._stepTv_NodeMouseClick);
            // 
            // _descTb
            // 
            this._descTb.Location = new System.Drawing.Point(6, 40);
            this._descTb.Multiline = true;
            this._descTb.Name = "_descTb";
            this._descTb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._descTb.Size = new System.Drawing.Size(441, 56);
            this._descTb.TabIndex = 9;
            // 
            // _testNameTb
            // 
            this._testNameTb.Location = new System.Drawing.Point(6, 14);
            this._testNameTb.Name = "_testNameTb";
            this._testNameTb.Size = new System.Drawing.Size(441, 20);
            this._testNameTb.TabIndex = 18;
            // 
            // _stepGb
            // 
            this._stepGb.Controls.Add(this._descriptionTb);
            this._stepGb.Controls.Add(this._clientDdl);
            this._stepGb.Controls.Add(this._directiveTb);
            this._stepGb.Controls.Add(this._directiveFileCb);
            this._stepGb.Controls.Add(this._IsCriticalCb);
            this._stepGb.Controls.Add(this._executorDdl);
            this._stepGb.Enabled = false;
            this._stepGb.Location = new System.Drawing.Point(1, 248);
            this._stepGb.Name = "_stepGb";
            this._stepGb.Size = new System.Drawing.Size(451, 145);
            this._stepGb.TabIndex = 19;
            this._stepGb.TabStop = false;
            this._stepGb.Text = "Test Step";
            // 
            // _descriptionTb
            // 
            this._descriptionTb.Location = new System.Drawing.Point(6, 46);
            this._descriptionTb.Multiline = true;
            this._descriptionTb.Name = "_descriptionTb";
            this._descriptionTb.Size = new System.Drawing.Size(441, 63);
            this._descriptionTb.TabIndex = 13;
            // 
            // _clientDdl
            // 
            this._clientDdl.FormattingEnabled = true;
            this._clientDdl.Location = new System.Drawing.Point(6, 19);
            this._clientDdl.Name = "_clientDdl";
            this._clientDdl.Size = new System.Drawing.Size(159, 21);
            this._clientDdl.TabIndex = 17;
            this._clientDdl.SelectedIndexChanged += new System.EventHandler(this._clientDdl_SelectedIndexChanged);
            // 
            // _directiveTb
            // 
            this._directiveTb.Location = new System.Drawing.Point(6, 115);
            this._directiveTb.Name = "_directiveTb";
            this._directiveTb.Size = new System.Drawing.Size(374, 20);
            this._directiveTb.TabIndex = 11;
            // 
            // _directiveFileCb
            // 
            this._directiveFileCb.AutoSize = true;
            this._directiveFileCb.Location = new System.Drawing.Point(390, 117);
            this._directiveFileCb.Name = "_directiveFileCb";
            this._directiveFileCb.Size = new System.Drawing.Size(42, 17);
            this._directiveFileCb.TabIndex = 12;
            this._directiveFileCb.Text = "File";
            this._directiveFileCb.UseVisualStyleBackColor = true;
            // 
            // _IsCriticalCb
            // 
            this._IsCriticalCb.AutoSize = true;
            this._IsCriticalCb.Location = new System.Drawing.Point(390, 21);
            this._IsCriticalCb.Name = "_IsCriticalCb";
            this._IsCriticalCb.Size = new System.Drawing.Size(57, 17);
            this._IsCriticalCb.TabIndex = 15;
            this._IsCriticalCb.Text = "Critical";
            this._IsCriticalCb.UseVisualStyleBackColor = true;
            // 
            // _executorDdl
            // 
            this._executorDdl.FormattingEnabled = true;
            this._executorDdl.Location = new System.Drawing.Point(171, 19);
            this._executorDdl.Name = "_executorDdl";
            this._executorDdl.Size = new System.Drawing.Size(159, 21);
            this._executorDdl.TabIndex = 14;
            // 
            // _testTv
            // 
            this._testTv.CheckBoxes = true;
            this._testTv.ContextMenuStrip = this.contextMenuStrip2;
            this._testTv.HideSelection = false;
            this._testTv.Location = new System.Drawing.Point(3, 6);
            this._testTv.Name = "_testTv";
            this._testTv.Size = new System.Drawing.Size(310, 387);
            this._testTv.TabIndex = 6;
            this._testTv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTestCaseSelectionChanged);
            this._testTv.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._testTv_NodeMouseClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 77);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._testTv);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._stepGb);
            this.splitContainer1.Size = new System.Drawing.Size(775, 404);
            this.splitContainer1.SplitterDistance = 316;
            this.splitContainer1.TabIndex = 8;
            // 
            // _runBtn
            // 
            this._runBtn.Location = new System.Drawing.Point(660, 51);
            this._runBtn.Name = "_runBtn";
            this._runBtn.Size = new System.Drawing.Size(59, 23);
            this._runBtn.TabIndex = 17;
            this._runBtn.Text = "Run";
            this._runBtn.UseVisualStyleBackColor = true;
            this._runBtn.Click += new System.EventHandler(this._runBtn_Click);
            // 
            // _testCaseGb
            // 
            this._testCaseGb.Controls.Add(this._descTb);
            this._testCaseGb.Controls.Add(this._testNameTb);
            this._testCaseGb.Controls.Add(this._stepTv);
            this._testCaseGb.Enabled = false;
            this._testCaseGb.Location = new System.Drawing.Point(333, 77);
            this._testCaseGb.Name = "_testCaseGb";
            this._testCaseGb.Size = new System.Drawing.Size(451, 242);
            this._testCaseGb.TabIndex = 20;
            this._testCaseGb.TabStop = false;
            this._testCaseGb.Text = "Test Case";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1255, 502);
            this.Controls.Add(this._testCaseGb);
            this.Controls.Add(this._runBtn);
            this.Controls.Add(this._saveBt);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this._testResultLb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._resTb);
            this.Controls.Add(this._pb);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Orchestrator";
            this.Load += new System.EventHandler(this.OnLoaded);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this._stepGb.ResumeLayout(false);
            this._stepGb.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this._testCaseGb.ResumeLayout(false);
            this._testCaseGb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _resTb;
        private System.Windows.Forms.ProgressBar _pb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label _testResultLb;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.Button _saveBt;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTestSuiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openTestSuiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.TreeView _stepTv;
        private System.Windows.Forms.TextBox _descTb;
        private System.Windows.Forms.TextBox _testNameTb;
        private System.Windows.Forms.GroupBox _stepGb;
        private System.Windows.Forms.TextBox _descriptionTb;
        private System.Windows.Forms.ComboBox _clientDdl;
        private System.Windows.Forms.TextBox _directiveTb;
        private System.Windows.Forms.CheckBox _directiveFileCb;
        private System.Windows.Forms.CheckBox _IsCriticalCb;
        private System.Windows.Forms.ComboBox _executorDdl;
        private System.Windows.Forms.TreeView _testTv;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button _runBtn;
        private System.Windows.Forms.ToolStripMenuItem exportTestResultsToolStripMenuItem;
        private System.Windows.Forms.GroupBox _testCaseGb;
    }
}

