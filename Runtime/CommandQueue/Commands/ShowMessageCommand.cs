using System;
using System.Collections;
using com.spector.CommandQueue.Messages;
using com.spector.CommandQueue.Messages.Config;
using com.spector.Extensions;
using com.spector.views;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

namespace com.spector.CommandQueue.Commands
{
    [Serializable]
    public class ShowMessageCommand : ICommand
    {
        public MessageBase Model;
        public MessageViewConfig ViewConfig;
        public MessageQueueConfig QueueConfig;
        public string FullyQualifiedName => GetType().FullName;
        
        public Action OnFinished { get; set; }

        // default constructor for using with Activator
        public ShowMessageCommand()
        {
            
        }
        
        /// <summary>
        /// Command for showing any type of supported message
        /// </summary>
        /// <param name="queueConfig">Global settings to be used by all messages for delay and on screen duration</param>
        /// <param name="viewConfig">Reference to prefab to instantiate and model class reference</param>
        /// <param name="message">The model that will populate our message</param>  
        public ShowMessageCommand(MessageQueueConfig queueConfig, MessageViewConfig viewConfig, MessageBase message)
        {
            QueueConfig = queueConfig;
            ViewConfig = viewConfig;
            Model = message;
        }
        
        public ShowMessageCommand(MessageQueueConfig queueConfig, MessageBase message)
        {
            QueueConfig = queueConfig;
            Model = message;
        }

        public IEnumerator Execute()
        {
            Model.OnClose += OnClose;

            // save original state of prefab
            if (ViewConfig != null)
            {
                if (ViewConfig.prefab == null)
                {
                    Debug.LogError("ViewConfig doesn't have a prefab.");
                    yield break;
                }
                
                // store starting active state
                var activeSelf = ViewConfig.prefab.activeSelf;
                
                // keep game object inactive when instantiating so it doesn't appear yet
                ViewConfig.prefab.SetActive(false);
                
                // instantiate message prefab
                var go = GameObject.Instantiate(ViewConfig.prefab);
            
                // restore prefab back to original state
                ViewConfig.prefab.SetActive(activeSelf);
            
                // send the message view the model and show it
                ViewBase messageView = go.GetComponent<ViewBase>();
                if (messageView != null)
                {
                    messageView.SetModel(Model);
                    messageView.Show(QueueConfig.Duration);    
                }
            }
            yield return null;
        }

        private void OnClose()
        {
            Model.OnClose -= OnClose;
            // rise the OnFinished event to say we're done with this command
            OnFinished?.Invoke();
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings(){ ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
        
        public ICommand Deserialize(string jsonCmd)
        {
            // deserialize the command string
            JObject cmdObject = JsonConvert.DeserializeObject<JObject>(jsonCmd);
            if (cmdObject != null)
            {
                // grab just the Model
                string modelString = cmdObject["Model"]?.ToString();
                if (!string.IsNullOrEmpty(modelString))
                {
                    // deserialize the model to locate the class name
                    JObject jObject = JsonConvert.DeserializeObject<JObject>(modelString);
                    if (jObject != null)
                    {
                        // use the FullName of the model to recreate it
                        string className = jObject.GetValue("FullyQualifiedName")?.ToString();
                        if (!string.IsNullOrEmpty(className))
                        {
                            Type t = TypeExtensions.GetType(className, true);
                            if (t != null)
                            {
                                // deserialize model to correct type extended from MessageBase
                                Model = (MessageBase)JsonConvert.DeserializeObject(modelString, t);
                            }
    
                            // look for view configs from the message queue
                            var messageQueue = GameObject.FindObjectOfType<MessageQueue>();
                            if (messageQueue != null)
                            {
                                if (Model != null) ViewConfig = messageQueue.GetMessageViewConfig(Model.FullyQualifiedName);
                                QueueConfig = messageQueue.messageQueueConfig;
                            }
                        }
                    }
                }
            }

            return this;
        }

        public bool Equals(ShowMessageCommand other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return Model.Equals(other.Model);
        }

        public bool Equals(ICommand other)
        {
            return Equals((ShowMessageCommand) other);
        }
    }
}