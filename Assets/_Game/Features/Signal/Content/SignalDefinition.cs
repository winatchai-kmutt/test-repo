using Game.Shared;
using UnityEngine;

namespace Game.Features.Signal
{
    [CreateAssetMenu(menuName = "Validation/Signal Definition", fileName = "SignalDefinition")]
    public sealed class SignalDefinition : ScriptableObject
    {
        [SerializeField] private string contentId;
        [SerializeField] private string displayName;

        public ContentId ContentId => new(contentId);

        public string DisplayName => displayName;

        public bool IsValid => ContentId.IsValid && !string.IsNullOrWhiteSpace(DisplayName);
    }
}
