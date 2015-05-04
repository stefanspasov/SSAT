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
namespace TestEnvironment.Executors
{
    public class ManualExecutor : IExecutor
    {
        string _answer = string.Empty;
        public string Execute(string source)
        {
            ManualForm formManual = new ManualForm() { Text = "Client", Instruction = source };
            TcpListener openSocketForScriptAnswer = new TcpListener(Constants.MANUAL_ANSWER_PORT);
            openSocketForScriptAnswer.Start();
            Thread RMThread = new Thread(new ThreadStart(() => RemoteMessageHandler(openSocketForScriptAnswer)));
            RMThread.Start();
            Thread LThread = new Thread(new ThreadStart(() => LocalMessageHandler(formManual)));
            // 2015-05-04
            // TODO: not sure about this solution (changing the thread to STA)
            // There was a problem with the previous one, when using Form.ShowDialog and 
            // invoking Form.Close, sometimes the dialog cannot be closed for some reasons.
            // The problem right now is that after several manual operations, the Client
            // stops listening to the Orchestrator.
            LThread.SetApartmentState(ApartmentState.STA);
            LThread.Start();
            while (string.IsNullOrEmpty(_answer))
            {
                Thread.Sleep(500);
            }
            if (LThread.IsAlive) {
                formManual.Invoke(new MethodInvoker(() => formManual.Close()));
            }
            if (RMThread.IsAlive)
            {
                openSocketForScriptAnswer.Stop();
                RMThread.Suspend();
            }
            return _answer;
        }

        void RemoteMessageHandler(TcpListener listener)
        {
            TcpClient resultSocket = listener.AcceptTcpClient();
            _answer = Connectivity.GetData(resultSocket);
            resultSocket.Close();
            listener.Stop();
        }

        void LocalMessageHandler(ManualForm frmMan)
        {
            frmMan.ManualAssertionRaised += OnManualAssertionRaised;
            Application.Run(frmMan);
            frmMan.ManualAssertionRaised -= OnManualAssertionRaised;
        }

        private void OnManualAssertionRaised(object sender, EventArgs e) {
            var frmMan = (ManualForm)sender;
            _answer = frmMan.Answer + "  Comment: " + frmMan.Comment;
        }
    }
}
