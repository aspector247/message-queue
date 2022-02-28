using System;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace com.spector.CommandQueue.Messages.Config
{
    [CreateAssetMenu(fileName = "Message View Config", menuName = "Message Queue/Message View Config")]
    [Serializable]
    public class MessageViewConfig : ScriptableObject
    {
        [JsonIgnore]
        public GameObject prefab;
    }
}