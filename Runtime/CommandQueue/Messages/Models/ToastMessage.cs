using System;
using UnityEngine;

namespace com.spector.CommandQueue.Messages
{
    public class ToastMessage : MessageBase, IEquatable<ToastMessage>
    {
        public string Title;
        public string Description;
        public string IconUrl; // need to support url
        [NonSerialized]
        public Sprite Icon;
        
        public ToastMessage(string title, string description, string iconUrl)
        {
            Name = "toast";
            Title = title;
            Description = description;
            IconUrl = iconUrl;
        }

        public override bool Equals(MessageBase obj)
        {
            return Equals((ToastMessage) obj);
        }

        public bool Equals(ToastMessage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return Name.Equals(other.Name) &&
                   Title.Equals(other.Title) &&
                   Description.Equals(other.Description) &&
                   IconUrl.Equals(other.IconUrl);
        }

        public override string ToString()
        {
            return $"Type: {Name}, Title: {Title}, Description: {Description}";
        }
    }
}