using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestEnvironment.CommonForms {
    public partial class ManualForm : Form {
        public string Answer = string.Empty;
        public string Comment = string.Empty;
        private string instruction;

        public string Instruction {
            get { return _instructionTb.Text; }
            set { _instructionTb.Text = value; }
        }

        public ManualForm() {
            InitializeComponent();
        }

        private void _passBt_Click(object sender, EventArgs e) {
            Answer = "passed";
            Comment = _commentTb.Text;
            this.Close();
        }

        private void _failBt_Click(object sender, EventArgs e) {
            Answer = "failed";
            Comment = _commentTb.Text;
            this.Close();
        }
    }
}
