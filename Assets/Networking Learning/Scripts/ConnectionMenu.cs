using kcp2k;

using Mirror;

using NetworkGame.Networking;

using System;
using System.Net;

using UnityEngine;
using UnityEngine.UI;

namespace NetworkGame
{
	public class ConnectionMenu : MonoBehaviour
	{
		private CustomNetworkManager networkManager;
		private KcpTransport transport;

		[SerializeField] private Button hostButton;
		[SerializeField] private InputField inputField;
		[SerializeField] private Button connectButton;
		
		private void Start()
		{
			networkManager = CustomNetworkManager.Instance;
			transport = Transport.activeTransport as KcpTransport;
			
			hostButton.onClick.AddListener(OnClickHost);
			inputField.onEndEdit.AddListener(OnEndEditAddress);
			connectButton.onClick.AddListener(OnClickConnect);

			CustomNetworkDiscovery discovery = networkManager.discovery;
			discovery.onServerFound.AddListener(OnFoundServer);
			discovery.StartDiscovery();
		}

		private void OnClickHost() => networkManager.StartHost();

		private void OnEndEditAddress(string _value) => networkManager.networkAddress = _value;

		private void OnClickConnect()
		{
			string address = inputField.text;
			ushort port = 7777;
			if(address.Contains(":"))
			{
				string portID = address.Substring(address.IndexOf(":", StringComparison.Ordinal) + 1);
				port = ushort.Parse(portID);
				address = address.Substring(0, address.IndexOf(":", StringComparison.Ordinal));
			}
			
			if(!IPAddress.TryParse(address, out IPAddress _))
			{
				Debug.LogError($"Invalid IP: {address}");
				address = "localhost";
			}

			((KcpTransport)Transport.activeTransport).Port = port;
			networkManager.networkAddress = address;
			networkManager.StartClient();
		}
	}
}