namespace KaanDonmez.Prefabs
{
    using System;
    using TMPro;
    using UnityEngine;
    using FirebaseScripts;

    public class MessagePrefab : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI senderText;
        [SerializeField] private TextMeshProUGUI contentText;

        public void Init(Message msg)
        {
            senderText.text = msg.sender;
            contentText.text = msg.content;

            DateTime dt = new DateTime() + TimeSpan.FromTicks(long.Parse(msg.time));
            timeText.text = $"[{dt:dd/MM/yyy mm.ss}]";

        }
    }

}