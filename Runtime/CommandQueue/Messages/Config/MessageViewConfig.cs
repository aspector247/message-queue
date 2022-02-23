using Scriptables.MessageConfigs;
using UnityEngine;

namespace com.spector.CommandQueue.Messages.Config
{
    [CreateAssetMenu(fileName = "Message View Config", menuName = "Message Queue/Create Message View Config")]
    public class MessageViewConfig : ScriptableObject
    {
        public MessageModelLocator messageModelLocator;
        public GameObject prefab;
    }
}