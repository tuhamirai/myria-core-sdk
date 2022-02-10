using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new();
    private NetworkRunner                        _runner;

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0,0,200,40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0,40,200,40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }
    
    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner              = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode            = mode,
            SessionName         = "TestRoom",
            Scene               = SceneManager.GetActiveScene().buildIndex,
            SceneObjectProvider = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });}

    #region NetworkRunnerCallbacks

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // Create a unique position for the player
        Vector3       spawnPosition       = new Vector3((player.RawEncoded%runner.Config.Simulation.DefaultPlayers)*3,1,0);
        NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
        // Keep track of the player avatars so we can remove it when they disconnect
        _spawnedCharacters.Add(player, networkPlayerObject);
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // Find and remove the players avatar
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)                                                      { throw new NotImplementedException(); }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)                             { throw new NotImplementedException(); }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)                                        { throw new NotImplementedException(); }
    public void OnConnectedToServer(NetworkRunner runner)                                                              { throw new NotImplementedException(); }
    public void OnDisconnectedFromServer(NetworkRunner runner)                                                         { throw new NotImplementedException(); }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { throw new NotImplementedException(); }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)         { throw new NotImplementedException(); }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)                            { throw new NotImplementedException(); }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)                              { throw new NotImplementedException(); }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)                  { throw new NotImplementedException(); }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)                { throw new NotImplementedException(); }
    public void OnSceneLoadDone(NetworkRunner runner)                                                                  { throw new NotImplementedException(); }
    public void OnSceneLoadStart(NetworkRunner runner)                                                                 { throw new NotImplementedException(); }

    #endregion
    
}
