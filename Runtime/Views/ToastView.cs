using com.spector.CommandQueue.Messages;
using com.spector.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.spector.views
{
    public class ToastView : MessageView<ToastMessage>
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        public Image icon;
        
        public override void Show(float duration)
        {
            // hide the icon until we have loaded it
            icon.enabled = false;
            this.gameObject.SetActive(true);
            
            title.text = Model.Title;
            description.text = Model.Description;
            
            // load the image and enable it once loaded or fallback to default if error
            icon.SetImageFromUrlAsync(Model.IconUrl, result =>
            {
                icon.enabled = true;
            });
            
            base.Show(duration);
        }
    }
}