using System;
using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace BlockBrawl.Duello
{
    public class RelayConnectionManager : MonoBehaviour
    {
        [SerializeField] private UnityTransport transport;
        [SerializeField] private int maxConnections = 1;

        public event Action<string> StatusChanged;
        public event Action<string> JoinCodeCreated;

        private Task signInTask;

        private void Awake()
        {
            signInTask = EnsureSignedInAsync();
        }

        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            }
        }

        private async Task EnsureSignedInAsync()
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            StatusChanged?.Invoke("Giris yapildi, hazir.");
        }

        public async void CreateRelayGame()
        {
            StatusChanged?.Invoke("Mac olusturuluyor...");
            await signInTask;

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            transport.SetRelayServerData(BuildHostRelayServerData(allocation, "dtls"));

            bool started = NetworkManager.Singleton.StartHost();
            StatusChanged?.Invoke(started ? "Host baslatildi, rakip bekleniyor..." : "Host baslatilamadi.");
            JoinCodeCreated?.Invoke(joinCode);
        }

        public async void JoinRelayGame(string joinCode)
        {
            StatusChanged?.Invoke("Maca katiliniyor...");
            await signInTask;

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            transport.SetRelayServerData(BuildClientRelayServerData(joinAllocation, "dtls"));

            bool started = NetworkManager.Singleton.StartClient();
            StatusChanged?.Invoke(started ? "Baglaniliyor..." : "Baglanti baslatilamadi.");
        }

        private void HandleClientConnected(ulong clientId)
        {
            StatusChanged?.Invoke($"Baglanti kuruldu. Client ID: {clientId}");
        }

        private static RelayServerData BuildHostRelayServerData(Allocation allocation, string connectionType)
        {
            RelayServerEndpoint endpoint = allocation.ServerEndpoints.First(e => e.ConnectionType == connectionType);

            return new RelayServerData(
                endpoint.Host,
                (ushort)endpoint.Port,
                allocation.AllocationIdBytes,
                allocation.ConnectionData,
                allocation.ConnectionData,
                allocation.Key,
                endpoint.Secure,
                connectionType == "wss");
        }

        private static RelayServerData BuildClientRelayServerData(JoinAllocation allocation, string connectionType)
        {
            RelayServerEndpoint endpoint = allocation.ServerEndpoints.First(e => e.ConnectionType == connectionType);

            return new RelayServerData(
                endpoint.Host,
                (ushort)endpoint.Port,
                allocation.AllocationIdBytes,
                allocation.ConnectionData,
                allocation.HostConnectionData,
                allocation.Key,
                endpoint.Secure,
                connectionType == "wss");
        }
    }
}
