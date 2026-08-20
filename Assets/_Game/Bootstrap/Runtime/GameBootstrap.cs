using Game.Features.Signal;
using Game.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.Bootstrap
{
    [DefaultExecutionOrder(-1000)]
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private SignalDefinition signalDefinition;
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private GameObject presenterPrefab;
        [SerializeField] private string validationSceneName = "ValidationLab";

        private ISignalSaveStore saveStore;
        private SignalSessionState currentState;
        private SignalLabPresenter presenter;
        private InputAction toggleAction;

        public SignalSessionState CurrentState => currentState;

        public bool IsReady { get; private set; }

        public bool HasPresenter => presenter != null;

        public bool PresenterMatchesCurrentState => presenter != null && presenter.LastRenderedIsActive == currentState.IsActive;

        private void Awake()
        {
            if (signalDefinition == null || !signalDefinition.IsValid || inputActions == null || presenterPrefab == null)
            {
                Debug.LogError("Validation bootstrap has incomplete authored dependencies.", this);
                enabled = false;
                return;
            }

            saveStore = new SignalPlayerPrefsSaveStore();
            currentState = LoadInitialState();
            presenter = Instantiate(presenterPrefab).GetComponent<SignalLabPresenter>();
            if (presenter == null)
            {
                Debug.LogError("Validation presenter prefab is missing SignalLabPresenter.", this);
                enabled = false;
                return;
            }

            presenter.Render(currentState);

            toggleAction = inputActions.FindAction("Lab/Toggle", true);
            IsReady = true;
        }

        private void OnEnable()
        {
            if (toggleAction == null)
            {
                return;
            }

            toggleAction.performed += OnTogglePerformed;
            toggleAction.Enable();
        }

        private void Start()
        {
            if (IsReady && !string.IsNullOrWhiteSpace(validationSceneName))
            {
                SceneManager.LoadSceneAsync(validationSceneName, LoadSceneMode.Additive);
            }
        }

        private void OnDisable()
        {
            if (toggleAction == null)
            {
                return;
            }

            toggleAction.performed -= OnTogglePerformed;
            toggleAction.Disable();
        }

        public void HandleToggleAction()
        {
            if (!IsReady)
            {
                return;
            }

            currentState = SignalRules.Toggle(currentState);
            saveStore.Save(signalDefinition.ContentId, SignalSaveMapper.ToSaveData(currentState));
            presenter.Render(currentState);
        }

        private SignalSessionState LoadInitialState()
        {
            if (!saveStore.TryLoad(signalDefinition.ContentId, out var saveData))
            {
                return new SignalSessionState(signalDefinition.ContentId, false);
            }

            return SignalSaveMapper.FromSaveData(saveData, signalDefinition.ContentId);
        }

        private void OnTogglePerformed(InputAction.CallbackContext _)
        {
            HandleToggleAction();
        }
    }
}
