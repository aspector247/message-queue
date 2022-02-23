using System;
using com.spector.GameEvents;
using UnityEngine;

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