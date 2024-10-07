using Firebase.Auth.Providers;
using Firebase.Auth;
using System.Threading.Tasks;

namespace MoneyContribution.Services
{
    public static class FirebaseAuthServices
    {
        private static FirebaseAuthClient _authClient;

        // Method to ensure the AuthClient is initialized
        public static FirebaseAuthClient AuthClient
        {
            get
            {
                if (_authClient == null)
                {
                    InitializeAuthClient();
                }
                return _authClient;
            }
        }

        // Initialization logic for FirebaseAuthClient
        private static void InitializeAuthClient()
        {
            _authClient = new FirebaseAuthClient(new FirebaseAuthConfig
            {
                ApiKey = ContribAPIs.ApiKey,
                AuthDomain = ContribAPIs.AuthDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider(),
                    new EmailProvider()
                }
            });
        }

        public static async Task<UserCredential> SignInUserAsync(string email, string password)
        {
            var _authClient = FirebaseAuthServices.AuthClient; // Ensure it's initialized
            var result = await _authClient.SignInWithEmailAndPasswordAsync(email, password);
            return result;
        }
        public static async Task SignOutAsync()
        {
            if (_authClient != null)
            {
                _authClient.SignOut();
                _authClient = null; 
            }
        }
    }
}
