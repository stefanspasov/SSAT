using SATFUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestEnvironment;
using TestEnvironment.CommonForms;
namespace TestEnvironment.Executors {
    public class ManualExecutor : IExecutor {
        string _answer = string.Empty;
        public string Execute(string source) {
            ManualForm formManual = new ManualForm() { Text = "Client", Instruction = source };
            TcpListener listener = new TcpListener(Constants.MANUAL_ANSWER_PORT);
            listener.Start();
            Thread RMThread = new Thread(new ThreadStart(() => RemoteMessageHandler(listener)));
            RMThread.Start();
            formManual.ManualAssertionRaised += OnManualAssertionRaised;
            Application.OpenForms[0].Invoke(new MethodInvoker(() => formManual.Show()));
            while (string.IsNullOrEmpty(_answer)) {
                Thread.Sleep(500);
            }
            if (!formManual.IsDisposed) {
                formManual.Invoke(new MethodInvoker(() => formManual.Close()));
            }
            if (RMThread.IsAlive) {
                listener.Stop();
                RMThread.Suspend();
            }
            return _answer;
        }

        void RemoteMessageHandler(TcpListener listener) {
            TcpClient resultSocket = listener.AcceptTcpClient();
            _answer = Connectivity.GetData(resultSocket);
            resultSocket.Close();
            listener.Stop();
        }

        private void OnManualAssertionRaised(object sender, EventArgs e) {
            var frmMan = (ManualForm)sender;
            _answer = frmMan.Answer + "  Comment: " + frmMan.Comment;
        }

        public void StartUp() { }

        public void ShutDown() { }
    }
}
