namespace KaanDonmez.FirebaseScripts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Firebase.Firestore;
    using UnityEngine;

    public class FirebaseMessageManager : MonoBehaviour
    {
        public static FirebaseMessageManager Instance;
        
        internal Action<Message> onMessagesUpdated { get; set; }
        
        private FirebaseFirestore _db;
        private User _user;
        private Dictionary<string, Message> messages;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("FirebaseManager already in use!");
                Destroy(this);
                return;
            }

            Instance = this;
            _db = FirebaseManager.Instance.db;
            _user = FirebaseManager.Instance.user;
            messages = new Dictionary<string, Message>();

            GetOrCreatePM("kaandonmez");
        }

        
        public async Task SendMessageTo(string targetUsername, string message)
        {
            if (targetUsername == _user.username)
            {
                return;
            }

            if (await FirebaseManager.Instance.CheckUser(targetUsername) == false)
            {
                return;
            }

            string docName = GetDocName(_user.username, targetUsername);

            DocumentReference docRef = _db.Collection("personal-messages").Document(docName).Collection("messages")
                .Document();

            Message msg = new Message
            {
                content = message,
                sender = _user.username,
                time = DateTime.UtcNow.Ticks.ToString()
            };

            await docRef.SetAsync(msg);
        }
        
        public async Task<Dictionary<string, Message>> GetOrCreatePM(string targetUsername)
        {
            if (targetUsername == _user.username)
            {
                return null;
            }

            if (await FirebaseManager.Instance.CheckUser(targetUsername) == false)
            {
                return null;
            }

            messages.Clear();
            string docName = GetDocName(_user.username, targetUsername);

            await _db.Collection("personal-messages").Document(docName).Collection("messages").GetSnapshotAsync()
                .ContinueWith(
                    task =>
                    {
                        Debug.Assert(task.Exception == null);

                        QuerySnapshot snapshot = task.Result;
                        Message msg = new Message();
                        Dictionary<string, object> msgDic = new Dictionary<string, object>();

                        foreach (DocumentSnapshot doc in snapshot.Documents)
                        {
                            messages.Add(doc.Id, CreateMessageFromDocument(doc.ToDictionary()));
                        }
                    });

            _db.Collection("personal-messages").Document(docName).Collection("messages").Listen(OnMessagesUpdated);
            return messages;
        }

        private string GetDocName(string username0, string username1)
        {
            int compareResult = string.CompareOrdinal(username0, username1);
            Debug.Assert(compareResult != 0);
            return compareResult < 0
                ? username0 + "-" + username1
                : username1 + "-" + username0;
        }

        private void OnMessagesUpdated(QuerySnapshot snapshot)
        {
            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                if (!messages.ContainsKey(doc.Id))
                {
                    Message msg = CreateMessageFromDocument(doc.ToDictionary());
                    messages.Add(doc.Id, msg);
                    onMessagesUpdated?.Invoke(msg);
                }
            }
        }

        private Message CreateMessageFromDocument(Dictionary<string, object> dictionary)
        {
            return new Message()
            {
                sender = dictionary["sender"] as string,
                content = dictionary["content"] as string,
                time = dictionary["time"] as string,
            };
        }
    }
}