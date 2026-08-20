using Game.Features.Signal;
using UnityEngine;

namespace Game.UI
{
    public sealed class SignalLabPresenter : MonoBehaviour
    {
        [SerializeField] private Renderer indicator;
        [SerializeField] private Color inactiveColor = Color.gray;
        [SerializeField] private Color activeColor = Color.cyan;

        public bool HasRenderedState { get; private set; }

        public bool LastRenderedIsActive { get; private set; }

        public void Render(SignalSessionState state)
        {
            if (indicator == null)
            {
                return;
            }

            indicator.material.color = state.IsActive ? activeColor : inactiveColor;
            LastRenderedIsActive = state.IsActive;
            HasRenderedState = true;
        }
    }
}
