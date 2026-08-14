using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace BlockBrawl.Duello
{
    public class DuelloBoardController : NetworkBehaviour
    {
        [SerializeField] private TMP_Text labelText;

        public override void OnNetworkSpawn()
        {
            UpdateLabel();
        }

        public override void OnGainedOwnership()
        {
            UpdateLabel();
        }

        public override void OnLostOwnership()
        {
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            labelText.text = IsOwner ? "SEN" : "RAKIP";
        }
    }
}
