using System;
using UnityEngine;

namespace com.spector.CommandQueue.Messages
{
    public class ToastMessage : MessageBase
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

        public override string ToString()
        {
            return $"Type: {Name}, Title: {Title}, Description: {Description}";
        }
    }
}