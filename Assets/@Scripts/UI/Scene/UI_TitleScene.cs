using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static Define;
using Object = UnityEngine.Object;

public class UI_TitleScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        Slider,
    }

    enum Buttons
    {
        StartButton
    }

    enum Texts
    {
        StartText
    }
    #endregion

    private enum TitleSceneState
	{
		None,
		AssetLoading,
		AssetLoaded,
		ConnectingToServer,
		ConnectedToServer,
		FailedToConnectToServer,
	}

	TitleSceneState _state = TitleSceneState.None;
	TitleSceneState State
	{
		get { return _state; }
		set
		{
			_state = value;
			switch (value)
			{
				case TitleSceneState.None:
					break;
				case TitleSceneState.AssetLoading:
					GetText((int)Texts.StartText).text = $"TODO 로딩중";
					break;
				case TitleSceneState.AssetLoaded:
					GetText((int)Texts.StartText).text = "TODO 로딩 완료";
					break;
				case TitleSceneState.ConnectingToServer:
					GetText((int)Texts.StartText).text = "TODO 서버 접속중";
					break;
				case TitleSceneState.ConnectedToServer:
					GetText((int)Texts.StartText).text = "터치하여 시작하기";
					break;
				case TitleSceneState.FailedToConnectToServer:
					GetText((int)Texts.StartText).text = "TODO 서버 접속 실패";
					break;
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();

		BindObjects(typeof(GameObjects));
		BindTexts(typeof(Texts));
		BindButtons(typeof(Buttons));

		GetButton((int)Buttons.StartButton).gameObject.BindEvent((evt) =>
        {
            Debug.Log("OnClick");
            Managers.Scene.LoadScene(EScene.LobbyScene);
        });

        GetButton((int)Buttons.StartButton).gameObject.SetActive(false);
	}

	protected override void Start()
	{
		base.Start();

		// Load 시작
		State = TitleSceneState.AssetLoading;

		Managers.Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
		{
			GetText((int)Texts.StartText).text = $"TODO 로딩중 : {key} {count}/{totalCount}";

			if (count == totalCount)
			{
				OnAssetLoaded();
			}
		});
	}

	private void OnAssetLoaded()
	{
		State = TitleSceneState.AssetLoaded;
		Managers.Data.Init();

        State = TitleSceneState.ConnectedToServer;

        GetButton((int)Buttons.StartButton).gameObject.SetActive(true);

        //Debug.Log("Connecting To Server");
        //State = TitleSceneState.ConnectingToServer;

        //IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        //IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
        //Managers.Network.GameServer.Connect(endPoint, OnConnectionSuccess, OnConnectionFailed);
    }

	private void OnConnectionSuccess()
	{
		Debug.Log("Connected To Server");
		State = TitleSceneState.ConnectedToServer;

		GetButton((int)Buttons.StartButton).gameObject.SetActive(true);

		StartCoroutine(CoSendTestPackets());
	}

	private void OnConnectionFailed()
	{
		Debug.Log("Failed To Connect To Server");
		State = TitleSceneState.FailedToConnectToServer;
	}

	IEnumerator CoSendTestPackets()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);

			C_Test pkt = new C_Test();
			pkt.Temp = 1;
			Managers.Network.Send(pkt);
		}
	}
}
