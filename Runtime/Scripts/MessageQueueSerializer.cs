using System;
using System.Collections.Generic;
using com.spector.CommandQueue;
using com.spector.CommandQueue.Interfaces;
using com.spector.Extensions;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
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
            
            var data = JsonConvert.SerializeObject(serializedList);
            PlayerPrefs.SetString("message_queue", data);
        }

        /// <summary>
        /// Returns a queue from storage if it exists
        /// Creates a new queue if nothing found
        /// </summary>
        /// <returns></returns>
        public Queue<ICommand> Deserialize()
        {
            Queue<ICommand> queue = new Queue<ICommand>();

            if (PlayerPrefs.HasKey("message_queue"))
            {
                List<string> list = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("message_queue"));
                
                if (list != null)
                {
                    Debug.Log($"Deserializing queue: {string.Join(",", list)}");
                    for (int i = 0; i < list.Count; i++)
                    {
                        // grab the first serialized command
                        var jsonCmd = list[i];
                        JObject jObject = JsonConvert.DeserializeObject<JObject>(jsonCmd);
                        Type t = GetTypeByFullyQualifiedName(jObject);
                        if (t != null)
                        {
                            // create the command class to use for class specific deserializing
                            ICommand cmd = (ICommand) Activator.CreateInstance(t);
                            cmd.Deserialize(jsonCmd);
                            queue.Enqueue(cmd);
                        }
                    }
                }
            }

            return queue;
        }

        public static Type GetTypeByFullyQualifiedName(JObject jObject)
        {
            if (jObject != null)
            {
                // retrieve command class name from serialized data
                string commandClass = jObject.GetValue("FullyQualifiedName")?.ToString();
                return TypeExtensions.GetType(commandClass, true);
            }

            return null;
        }
    }
}
