using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class AdjacentDetectPlayer : MonoBehaviour
    {
        public GameObject Player;
        public GameObject Arrow;

        private PlayerMovement playerMovementScript;
        private bool _playerHasShield;

        void Start()
        {
            if (playerMovementScript == null)
            {
                playerMovementScript = Player.GetComponent<PlayerMovement>();
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3Int adjacentLeftPos = Vector3Int.FloorToInt(transform.position);
            Vector3Int adjacentDownPos = Vector3Int.FloorToInt(transform.position);
            Vector3Int playerPos = Vector3Int.FloorToInt(Player.transform.position);

            if (playerPos == adjacentLeftPos || playerPos == adjacentDownPos)
            {
                if (!_playerHasShield)
                {
                    Player.SendMessage("ReduceHealth", 1);
                    playerMovementScript.ResetPlayer();
                }
                else
                    Arrow.SetActive(false);
            }
                
        }        
    }
}
