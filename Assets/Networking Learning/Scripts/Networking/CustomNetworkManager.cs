using JetBrains.Annotations;

using Mirror;

using System.Collections.Generic;

namespace NetworkGame.Networking
{
	public class CustomNetworkManager : NetworkManager
	{
		/// <summary> A reference to the CustomNetworkManager version of the singleton. </summary>
		public static CustomNetworkManager Instance => singleton as CustomNetworkManager;

		/// <summary> Attempts to find a player using the passed NetID, this can return null. </summary>
		/// <param name="_id"> The NetID of the player that we are trying to find. </param>
		[CanBeNull]
		public static NetworkPlayer FindPlayer(uint _id)
		{
			Instance.players.TryGetValue(_id, out NetworkPlayer player);
			return player;
		}

		/// <summary> Whether or not this NetworkManager is the host. </summary>
		public bool IsHost { get; private set; } = false;

		/// <summary> The dictionary of all connected players using their NetID as the key. </summary>
		private readonly Dictionary<uint, NetworkPlayer> players = new Dictionary<uint, NetworkPlayer>();

		/// <summary>
		/// This is invoked when a host is started.
		/// <para>StartHost has multiple signatures, but they all cause this hook to be called.</para>
		/// </summary>
		public override void OnStartHost()
		{
			IsHost = true;
		}

		/// <summary> This is called when a host is stopped. </summary>
		public override void OnStopHost()
		{
			IsHost = false;
		}
	}
}