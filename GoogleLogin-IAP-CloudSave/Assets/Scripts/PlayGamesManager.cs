using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayGamesManager : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI detailsTxt;
    [SerializeField] private Button loginBtn;
    void Start()
    {
        SignIn();
    }

    private void SignIn(){
        // PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
    }
    
    internal void ProcessAuthentication(SignInStatus status) {
        if (status == SignInStatus.Success) {
            // Continue with Play Games Services
            var playerName = PlayGamesPlatform.Instance.GetUserDisplayName();
            var playerId = PlayGamesPlatform.Instance.GetUserId();
            var playerIngUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
            
            detailsTxt.text = "Signed in as " + playerName + " (" + playerId + ")";
            
        } else{
            detailsTxt.text = "Sign in Failed!!!";
            
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }
}
