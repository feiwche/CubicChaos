using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBrawl.Duello
{
    public class NetworkTestUI : MonoBehaviour
    {
        [SerializeField] private RelayConnectionManager connectionManager;
        [SerializeField] private Button createButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private InputField joinCodeInputField;
        [SerializeField] private TMP_Text joinCodeDisplayText;
        [SerializeField] private TMP_Text statusText;

        private void Awake()
        {
            createButton.onClick.AddListener(HandleCreateClicked);
            joinButton.onClick.AddListener(HandleJoinClicked);
        }

        private void OnEnable()
        {
            connectionManager.StatusChanged += HandleStatusChanged;
            connectionManager.JoinCodeCreated += HandleJoinCodeCreated;
        }

        private void OnDisable()
        {
            connectionManager.StatusChanged -= HandleStatusChanged;
            connectionManager.JoinCodeCreated -= HandleJoinCodeCreated;
        }

        private void HandleCreateClicked()
        {
            connectionManager.CreateRelayGame();
        }

        private void HandleJoinClicked()
        {
            connectionManager.JoinRelayGame(joinCodeInputField.text);
        }

        private void HandleStatusChanged(string status)
        {
            statusText.text = status;
        }

        private void HandleJoinCodeCreated(string joinCode)
        {
            joinCodeDisplayText.text = $"Kod: {joinCode}";
        }
    }
}
