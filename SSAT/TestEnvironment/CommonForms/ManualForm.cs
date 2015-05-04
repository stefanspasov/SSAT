using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestEnvironment.Entities;

namespace TestEnvironment.CommonForms {
    public partial class ManualForm : Form {
        public string Answer = string.Empty;
        public string Comment = string.Empty;
        private TestAction _testAction;
        public event EventHandler ManualAssertionRaised;

        public string Instruction {
            get { return _instructionTb.Text; }
            set { _instructionTb.Text = value; }
        }

        public TestAction TestAction {
            get { return _testAction; }
            set { _testAction = value; }
        }

        public ManualForm() {
            InitializeComponent();
        }

        private void _passBt_Click(object sender, EventArgs e) {
            Answer = "passed";
            Comment = _commentTb.Text;
            OnManualAssertionRaised(null);
            this.Close();
        }

        private void _failBt_Click(object sender, EventArgs e) {
            Answer = "failed";
            Comment = _commentTb.Text;
            OnManualAssertionRaised(null);
            this.Close();
        }

        private void OnManualAssertionRaised(EventArgs e) {
            if (ManualAssertionRaised != null) {
                ManualAssertionRaised(this, e);
            }
        }
    }
}
