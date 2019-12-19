using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UINetwork : MonoBehaviour
{
    GameObject panelKoneksi;
    Button btnHost;
    Button btnJoin;
    Button btnCancel;
    Text txInfo;
    NetworkManager network;

    int status = 0;

    // Use this for initialization
    void Start()
    {
        panelKoneksi = GameObject.Find("Multiplayer");
        panelKoneksi.transform.localPosition = Vector3.zero;
        btnHost = GameObject.Find("Host").GetComponent<Button>();
        btnJoin = GameObject.Find("Client").GetComponent<Button>();
        btnCancel = GameObject.Find("Cancel").GetComponent<Button>();
        txInfo = GameObject.Find("Info").GetComponent<Text>();
        btnHost.onClick.AddListener(StartHostGame);
        btnJoin.onClick.AddListener(StartJoinGame);
        btnCancel.onClick.AddListener(CancelConnection);
        btnCancel.interactable = false;
        network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        txInfo.text = "Info: Server Address " + network.networkAddress + " with port " + network.networkPort;
        string ip = NetworkManager.singleton.networkAddress;
        Debug.Log(ip);
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkClient.active || NetworkServer.active)
        {
            btnHost.interactable = false;
            btnJoin.interactable = false;
            btnCancel.interactable = true;
        }
        else
        {
            btnHost.interactable = true;
            btnJoin.interactable = true;
            btnCancel.interactable = false;
        }
        if (NetworkServer.connections.Count == 2 && status == 0)
        {
            status = 1;
            MulaiPermainan();
        }
        if (ClientScene.ready && !NetworkServer.active && status == 0)
        {
            status = 1;
            MulaiPermainan();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            KembaliKeMenu();
        }

    }

    private void StartHostGame()
    {
        Debug.Log("ketika tombol Host ditekan");
        if (!NetworkServer.active)
        {
            network.StartHost();
        }
        if (NetworkServer.active) txInfo.text = "Info: Menunggu Player lain (Jika Server Aktif)";
    }

    private void StartJoinGame()
    {
        Debug.Log("ketika tombol Join ditekan");
        if (!NetworkClient.active)
        {
            network.StartClient();
            network.client.RegisterHandler(MsgType.Disconnect, ConnectionError);
        }
        if (NetworkClient.active) txInfo.text = "Info: Mencoba mengkoneksikan dengan Server";
    }

    private void CancelConnection()
    {
        Debug.Log("ketika tombol Cancel ditekan");
        if (NetworkServer.active)
            network.StopHost();
        if (NetworkClient.active)
            network.StopClient();
        btnHost.interactable = true;
        btnJoin.interactable = true;
        btnCancel.interactable = false;
        txInfo.text = "Info: Server Address " + network.networkAddress + " with port " + network.networkPort;

    }

    private void ConnectionError(NetworkMessage netMsg)
    {
        //network.StopClient();
        txInfo.text = "Info: Koneksi terputus dari Server";
    }

    public void MulaiPermainan()
    {
        panelKoneksi.transform.localPosition = new Vector3(-1500, 0, 0);
    }

    public void KembaliKeMain()
    {
        network.StopHost();
        //SceneManager.LoadScene("Main");
        Debug.Log("Main");
    }

    public void KembaliKeMenu()
    {
        network.StopHost();
        //SceneManager.LoadScene("Menu");
        Debug.Log("Menu");
    }

}
