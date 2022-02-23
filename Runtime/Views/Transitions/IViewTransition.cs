using System;

namespace com.spector.views.Transitions
{
    public interface IViewTransition
    {
        void Show(Action onShowing = null);
        void Hide(Action onHiding = null);
    }
}