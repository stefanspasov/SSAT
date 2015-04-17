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
            this._runBt = new System.Windows.Forms.Button();
            this.labelResult = new System.Windows.Forms.Label();
            this._reloadBt = new System.Windows.Forms.Button();
            this._resTb = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._testTv = new System.Windows.Forms.TreeView();
            this._stepTv = new System.Windows.Forms.TreeView();
            this._descTb = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _runBt
            // 
            this._runBt.Location = new System.Drawing.Point(12, 12);
            this._runBt.Name = "_runBt";
            this._runBt.Size = new System.Drawing.Size(79, 23);
            this._runBt.TabIndex = 2;
            this._runBt.Text = "Run";
            this._runBt.UseVisualStyleBackColor = true;
            this._runBt.Click += new System.EventHandler(this.OnBtnRunClicked);
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Location = new System.Drawing.Point(24, 183);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(10, 13);
            this.labelResult.TabIndex = 3;
            this.labelResult.Text = " ";
            // 
            // _reloadBt
            // 
            this._reloadBt.Location = new System.Drawing.Point(97, 12);
            this._reloadBt.Name = "_reloadBt";
            this._reloadBt.Size = new System.Drawing.Size(79, 23);
            this._reloadBt.TabIndex = 4;
            this._reloadBt.Text = "Reload";
            this._reloadBt.UseVisualStyleBackColor = true;
            this._reloadBt.Click += new System.EventHandler(this.OnBtnReloadClicked);
            // 
            // _resTb
            // 
            this._resTb.Location = new System.Drawing.Point(3, 294);
            this._resTb.Multiline = true;
            this._resTb.Name = "_resTb";
            this._resTb.Size = new System.Drawing.Size(377, 99);
            this._resTb.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(2, 41);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._testTv);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._descTb);
            this.splitContainer1.Panel2.Controls.Add(this._stepTv);
            this.splitContainer1.Panel2.Controls.Add(this._resTb);
            this.splitContainer1.Size = new System.Drawing.Size(599, 404);
            this.splitContainer1.SplitterDistance = 206;
            this.splitContainer1.TabIndex = 8;
            // 
            // _testTv
            // 
            this._testTv.CheckBoxes = true;
            this._testTv.HideSelection = false;
            this._testTv.Location = new System.Drawing.Point(10, 6);
            this._testTv.Name = "_testTv";
            this._testTv.Size = new System.Drawing.Size(193, 387);
            this._testTv.TabIndex = 6;
            this._testTv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTestCaseSelectionChanged);
            // 
            // _stepTv
            // 
            this._stepTv.Location = new System.Drawing.Point(3, 70);
            this._stepTv.Name = "_stepTv";
            this._stepTv.Size = new System.Drawing.Size(377, 218);
            this._stepTv.TabIndex = 8;
            // 
            // _descTb
            // 
            this._descTb.Enabled = false;
            this._descTb.Location = new System.Drawing.Point(4, 6);
            this._descTb.Multiline = true;
            this._descTb.Name = "_descTb";
            this._descTb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._descTb.Size = new System.Drawing.Size(376, 58);
            this._descTb.TabIndex = 9;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 446);
            this.Controls.Add(this._reloadBt);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this._runBt);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "Orchestrator";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _runBt;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Button _reloadBt;
        private System.Windows.Forms.TextBox _resTb;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView _testTv;
        private System.Windows.Forms.TextBox _descTb;
        private System.Windows.Forms.TreeView _stepTv;
    }
}

