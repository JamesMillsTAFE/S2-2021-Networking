using kcp2k;

using NetworkGame.Networking;

using UnityEngine;
using UnityEngine.UI;

namespace NetworkGame
{
	[RequireComponent(typeof(Button))]
	public class DiscoveredGame : MonoBehaviour
	{
		[SerializeField] private Text gameInformation;

		private CustomNetworkManager networkManager;
		private KcpTransport transport;
		private DiscoveryResponse response;

		public void Setup(DiscoveryResponse _response, CustomNetworkManager _networkManager, KcpTransport _transport)
		{
			networkManager = _networkManager;
			transport = _transport;
			UpdateResponse(_response);

			Button button = gameObject.GetComponent<Button>();
			button.onClick.AddListener(() =>
			{
				// Set the ipAddress to the endpoint address
				networkManager.networkAddress = response.EndPoint.Address.ToString();
				// Change the port to the correct type and assign the port to it
				transport.Port = response.port;
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
		private void Update() { }
	}
}