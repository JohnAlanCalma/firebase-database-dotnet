using System;

namespace Firebase.Auth.REST
{
    public class FirebaseAuthEventArgs : EventArgs
    {
        public readonly FirebaseAuth FirebaseAuth;

        public FirebaseAuthEventArgs(FirebaseAuth auth)
        {
            this.FirebaseAuth = auth;
        }
    }
}
