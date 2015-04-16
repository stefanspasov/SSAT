using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SATFUtilities
{
    public static class Connectivity
    {
        public static String GetData(TcpClient clientSocket)
        {
            NetworkStream stream = clientSocket.GetStream();
            byte[] bytesFrom = new byte[65536];
            stream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
            string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
            dataFromClient = dataFromClient.Replace("\0", String.Empty);
            return dataFromClient;
        }

        public static void SendData(TcpClient clientSocket, string data)
        {
            NetworkStream stream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(data);
            stream.Write(outStream, 0, outStream.Length);
            stream.Flush();
        }

        public static void GetFile(TcpClient socketToReadFrom, string fileName) 
        {
            if (!Directory.Exists(Constants.ZippedScriptFolder))
            {
                Directory.CreateDirectory(Constants.ZippedScriptFolder);
            }
            var stream = socketToReadFrom.GetStream();
            using (var output = File.Create(Path.Combine(Constants.ZippedScriptFolder, fileName)))
            {
                var buffer = new byte[256];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, bytesRead);
                }
                output.Close();
            }
        }

        public static void SendFile(TcpClient socketToSendTo, string filePathFull)
        {
           var stream = socketToSendTo.GetStream();
           FileStream fs = new FileStream(filePathFull, FileMode.Open, FileAccess.Read);
           byte[] SendingBuffer = new byte[(int)fs.Length];
           fs.Read(SendingBuffer, 0, (int)fs.Length);
           stream.Write(SendingBuffer, 0, (int)SendingBuffer.Length);
           fs.Close();
           stream.Flush();
        }

        public static bool SendFileWithMetadata(IPAddress ipAddress, string filePathFull)
        {
            TcpClient clientSocket = new TcpClient();
            clientSocket.Connect(ipAddress, Constants.WRITE_SCRIPT_PORT);
            TcpClient clientSocketResult = new TcpClient();

            var fileName = Path.GetFileName(filePathFull);
            Connectivity.SendData(clientSocket, fileName);
            if (Connectivity.GetData(clientSocket) == Constants.FILENAME_RECEIVED_MSG)
            {
                Connectivity.SendFile(clientSocket, filePathFull);
            }
            clientSocket.Close();
            clientSocketResult.Connect(ipAddress, Constants.WRITE_SCRIPT_PORT);
            if (Connectivity.GetData(clientSocketResult) == Constants.FILE_RECEIVED_MSG)
            {
                clientSocketResult.Close();
                return true;
            }
            else
            {
                clientSocketResult.Close();
                return false;
            }          
        }

        public static string WaitForAnswer(int listeningPort)
        {
            TcpListener openSocketForScriptAnswer = new TcpListener(listeningPort);
            openSocketForScriptAnswer.Start();
            TcpClient resultSocket = openSocketForScriptAnswer.AcceptTcpClient();
            String answer = Connectivity.GetData(resultSocket);
            Console.WriteLine(answer);
            resultSocket.Close();
            openSocketForScriptAnswer.Stop();
            return answer;
        }
    }
}
