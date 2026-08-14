using Unity.Netcode;
using UnityEngine;

namespace BlockBrawl.Duello
{
    public class DuelloSessionManager : NetworkBehaviour
    {
        [SerializeField] private NetworkObject boardASlot;
        [SerializeField] private NetworkObject boardBSlot;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                return;
            }

            NetworkManager.OnClientConnectedCallback += HandleClientConnected;
            AssignNextSlot(NetworkManager.LocalClientId);
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer && NetworkManager != null)
            {
                NetworkManager.OnClientConnectedCallback -= HandleClientConnected;
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            if (clientId == NetworkManager.LocalClientId)
            {
                return;
            }

            AssignNextSlot(clientId);
        }

        private void AssignNextSlot(ulong clientId)
        {
            if (boardASlot.OwnerClientId == NetworkManager.ServerClientId)
            {
                boardASlot.ChangeOwnership(clientId);
            }
            else if (boardBSlot.OwnerClientId == NetworkManager.ServerClientId)
            {
                boardBSlot.ChangeOwnership(clientId);
            }
        }
    }
}
