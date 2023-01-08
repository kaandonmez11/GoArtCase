namespace KaanDonmez.FirebaseScripts
{
    using System.Threading.Tasks;
    using Firebase;
    using Firebase.Auth;
    using Firebase.Firestore;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UI;

    public class FirebaseManager : MonoBehaviour
    {
        public static FirebaseManager Instance;

        internal FirebaseFirestore db { get; private set; }
        internal User user { get; private set; }
        
        private FirebaseAuth _auth;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("FirebaseManager already in use!");
                Destroy(this);
                return;
            }

            Instance = this;
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    _auth = FirebaseAuth.DefaultInstance;
                    db = FirebaseFirestore.DefaultInstance;
                }
                else
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });

            DontDestroyOnLoad(gameObject);
        }


        public async void Register(string username, string email, string password)
        {
            Debug.Assert(_auth != null);

            if (await CheckUser(username))
            {
                AuthUIManager.Instance.DisplayMessage("Username already taken!", Color.red);
                return;
            }

            string message = "";
            Color color = Color.white;

            await _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    message = "Successfully Registered!";
                    color = Color.green;

                    DocumentReference docRef = db.Collection("users").Document(_auth.CurrentUser.UserId);

                    user = new User() { username = username, email = email };

                    docRef.SetAsync(user);
                    _auth.SignInWithEmailAndPasswordAsync(email, password);
                }
                else
                {
                    var exception = task.Exception.GetBaseException() as FirebaseException;
                    message = $"Failed to register - {(AuthError)exception.ErrorCode}";
                    color = Color.red;
                }
            });

            AuthUIManager.Instance.DisplayMessage(message, color);
            AuthUIManager.Instance.SetEnableAllButtons(true);
        }

        public async void Login(string username, string password)
        {
            Debug.Assert(_auth != null);

            string email = await GetUserEmail(username);
            if (string.IsNullOrEmpty(email))
            {
                AuthUIManager.Instance.DisplayMessage("User cannot be found!", Color.red);
                return;
            }

            user = new User
            {
                email = email,
                username = username
            };

            bool success = false;
            string message = "";
            Color color = Color.white;
            await _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                success = task.Exception == null;

                if (task.Exception == null)
                {
                    message = "Successfully logged in";
                    color = Color.green;
                }
                else
                {
                    var exception = task.Exception.GetBaseException() as FirebaseException;
                    message = $"Failed to login - {(AuthError)exception.ErrorCode}";
                    color = Color.red;
                }
            });

            AuthUIManager.Instance.DisplayMessage(message, color);

            await Task.Delay(1000); // For visible information text

            AuthUIManager.Instance.SetEnableAllButtons(true);
            if (success)
            {
                SceneManager.LoadSceneAsync(1);
            }
        }

        public async void ResetPassword(string email)
        {
            Debug.Assert(_auth != null);

            string message = "";
            Color color = Color.white;

            await _auth.SendPasswordResetEmailAsync(email).ContinueWith(resetTask =>
            {
                if (resetTask.Exception == null)
                {
                    message = "Email sent";
                    color = Color.green;
                }
                else
                {
                    var exception = resetTask.Exception.GetBaseException() as FirebaseException;
                    message = $"Failed to password reset - {(AuthError)exception.ErrorCode}";
                    color = Color.red;
                }
            });

            AuthUIManager.Instance.DisplayMessage(message, color);
        }

        public async Task<string> GetJsonURL()
        {
            Debug.Assert(db != null);
            string url = "";

            await db.Collection("app-settings").GetSnapshotAsync().ContinueWith(task =>
            {
                QuerySnapshot snapshot = task.Result;

                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    var dictionary = doc.ToDictionary();

                    url = dictionary["json-url"] as string;
                }
            });

            return url;
        }

        internal async Task<bool> CheckUser(string username)
        {
            Debug.Assert(db != null);

            bool result = false;

            await db.Collection("users").GetSnapshotAsync().ContinueWith(task =>
            {
                QuerySnapshot snapshot = task.Result;

                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    var dictionary = doc.ToDictionary();

                    if ((string)dictionary["username"] == username)
                    {
                        result = true;
                        break;
                    }
                }
            });

            return result;
        }

        private async Task<string> GetUserEmail(string username)
        {
            Debug.Assert(db != null);

            string email = "";

            await db.Collection("users").GetSnapshotAsync().ContinueWith(task =>
            {
                QuerySnapshot snapshot = task.Result;

                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    var dictionary = doc.ToDictionary();

                    if ((string)dictionary["username"] == username)
                    {
                        email = dictionary["email"] as string;
                        break;
                    }
                }
            });

            return email;
        }

    }
}