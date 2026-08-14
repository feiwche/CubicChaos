using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace BlockBrawl.Duello
{
    public class AuthTest : MonoBehaviour
    {
        private async void Start()
        {
            await SignInAsync();
        }

        private async Task SignInAsync()
        {
            await UnityServices.InitializeAsync();

            if (AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log($"Zaten giris yapilmis. Player ID: {AuthenticationService.Instance.PlayerId}");
                return;
            }

            AuthenticationService.Instance.SignedIn += HandleSignedIn;
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        private void HandleSignedIn()
        {
            Debug.Log($"Anonim giris basarili. Player ID: {AuthenticationService.Instance.PlayerId}");
        }
    }
}
