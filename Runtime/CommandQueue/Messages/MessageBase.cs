using System;
using UnityEngine;

namespace com.spector.CommandQueue.Messages
{
    [Serializable]
    public class MessageBase : IEquatable<MessageBase>
    {
        public string Name;
        [NonSerialized]
        public Action OnClose;

        public string FullyQualifiedName => GetType().FullName;
        
        public virtual bool Equals(MessageBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return this.Name.Equals(other.Name);
        }
    }
}