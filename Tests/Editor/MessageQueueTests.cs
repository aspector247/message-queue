using System;
using System.Collections;
using com.spector.CommandQueue;
using com.spector.CommandQueue.Commands;
using com.spector.CommandQueue.Messages;
using com.spector.CommandQueue.Messages.Config;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class MessageQueueTests
{
    private ToastMessage _toastMessage;
    
    private string jsonToast =
        "{ \"Name\": \"toast\", \"Title\": \"my title\", \"Description\": \"my description\", \"IconUrl\": \"https://www.4me.com/wp-content/uploads/2018/03/4me-icon-bell.png\" }";

    [SetUp]
    public void Setup()
    {
        _toastMessage = (ToastMessage) MessageFactory.FromJson(jsonToast);
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
            (GameObject)AssetDatabase.LoadAssetAtPath("Packages/com.spector.messagequeue/Samples/Toast/Prefabs/ToastView.prefab", typeof(GameObject));
        
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
}
