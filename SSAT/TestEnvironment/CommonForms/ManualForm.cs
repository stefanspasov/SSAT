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

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void SetFocus(IntPtr hWnd);
        public string Instruction {
            get { return _instructionTb.Text; }
            set { _instructionTb.Text = value; }
        }

        public TestAction TestAction {
            get { return _testAction; }
            set { _testAction = value; }
        }

        public ManualForm() {
            this.TopMost = true;
            Load += ManualForm_Load;
            InitializeComponent();
        }

        void ManualForm_Load(object sender, EventArgs e) {
            BeginInvoke(new MethodInvoker(() => { 
                Focus(); 
                BringToFront(); 
                TopMost = true; 
                SwitchToThisWindow(this.Handle, true); 
                SetFocus(this.Handle); 
            }));
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
