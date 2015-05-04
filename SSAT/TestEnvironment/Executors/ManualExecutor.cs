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
        string _answer = "";
        public string Execute(string source)
        {
            ManualForm formManual = new ManualForm() { Text = "Client", Instruction = source };
            TcpListener openSocketForScriptAnswer = new TcpListener(Constants.MANUAL_ANSWER_PORT);
            openSocketForScriptAnswer.Start();
            Thread RMThread = new Thread(new ThreadStart(() => RemoteMessageHandler(openSocketForScriptAnswer)));
            RMThread.Start();
            Thread LThread = new Thread(new ThreadStart(() => LocalMessageHandler(formManual)));
            LThread.Start();
            while (_answer == "")
            {
                Thread.Sleep(500);
            }
            if (LThread.IsAlive)
            {
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
            frmMan.ShowDialog();
            _answer = frmMan.Answer + "  Comment: " + frmMan.Comment;
        }
    }
}
