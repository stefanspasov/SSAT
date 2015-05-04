namespace TestEnvironment.CommonForms {
    partial class ManualForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this._instructionTb = new System.Windows.Forms.TextBox();
            this._commentTb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._passBt = new System.Windows.Forms.Button();
            this._failBt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Instruction:";
            // 
            // _instructionTb
            // 
            this._instructionTb.Location = new System.Drawing.Point(16, 30);
            this._instructionTb.Multiline = true;
            this._instructionTb.Name = "_instructionTb";
            this._instructionTb.ReadOnly = true;
            this._instructionTb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._instructionTb.Size = new System.Drawing.Size(486, 90);
            this._instructionTb.TabIndex = 1;
            // 
            // _commentTb
            // 
            this._commentTb.Location = new System.Drawing.Point(16, 141);
            this._commentTb.Multiline = true;
            this._commentTb.Name = "_commentTb";
            this._commentTb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._commentTb.Size = new System.Drawing.Size(486, 90);
            this._commentTb.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Comment:";
            // 
            // _passBt
            // 
            this._passBt.BackColor = System.Drawing.Color.YellowGreen;
            this._passBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._passBt.Location = new System.Drawing.Point(16, 237);
            this._passBt.Name = "_passBt";
            this._passBt.Size = new System.Drawing.Size(240, 55);
            this._passBt.TabIndex = 4;
            this._passBt.Text = "Pass";
            this._passBt.UseVisualStyleBackColor = false;
            this._passBt.Click += new System.EventHandler(this._passBt_Click);
            // 
            // _failBt
            // 
            this._failBt.BackColor = System.Drawing.Color.Tomato;
            this._failBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._failBt.Location = new System.Drawing.Point(262, 237);
            this._failBt.Name = "_failBt";
            this._failBt.Size = new System.Drawing.Size(240, 55);
            this._failBt.TabIndex = 5;
            this._failBt.Text = "Fail";
            this._failBt.UseVisualStyleBackColor = false;
            this._failBt.Click += new System.EventHandler(this._failBt_Click);
            // 
            // ManualForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 304);
            this.Controls.Add(this._failBt);
            this.Controls.Add(this._passBt);
            this.Controls.Add(this._commentTb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._instructionTb);
            this.Controls.Add(this.label1);
            this.Name = "ManualForm";
            this.Text = "ManualWinForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _instructionTb;
        private System.Windows.Forms.TextBox _commentTb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button _passBt;
        private System.Windows.Forms.Button _failBt;
    }
}