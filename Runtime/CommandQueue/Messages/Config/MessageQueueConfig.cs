using UnityEngine;

namespace com.spector.CommandQueue.Messages
{
    [CreateAssetMenu(fileName = "MessageQueueConfig", menuName = "Message Queue/Create Message Queue Config", order = 0)]
    public class MessageQueueConfig : ScriptableObject
    {
        [Header("Delay in between messages")]
        public float DelayNext;
        
        [Header("On screen duration.")]
        public float Duration;
    }
}