using System;
using System.Collections;

namespace com.spector.CommandQueue
{
    public interface ICommand
    {
        Action OnFinished { get; set; }
        IEnumerator Execute();
        string Serialize();
        object Deserialize(string json);
    }
}