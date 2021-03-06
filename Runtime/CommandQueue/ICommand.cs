using System;
using System.Collections;

namespace com.spector.CommandQueue
{
    public interface ICommand : IEquatable<ICommand>
    {
        Action OnFinished { get; set; }
        IEnumerator Execute();
        string Serialize();
        ICommand Deserialize(string json);
    }
}