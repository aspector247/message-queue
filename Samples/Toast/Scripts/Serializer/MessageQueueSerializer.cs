using System.Collections.Generic;
using com.spector.CommandQueue;
using com.spector.CommandQueue.Interfaces;
using UnityEngine;

namespace Samples.Scripts.Serializer
{
    [CreateAssetMenu(fileName = "message queue serializer", menuName = "Message Queue/Message Queue Serializer", order = 1)]
    public class MessageQueueSerializer : ScriptableObject, ICommandQueueSerializer
    {
        public void Serialize(Queue<ICommand> queue)
        {
            var list = queue.ToArray();
            var serializedList = new string[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                serializedList[i] = list[i].Serialize();
            }
        
            Debug.Log($"Saving list: {string.Join(", ", serializedList)}");
            //TODO save this to disk
        }

        public Queue<ICommand> Deserialize()
        {
            //TODO add deserializing
            return null;
        }
    }
}
