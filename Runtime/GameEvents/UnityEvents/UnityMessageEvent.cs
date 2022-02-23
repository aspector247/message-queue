using System;
using com.spector.CommandQueue.Messages;
using UnityEngine.Events;

namespace com.spector.GameEvents
{
    [Serializable] public class UnityMessageEvent : UnityEvent<MessageBase>
    {
        
    }
}