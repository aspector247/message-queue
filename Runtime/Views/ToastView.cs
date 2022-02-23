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
            this.gameObject.SetActive(true);
            
            title.text = Model.Title;
            description.text = Model.Description;
            icon.SetImageFromUrlAsync(Model.IconUrl);
            
            base.Show(duration);
        }
    }
}