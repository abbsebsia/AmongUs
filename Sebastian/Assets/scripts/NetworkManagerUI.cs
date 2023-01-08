using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode.Transports.UTP;

public class NetworkManagerUI: MonoBehaviour
{

    [SerializeField] private string ipAddress = "";
    private readonly ushort port = 8082;
 
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button setButton;

    [SerializeField] private TextMeshProUGUI ipInput;
    [SerializeField] private TextMeshProUGUI portInput;


    private void setConnection()
    {
       
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
        ipAddress,  // The IP address is a string
        port, // The port number is an unsigned short
        "0.0.0.0"

        );
    }

    private void Awake()
    {
        
        setButton.onClick.AddListener(() =>
        {
            ipAddress = ipInput.text;
            Debug.Log("ip address set to: " + ipAddress);
            setConnection();
        });
        serverBtn.onClick.AddListener(() =>
        {   
            NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() =>
        {

            
            Debug.Log("connected host to: " + ipAddress);
            NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() =>
        {

            Debug.Log("connected client to: " + ipAddress);
            NetworkManager.Singleton.StartClient();
        });
        //Debug.Log(ipInput);
        //Debug.Log(portInput);

    }

}
 