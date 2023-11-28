using System.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class PlayGamesManager : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI detailsTxt;
    [SerializeField] private Button loginBtn;
    
    private string _googlePlayToken;
    private string _googlePlayError;

    public async void Start(){
        await Authenticate();
    }

    public async Task Authenticate(){
        PlayGamesPlatform.Activate();
        await UnityServices.InitializeAsync();
        
        PlayGamesPlatform.Instance.Authenticate(Callback);
    }

    private void Callback(SignInStatus status){
        if (status == SignInStatus.Success){
            PlayGamesPlatform.Instance.RequestServerSideAccess(true, RequestServerAccess);
        }
    }

    private  void RequestServerAccess(string status){
        Debug.Log(status);
        AuthenticateWithUnity();
    }

    private async Task AuthenticateWithUnity(){
        try{
            await AuthenticationService.Instance.SignInWithGoogleAsync(_googlePlayToken);
        }
        catch (AuthenticationException e){
            Debug.LogException(e);
            detailsTxt.text = e.ToString();
            throw;
        }
        catch (RequestFailedException e){
            Debug.LogException(e);
            detailsTxt.text = e.ToString();
            throw;
        }
    }

    private void SignIn(){
        // PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
    }

    private async void ProcessAuthentication(SignInStatus status) {
        if (status == SignInStatus.Success) {
            // Continue with Play Games Services
            var playerName = PlayGamesPlatform.Instance.GetUserDisplayName();
            var playerId = PlayGamesPlatform.Instance.GetUserId();
            var playerIngUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
            
            detailsTxt.text = "Signed in as " + playerName + " (" + playerId + ")";
            
            PlayGamesPlatform.Instance.RequestServerSideAccess(true, code => {
                Debug.Log($"Auth code is {code}");
                detailsTxt.text += code;
                _googlePlayToken = code;
            });
            
        } else{
            detailsTxt.text = "Sign in Failed!!!";
            _googlePlayError = "Sign in failed!!!";
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }

        await AuthenticateWithUnity();
    }
}
