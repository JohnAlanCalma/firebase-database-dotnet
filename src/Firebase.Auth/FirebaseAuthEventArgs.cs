using System;

namespace FirebaseREST.Auth
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
