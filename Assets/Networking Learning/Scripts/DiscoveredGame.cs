using kcp2k;

using Mirror;

using NetworkGame.Networking;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkGame
{
    [RequireComponent(typeof(Button))]
    public class DiscoveredGame : MonoBehaviour
    {
        [SerializeField] private Text gameInformation;

        private CustomNetworkManager networkManager;
        private DiscoveryResponse response;

        public void Setup(DiscoveryResponse _response)
        {
            networkManager = CustomNetworkManager.Instance;
            UpdateResponse(_response);

            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                // Set the ipAddress to the endpoint address
                networkManager.networkAddress = response.EndPoint.Address.ToString();
                // Change the port to the correct type and assign the port to it
                ((KcpTransport)Transport.activeTransport).Port = Convert.ToUInt16(response.EndPoint.Port);
                // Start the client with the address information
                networkManager.StartClient();
            });
        }

        public void UpdateResponse(DiscoveryResponse _response)
        {
            response = _response;
            // Setup the text to show the ip in bold and the ping in normal
            gameInformation.text = $"<b>{response.EndPoint.Address}</b>";
        }

        // Update is called once per frame
        private void Update()
        {
        
        }
    }
}