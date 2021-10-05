using Mirror;

using UnityEngine;

namespace NetworkGame.Networking
{
    [RequireComponent(typeof(PlayerController))]
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private GameObject enemyToSpawn;
        
        private void Awake()
        {
            // This will run REGARDLESS if we are the local or remote player
        }

        private void Update()
        {
            // First determine if this function is being run on the local player
            if(isLocalPlayer)
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    // Run a function that tells every client to change the colour of this gameObject
                    CmdRandomColor();
                }

                if(Input.GetKeyDown(KeyCode.E))
                {
                    CmdSpawnEnemy();
                }
            }
        }

        [Command]
        public void CmdSpawnEnemy()
        {
            // NetworkServer.Spawn requires an instance of the object in the server's scene to be present
            // so if the object being spawned is a prefab, instantiate needs to be called first
            GameObject newEnemy = Instantiate(enemyToSpawn);
            NetworkServer.Spawn(newEnemy);
        }
        
        // RULES FOR COMMANDS:
        // 1. Cannot return anything
        // 2. Must follow the correct naming convention: The function name MUST start with 'Cmd' exactly like that
        // 3. The function must have the attribute [Command] found in Mirror namespace
        // 4. Can only be certain serializable types (see Command in the documentation)
        [Command]
        public void CmdRandomColor()
        {
            // This is running on the server
            RpcRandomColor(Random.Range(0f, 1f));
        }
        
        // RULES FOR CLIENT RPC:
        // 1. Cannot return anything
        // 2. Must follow the correct naming convention: The function name MUST start with 'Rpc' exactly like that
        // 3. The function must have the attribute [ClientRpc] found in Mirror namespace
        // 4. Can only be certain serializable types (see Command in the documentation)
        [ClientRpc]
        public void RpcRandomColor(float _hue)
        {
            // This is running on every instance of the same object that the client was calling from.
            // i.e. Red GO on Red Client runs Cmd, Red GO on Red, Green and Blue client's run Rpc
            MeshRenderer rend = gameObject.GetComponent<MeshRenderer>();
            rend.material.color = Color.HSVToRGB(_hue, 1, 1);
        }

        // This is run via the network starting and the player connecting...
        // NOT Unity
        // It is run when the object is spawned via the networking system NOT when Unity
        // instantiates the object
        public override void OnStartLocalPlayer()
        {
            // This is run if we are the local player and NOT a remote player
        }

        // This is run via the network starting and the player connecting...
        // NOT Unity
        // It is run when the object is spawned via the networking system NOT when Unity
        // instantiates the object
        public override void OnStartClient()
        {
            // This will run REGARDLESS if we are the local or remote player
            // isLocalPlayer is true if this object is the client's local player otherwise it's false
            PlayerController controller = gameObject.GetComponent<PlayerController>();
            controller.enabled = isLocalPlayer;
        }
    }
}