using System;

namespace com.spector.CommandQueue.Messages
{
    [Serializable]
    public class MessageBase
    {
        public string Name;
        [NonSerialized]
        public Action OnClose;
    }
}