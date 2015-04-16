using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SATFUtilities;
using System.Net;
using Newtonsoft.Json;
using TestEnvironment.Executors;
using TestEnvironment;
using TestEnvironment.Entities;
using System.IO;
using System.Runtime.InteropServices;


namespace Client
{
    class MainProcess
    {

        //DONE ON EXIT
        private static bool ConsoleCtrlCheck(Process.CtrlTypes ctrlType)
        {
            try
            {
                IExecutor executor = ExecutorFactory.Instance().CreateExecutor(TestEnvironment.Entities.TestTechnology.Sikuli);
                executor.Execute("<stop>");
            }
            catch (Exception) { }
            return true;
        }

        static void Main(string[] args)
        {
            Process.HandlerRoutine hr = new Process.HandlerRoutine(ConsoleCtrlCheck);
            GC.KeepAlive(hr);
            Process.SetConsoleCtrlHandler(hr, true);
            Process.StartSikuliServer(Path.Combine(Constants.UnzippedScriptFolderClient, Constants.PYTHON_SERVER_NAME));  
            Thread writeScriptThread = new Thread(new ThreadStart(() => WriteScriptThreadHandler(Constants.WRITE_SCRIPT_PORT)));
            writeScriptThread.Start(); 
            StartFacade();
        }

        public static void WriteScriptThreadHandler(int listeningPort)
        {
            TcpListener listener = new TcpListener(listeningPort);
            TcpClient clientSocket = default(TcpClient);
            TcpClient clientSocketResult = default(TcpClient);
            listener.Start();

            while (true)
            {
                clientSocket = listener.AcceptTcpClient();
                String scriptName = Connectivity.GetData(clientSocket);
                Connectivity.SendData(clientSocket, Constants.FILENAME_RECEIVED_MSG);
                Connectivity.GetFile(clientSocket, scriptName);
                clientSocket.Close();
                clientSocketResult = listener.AcceptTcpClient();
                Process.DecompressFile(Path.Combine(Constants.ZippedScriptFolder, scriptName),  Constants.UnzippedScriptFolderClient);
                Connectivity.SendData(clientSocketResult, Constants.FILE_RECEIVED_MSG);
                clientSocketResult.Close();
            }
        }

        public static void StartFacade()
        {
            TcpListener listener = new TcpListener(Constants.RUN_SCRIPT_PORT);
            listener.Start();
            while (true)
            {
                var client = listener.AcceptTcpClient();
                var operationStr = Connectivity.GetData(client);
                var operation = JsonConvert.DeserializeObject<Operation>(operationStr);
                IExecutor executor = ExecutorFactory.Instance().CreateExecutor(operation.Executor);
                Connectivity.SendData(client, executor.Execute(operation.Directive));
                client.Close();
            }
        }
    }
}
