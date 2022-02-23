using System.Collections.Generic;

namespace com.spector.CommandQueue.Interfaces
{
    /// <summary>
    /// Interface for saving and restoring Commands 
    /// </summary>
    public interface ICommandQueueSerializer
    {
        void Serialize(Queue<ICommand> queue);
        Queue<ICommand> Deserialize();
    }
}