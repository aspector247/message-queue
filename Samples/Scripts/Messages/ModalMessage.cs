using System;
using com.spector.CommandQueue.Messages;

namespace Sample.Scripts.Messages
{
    public class ModalMessage : ToastMessage
    {
        private Action _onOkay;
        private Action _onCancel;

        public ModalMessage(string title, string description, string iconUrl, Action onOkay = null, Action onCancel = null) : base(title, description, iconUrl)
        {
            Name = "modal";
            _onOkay = onOkay;
            _onCancel = onCancel;
        }
        
        public override string ToString()
        {
            return $"Type: {Name}, Title: {Title}, Description: {Description}";
        }
    }
}