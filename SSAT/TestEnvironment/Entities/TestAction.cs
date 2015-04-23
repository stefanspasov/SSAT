using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestEnvironment;

namespace TestEnvironment.Entities
{
   public class TestAction
    {

        string _description;
        Client _targetClient;
        Operation _operation;
        string _response;
        string _file;
        FileState _fileState;

        public FileState FileState
        {
            get { return _fileState; }
            set { _fileState = value; }
        }

        public string File
        {
            get { return _file; }
            set { _file = value; _fileState = FileState.NotReady; }
        } 

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public Operation Operation
        {
            get { return _operation; }
            set { _operation = value; }
        }

        public Client TargetClient
        {
            get { return _targetClient; }
            set { _targetClient = value; }
        }

        public string Response
        {
            get { return _response; }
            set { _response = value; }
        }

        TestStatus _status;
        public TestStatus Status {
            get { return _status; }
            set { _status = value; }
        }

        public TestAction(Client client, Operation operation, bool isThereAFile)
        {
            _operation = operation;
            _targetClient = client;
            if (isThereAFile)
            {
                _fileState = FileState.NotReady;
                _file = operation.Directive;
            }
            else
            {
                _fileState = FileState.NoFile;
            }
        }
    }

    public enum FileState
    {
      NotReady, NoFile, Ready
    }
}
