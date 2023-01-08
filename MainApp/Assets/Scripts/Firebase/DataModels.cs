namespace KaanDonmez.FirebaseScripts
{
    using Firebase.Firestore;

    [FirestoreData]
    public struct User
    {
        [FirestoreProperty] public string username { get; set; }
        [FirestoreProperty] public string email { get; set; }
    }
    
    [FirestoreData]
    public struct Message
    {
        [FirestoreProperty] public string sender { get; set; }
        [FirestoreProperty] public string content { get; set; }
        [FirestoreProperty] public string time { get; set; }
    }
}