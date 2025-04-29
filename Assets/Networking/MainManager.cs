using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Multi
{
    public class MainManager : MonoBehaviour
    {
        public static MainManager Instance { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        [Header("Managers")]
        public RelayManager relayManager;
        public LobbyManager lobbyManager;
        public NetworkManager netMag;
        public SceneManager sceMag;
     //   public ChatManager chatMag;
        public StringManager strMag;

        [Header("User Properties")]
        public UserType userType;

        [Header("Server Properties")]
        public int maxConnectionsAllowed;

        [Header("Player Properties")]
        public string playerName;
        public Player currentPlayer;
        public GamePlayType gamePlayType;
        public bool joinedGame;

        async void Start()
        {
            if(userType == UserType.Player)
            {
                await startUnityServices();
                await startAuthentication();
            }

            if (userType == UserType.Server) sceMag.StartServer();
        }

        public async Task startUnityServices()
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            playerName = UtilityManager.GenerateRandomKey(10, false);
            Debug.Log($"PlayerName: {playerName}");
            initializationOptions.SetProfile(profile: playerName);

            // Initialize Unity Services
            await UnityServices.InitializeAsync(initializationOptions);
        }

        public async Task startAuthentication()
        {
            // SignIn User
            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log($"Signed In. Player :: " +
                    $"ID : {AuthenticationService.Instance.PlayerId}, " +
                    $"Name : {AuthenticationService.Instance.PlayerName}, " +
                    $"Info : {AuthenticationService.Instance.PlayerInfo.Username}");

                currentPlayer = createPlayerInstance();

                LobbyListUI.Instance.refreshLobbiesList();
            };
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        public Player createPlayerInstance()
        {
            return new Player(
                id: AuthenticationService.Instance.PlayerId,
                profile: new PlayerProfile(AuthenticationService.Instance.PlayerName),
                data: new Dictionary<string, PlayerDataObject>() {
                    { StringManager.PlayerName, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, MainManager.Instance.playerName) },
                },
                joined: DateTime.Now,
                lastUpdated: DateTime.Now
            //connectionInfo: null,
            //allocationId: null
            );
        }
    }
}
