# message-queue
Event-driven message queue for displaying things like toast messages. 
The system can also be used to queue up any type of game events if for example you want to move a character.

## Installation
1. Open Unity Package Manager
2. Choose Add package from Git url: https://github.com/aspector247/message-queue.git
3. Find the Message Queue package and rename the folder Samples to Samples~. This is necessary until I create a github action to automatically change this file name.
4. Import Samples folder
5. Open the Sample Scene - you may be prompted to install Text Mesh Pro Essentials that exist in the scene.
6. Click on the Sample Scene to reload it to make sure all the fonts are appearing on the buttons.

## Play

Press play and test multiple taps of button Show Toast 1 and Show Toast 2.

You will noticed that a new message will be queued to show one after the other.

Tap the Show Modal button and you'll see how the system can be extended to support many different types of messages.


## Usage
![alt text](https://github.com/aspector247/message-queue/blob/master/Samples/Toast/Images/message-queue-component.png "Message Queue Component")
In the SampleScene, take a look at the MessageQueue component. To use it you will need to create:
  1. A QueueSerializer - responsible for saving the state of your message queue if app crashes or user closes.
  2. A MessageQueueConfig that is a global setting for determining how long to show the message before hiding and how many seconds to delay the next message. 
  3. A list of MessageViewConfigs that determine which views are supported. In this example we have added a ToastView and a ModalView.
  4. The MessageEventListener component listens to an instance of a ScriptableObject called MessageEvent that is also attached to the UIManager ![alt text](https://github.com/aspector247/message-queue/blob/master/Samples/Toast/Images/ui-manager-component.png "UI Manager") and sends either a ToastMessage or a ModalMessage to the ShowMessage method on the MessageQueue. This just illustrates that you can create and send any type of MessageBase to the MessageQueue.
  5. The ToastMessage can either be created in code or if you are calling a service it can be deserialized into its correct form with an example such as: { "Name": "toast", "Title": "my title", "Description": "my description", "Icon": "some url" }

## Creating a SuperToastView
  1. Create your view similar to the ToastView. Decide if you need a new ToastView class for different functionality.
  2. Attach your ToastView class or newly created SuperToastView to the game object 
  3. If you're going to animate your message on and offscreen there is the flexibility to either attach an AnimatorViewTransition or make your own animations in code by implementing the IViewTransition interface.
  4. Create a MessageViewConfig by right-clicking Create -> Message Queue -> Create Message View Config.
  5. Attach your newly created SuperToastView and SuperToastMessage model to the Message View Config.
  6. Finally add this Message View Config to the MessageViewConfigs list on the MessageQueue object in the scene.

## Further Extending The CommandQueue
The MessageQueue is a subclass of CommandQueue and uses the ShowMessageCommand to perform most of its work. You could create additional systems using different Commands or creating many different Command Queues within the same scene to perform domain specific tasks.
