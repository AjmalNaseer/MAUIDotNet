using Firebase.Auth;

namespace MoneyContribution;

public partial class FlyoutHeaderControl : StackLayout
{
    private readonly FirebaseAuthClient _firebaseAuthClient;

    public FlyoutHeaderControl(FirebaseAuthClient firebaseAuthClient)
	{
		InitializeComponent();
		_firebaseAuthClient = firebaseAuthClient;
        if (!string.IsNullOrWhiteSpace(_firebaseAuthClient?.User?.Info?.Email))
        {
            lblUserEmail.Text = firebaseAuthClient?.User?.Info?.Email;
            lblUsername.Text = firebaseAuthClient?.User?.Info?.DisplayName;
            lblUserEmail.Text = firebaseAuthClient?.User?.Info?.Email;
        }
    }
}