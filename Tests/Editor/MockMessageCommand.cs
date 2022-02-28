using System;
using System.Collections;
using com.spector.CommandQueue;
using com.spector.CommandQueue.Messages;
using com.spector.CommandQueue.Messages.Config;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Tests.Editor
{
    public class MockMessageCommand : ICommand, IEquatable<MockMessageCommand>
    {

        private MessageBase _model;
        private MessageQueueConfig _messageQueueConfig;
        
        public MockMessageCommand(MessageQueueConfig messageQueueConfig, MessageBase model)
        {
            _messageQueueConfig = messageQueueConfig;
            _model = model;
        }
        
        public Action OnFinished { get; set; }
        public IEnumerator Execute()
        {
            yield return new WaitForSeconds(_messageQueueConfig.Duration);
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(_model);
        }

        public ICommand Deserialize(string json)
        {
            MessageBase model = JsonConvert.DeserializeObject<MessageBase>(json);
            MockMessageCommand mockMessageCommand = new MockMessageCommand(_messageQueueConfig, model);
            return mockMessageCommand;
        }

        public bool Equals(MockMessageCommand other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_model, other._model) && Equals(_messageQueueConfig, other._messageQueueConfig);
        }

        public bool Equals(ICommand other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return Equals((MockMessageCommand) other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MockMessageCommand) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_model != null ? _model.GetHashCode() : 0) * 397) ^ (_messageQueueConfig != null ? _messageQueueConfig.GetHashCode() : 0);
            }
        }

        public static bool operator ==(MockMessageCommand left, MockMessageCommand right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MockMessageCommand left, MockMessageCommand right)
        {
            return !Equals(left, right);
        }
    }
}