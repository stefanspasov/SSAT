using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestEnvironment;

namespace TestEnvironment.Entities
{
    [Serializable]
    public class TestAction
    {
        string _description;
        Client _targetClient;
        Operation _operation;
        string _response;
        FileState _fileState = FileState.NoFile;
        [XmlIgnore]
        public FileState FileState
        {
            get { return _fileState; }
            set { _fileState = value; }
        }
        [XmlIgnore]
        public string File
        {
            get { return _hasFile ? _operation.Directive : null; }
        }
        [XmlAttribute]
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
        [XmlIgnore]
        public string Response
        {
            get { return _response; }
            set { _response = value; }
        }

        TestStatus _status;
        [XmlIgnore]
        public TestStatus Status {
            get { return _status; }
            set { _status = value; }
        }

        private bool _hasFile;
        [XmlAttribute]
        public bool HasFile {
            get { return _hasFile; }
            set { 
                _hasFile = value;
                _fileState = _hasFile ? FileState.NotReady : FileState.NoFile;
            }
        }
        public TestAction() { }
    }

    public enum FileState
    {
        NotReady, NoFile, Ready
    }
}
