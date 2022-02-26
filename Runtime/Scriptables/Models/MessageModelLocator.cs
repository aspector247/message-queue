using com.spector.Attributes;
using com.spector.CommandQueue.Messages;
using UnityEngine;

namespace Scriptables.MessageConfigs
{
    [CreateAssetMenu(fileName = "label", menuName = "Message Queue/New Message Label", order = 0)]
    public class MessageModelLocator : ScriptableObject
    {
        public MessageBase modelClass;
        [SerializeField]
        [RequireInterface(typeof(MessageBase))]
        private object objectClass;
        public MessageBase myclass => objectClass as MessageBase;
    }
}