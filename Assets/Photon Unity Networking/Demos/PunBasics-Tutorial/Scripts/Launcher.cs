using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace ExitGames.Demos.DemoAnimator
{
    public class Launcher : Photon.PunBehaviour
    {
        public LoaderAnime loaderAnime;
        public byte Version = 1;
        private bool ConnectInUpdate = true;
        bool isConnecting;
        string _gameVersion = "1";
        private RoomInfo[] _info;

        void Awake()
        {
            loaderAnime.StartLoaderAnimation();
            PhotonNetwork.automaticallySyncScene = true;
        }

        public virtual void Update()
        {
            if (!PhotonNetwork.connected)
            {
                PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
            }

          
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            base.OnPhotonPlayerConnected(newPlayer);
            if (PhotonNetwork.room.PlayerCount >= 2 && PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.LoadLevel(1);
            }
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            loaderAnime.gameObject.SetActive(false);
            if (Application.platform == RuntimePlatform.Android)
            {
                CreatRoom();
            }
        }

        public void CreatRoom()
        {
            PhotonNetwork.CreateRoom("AnimaticsRoom", new RoomOptions() {MaxPlayers = 2}, null);
            loaderAnime.gameObject.SetActive(true);
            loaderAnime.StartLoaderAnimation();
        }

        private void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
            loaderAnime.gameObject.SetActive(true);
            loaderAnime.StartLoaderAnimation();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnDisconnectedFromPhoton()
        {
            loaderAnime.StopLoaderAnimation();
            isConnecting = false;
        }

        public override void OnReceivedRoomListUpdate()
        {
            base.OnReceivedRoomListUpdate();
            if (PhotonNetwork.GetRoomList().Length >= 1)
            {
                JoinRoom(PhotonNetwork.GetRoomList()[0].Name);
            }
        }
    }
}