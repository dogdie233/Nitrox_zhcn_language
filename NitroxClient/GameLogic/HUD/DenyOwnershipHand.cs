

using UnityEngine;

namespace NitroxClient.GameLogic.HUD
{
    public class DenyOwnershipHand : MonoBehaviour
    {
        void Start()
        {
            // Force the message to go away after a few seconds.
            Destroy(this, 2);
        }

        void Update()
        {
            HandReticle.main.SetInteractText("另一个玩家正在与该对象进行交互。");
            HandReticle.main.SetIcon(HandReticle.IconType.HandDeny, 1f);
        }
    }
}
