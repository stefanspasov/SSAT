using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestEnvironment
{
   public class ManualForm : Form
    {
        public string Answer = "";
        public string Comment = "";
        public ManualForm(string title, string task)
        {
            InitializeComponent(title, task);
        }

        private System.ComponentModel.IContainer components = null;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent(string title, string task)
        {
            this.buttonPassed = new System.Windows.Forms.Button();
            this.buttonFailed = new System.Windows.Forms.Button();
            this.textBoxComment = new System.Windows.Forms.TextBox();
            this.labelAction = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonPassed
            // 
            this.buttonPassed.BackColor = System.Drawing.Color.YellowGreen;
            this.buttonPassed.Location = new System.Drawing.Point(12, 122);
            this.buttonPassed.Name = "buttonPassed";
            this.buttonPassed.Size = new System.Drawing.Size(93, 46);
            this.buttonPassed.TabIndex = 0;
            this.buttonPassed.Text = "Passed";
            this.buttonPassed.UseVisualStyleBackColor = false;
            this.buttonPassed.Click += new System.EventHandler(this.buttonP_Click);
            // 
            // buttonFailed
            // 
            this.buttonFailed.BackColor = System.Drawing.Color.Tomato;
            this.buttonFailed.Location = new System.Drawing.Point(111, 122);
            this.buttonFailed.Name = "buttonFailed";
            this.buttonFailed.Size = new System.Drawing.Size(94, 46);
            this.buttonFailed.TabIndex = 1;
            this.buttonFailed.Text = "Failed";
            this.buttonFailed.UseVisualStyleBackColor = false;
            this.buttonFailed.Click += new System.EventHandler(this.buttonF_Click);
            // 
            // textBoxComment
            // 
            this.textBoxComment.Location = new System.Drawing.Point(12, 174);
            this.textBoxComment.Multiline = true;
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Size = new System.Drawing.Size(193, 75);
            this.textBoxComment.TabIndex = 2;
            // 
            // labelAction
            // 
            this.labelAction.AutoSize = true;
            this.labelAction.Location = new System.Drawing.Point(12, 20);
            this.labelAction.Name = "labelAction";
            this.labelAction.Size = new System.Drawing.Size(40, 13);
            this.labelAction.TabIndex = 3;
            this.labelAction.Text = task;
            // 
            // FormManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 261);
            this.ControlBox = false;
            this.Controls.Add(this.labelAction);
            this.Controls.Add(this.textBoxComment);
            this.Controls.Add(this.buttonFailed);
            this.Controls.Add(this.buttonPassed);
            this.Name = "FormManual";
            this.Text = title;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void buttonP_Click(object sender, EventArgs e)
        {
            Answer = "Passed";
            Comment = textBoxComment.Text;
            this.Close();
        }

        private void buttonF_Click(object sender, EventArgs e)
        {
            Answer = "Failed";
            Comment = textBoxComment.Text;
            this.Close();
        }

        public System.Windows.Forms.Button buttonPassed;
        public System.Windows.Forms.Button buttonFailed;
        public System.Windows.Forms.TextBox textBoxComment;
        public System.Windows.Forms.Label labelAction;
    }
}
