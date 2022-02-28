using System;
using System.Collections;
using System.Collections.Generic;
using com.spector.CommandQueue;
using com.spector.CommandQueue.Commands;
using com.spector.CommandQueue.Messages;
using com.spector.CommandQueue.Messages.Config;
using NUnit.Framework;
using Samples.Scripts.Serializer;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class MessageQueueTests
{
    private ToastMessage _toastMessage;
    
    private string jsonToast =
        "{ \"Name\": \"toast\", \"Title\": \"my title\", \"Description\": \"my description\", \"IconUrl\": \"https://www.4me.com/wp-content/uploads/2018/03/4me-icon-bell.png\" }";
    private MessageQueue _messageQueue;
    private MessageQueueConfig _messageQueueConfig;
    private MessageViewConfig _messageViewConfig;
    
    [SetUp]
    public void Setup()
    {
        _toastMessage = JsonConvert.DeserializeObject<ToastMessage>(jsonToast);
        
        // attach message queue to game object
        GameObject gameObject = Activator.CreateInstance<GameObject>();
        _messageQueue = gameObject.AddComponent<MessageQueue>();
        
        // create a message view config to populate with test data
        _messageViewConfig = ScriptableObject.CreateInstance<MessageViewConfig>();
        
        // use an existing toast model locator which is used as a reference for deserializing JSON
        _messageViewConfig.prefab =
            (GameObject)AssetDatabase.LoadAssetAtPath("Packages/com.spector.messagequeue/Tests/Editor/Prefabs/ToastView.prefab", typeof(GameObject));
        
        // create global message queue config for default control
        _messageQueueConfig = ScriptableObject.CreateInstance<MessageQueueConfig>();
        _messageQueueConfig.DelayNext = 1;
        _messageQueueConfig.Duration = 2; 
        _messageQueue.messageViewConfigs = new MessageViewConfig[] { _messageViewConfig };
    }

    // A Test behaves as an ordinary method
    [Test]
    public void MessageQueueTest_CreateToastMessageFromJson()
    {
        Assert.AreEqual("toast", _toastMessage.Name, "Name should be toast");
        Assert.AreEqual("my title", _toastMessage.Title, "Title should be my title");
        Assert.AreEqual("my description", _toastMessage.Description, "Description should be my description");
        Assert.AreEqual("https://www.4me.com/wp-content/uploads/2018/03/4me-icon-bell.png", _toastMessage.IconUrl, "url should match");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator MessageQueueTest_ShowToastMessage()
    {
        // attach message queue to game object
        GameObject gameObject = Activator.CreateInstance<GameObject>();
        MessageQueue messageQueue = gameObject.AddComponent<MessageQueue>();
        
        // create a message view config to populate with test data
        var messageViewConfig = ScriptableObject.CreateInstance<MessageViewConfig>();
        
        // use an existing toast model locator which is used as a reference for deserializing JSON
        messageViewConfig.prefab =
            (GameObject)AssetDatabase.LoadAssetAtPath("Packages/com.spector.messagequeue/Tests/Editor/Prefabs/ToastView.prefab", typeof(GameObject));
        
        // create global message queue config for default control
        MessageQueueConfig messageQueueConfig = ScriptableObject.CreateInstance<MessageQueueConfig>();
        messageQueueConfig.DelayNext = 1;
        messageQueueConfig.Duration = 2; 
        messageQueue.messageViewConfigs = new MessageViewConfig[] { messageViewConfig };
        
        // create and enqueue the command
        var command = new ShowMessageCommand(messageQueueConfig, messageViewConfig, _toastMessage);
        messageQueue.Enqueue(command);

        yield return null;
    }

    [Test]
    public void MessageQueueTest_CommandSerialization()
    {
        // create commands to serialize
        ToastMessage mock1Model = new ToastMessage("my mock title 1", "my description 1", "my icon 1");
        ShowMessageCommand mockMessageCommand1 = new ShowMessageCommand(_messageQueueConfig, mock1Model);

        // add a custom serializer
        _messageQueue.QueueSerializer = ScriptableObject.CreateInstance<MessageQueueSerializer>();
        
        // test the command serialization
        var mockCommandData1 = mockMessageCommand1.Serialize();
        
        // create a new message command to restore
        ShowMessageCommand restoredMessageCommand = new ShowMessageCommand();
        restoredMessageCommand.ViewConfig = _messageViewConfig;
        restoredMessageCommand.Deserialize(mockCommandData1);
        Assert.AreEqual(mockMessageCommand1, restoredMessageCommand);
    }

    [UnityTest]
    public IEnumerator MessageQueueTest_MessageQueue_Deserialization()
    {
        // create commands to serialize
        ToastMessage mock1Model = new ToastMessage("my mock title 1", "my description 1", "my icon 1");
        ToastMessage mock2Model = new ToastMessage("my mock title 2", "my description 2", "my icon 2");

        // attach message queue to game object
        GameObject gameObject = Activator.CreateInstance<GameObject>();
        MessageQueue messageQueue = gameObject.AddComponent<MessageQueue>();
        
        // create a message view config to populate with test data
        MessageViewConfig messageViewConfig = ScriptableObject.CreateInstance<MessageViewConfig>();
        
        // use an existing toast model locator which is used as a reference for deserializing JSON
        messageViewConfig.prefab =
            (GameObject)AssetDatabase.LoadAssetAtPath("Packages/com.spector.messagequeue/Tests/Editor/Prefabs/ToastView.prefab", typeof(GameObject));
        
        // create global message queue config for default control
        MessageQueueConfig messageQueueConfig = ScriptableObject.CreateInstance<MessageQueueConfig>();
        messageQueueConfig.DelayNext = 1;
        messageQueueConfig.Duration = 2;
        
        // assign  view configs
        messageQueue.messageViewConfigs = new MessageViewConfig[] { messageViewConfig };
        
        // add a custom serializer
        messageQueue.QueueSerializer = ScriptableObject.CreateInstance<MessageQueueSerializer>();
        
        ShowMessageCommand mockMessageCommand1 = new ShowMessageCommand(messageQueueConfig, mock1Model);
        ShowMessageCommand mockMessageCommand2 = new ShowMessageCommand(messageQueueConfig, mock2Model);
        
        messageQueue.Enqueue(mockMessageCommand1);
        messageQueue.Enqueue(mockMessageCommand2);

        // save the current count of the queue to assert against
        int count = messageQueue.Queue.Count;
        // clear the queue so we can use the deserializer
        messageQueue.Clear();
        
        Queue<ICommand> restoredQueue = messageQueue.QueueSerializer.Deserialize();
        Assert.AreEqual(count, restoredQueue.Count, "Queues are not equal");
        yield break;
    }
    
    
}
