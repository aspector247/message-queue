using System;
using System.Collections;
using com.spector.CommandQueue.Messages;
using com.spector.CommandQueue.Messages.Config;
using com.spector.views;
using UnityEngine;

namespace com.spector.CommandQueue.Commands
{
    public class ShowMessageCommand : ICommand
    {
        private readonly MessageBase _messageModel;
        private readonly MessageViewConfig _messageViewConfig;
        private readonly MessageQueueConfig _messageQueueConfig;
        
        public Action OnFinished { get; set; }

        /// <summary>
        /// Command for showing any type of supported message
        /// </summary>
        /// <param name="messageQueueConfig">Global settings to be used by all messages for delay and on screen duration</param>
        /// <param name="messageViewConfig">Reference to prefab to instantiate and model class reference</param>
        /// <param name="message">The model that will populate our message</param>  
        public ShowMessageCommand(MessageQueueConfig messageQueueConfig, MessageViewConfig messageViewConfig, MessageBase message)
        {
            _messageQueueConfig = messageQueueConfig;
            _messageViewConfig = messageViewConfig;
            _messageModel = message;
        }

        public IEnumerator Execute()
        {
            _messageModel.OnClose += OnClose;

            // save original state of prefab
            var activeSelf = _messageViewConfig.prefab.activeSelf;
            
            // keep game object inactive when instantiating so it doesn't appear yet
            _messageViewConfig.prefab.SetActive(false);
            
            // instantiate message prefab
            var go = GameObject.Instantiate(_messageViewConfig.prefab);
            
            // restore prefab back to original state
            _messageViewConfig.prefab.SetActive(activeSelf);
            
            // send the message view the model and show it
            ViewBase messageView = go.GetComponent<ViewBase>();
            if (messageView != null)
            {
                messageView.SetModel(_messageModel);
                messageView.Show(_messageQueueConfig.Duration);    
            }

            yield return null;
        }

        private void OnClose()
        {
            _messageModel.OnClose -= OnClose;
            // rise the OnFinished event to say we're done with this command
            OnFinished?.Invoke();
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(_messageModel);
        }

        public object Deserialize(string json)
        {
            return JsonUtility.FromJson<ToastMessage>(json);
        }
    }
}