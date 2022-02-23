using System;
using System.Collections;
using com.spector.CommandQueue;
using com.spector.CommandQueue.Commands;
using com.spector.CommandQueue.Messages;
using com.spector.CommandQueue.Messages.Config;
using NUnit.Framework;
using Scriptables.MessageConfigs;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class MessageQueueTests
{
    private ToastMessage _toastMessage;
    //private ModalMessage _modalMessage;
    
    private string jsonToast =
        "{ \"Name\": \"toast\", \"Title\": \"my title\", \"Description\": \"my description\", \"Icon\": \"some url or relative asset\" }";
    private string jsonModal =
        "{ \"Name\": \"modal\", \"Title\": \"my title\", \"Description\": \"my description\", \"Icon\": \"some url or relative asset\" }";
    
    [SetUp]
    public void Setup()
    {
        _toastMessage = (ToastMessage)MessageFactory.FromJson(jsonToast);
        //_modalMessage = (ModalMessage)MessageFactory.FromJson(jsonModal);
    }
    
    // A Test behaves as an ordinary method
    [Test]
    public void MessageQueueTest_CreateToastMessageFromJson()
    {
        Assert.AreEqual("toast", _toastMessage.Name, "Name should be toast");
        Assert.AreEqual("my title", _toastMessage.Title, "Title should be my title");
    }
    
    // [Test]
    // public void MessageQueueTest_CreateModalMessageFromJson()
    // {
    //     Assert.AreEqual("modal", _modalMessage.Name, "Name should be modal");
    //     Assert.AreEqual("my title", _modalMessage.Title, "Title should be my title");
    // }

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
        
        // create a toast label and model
        MessageModelLocator toast = ScriptableObject.CreateInstance<MessageModelLocator>();
        toast.name = "toast";
        var messageBase = (MessageBase) Activator.CreateInstance(toast.modelClass.GetType());
        //messageViewConfig.MessageLabelModel = ScriptableObject
        // assign the ToastView to be used for this message
        //messageViewConfig.MessageBase = ScriptableObject.CreateInstance<ToastMessage>();
        //messageViewConfig.MessageBase.Name = "toast";
        messageViewConfig.messageModelLocator = toast;
        messageViewConfig.prefab =
            (GameObject)AssetDatabase.LoadAssetAtPath("Packages/com.spector.messagequeue/Sample/Prefabs/ToastView.prefab", typeof(GameObject));
        
        // create global message queue config for default control
        MessageQueueConfig messageQueueConfig = ScriptableObject.CreateInstance<MessageQueueConfig>();
        messageQueueConfig.DelayNext = 1;
        messageQueueConfig.Duration = 2; 
        messageQueue.messageViewConfigs = new MessageViewConfig[] { messageViewConfig };
        
        // create and enqueue the command
        var command = new ShowMessageCommand(messageQueueConfig, messageViewConfig, messageBase);
        messageQueue.Enqueue(command);

        yield return null;
    }
}
