using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyDetectPlayer : MonoBehaviour
    {
        //private bool _hasDamaged;
        private bool _playerHasSword = false;
        
        public GameObject Player;
        public GameObject Enemy;

        private PlayerMovement playerMovementScript;

        private void Start()
        {
            if (playerMovementScript == null)
            {
                playerMovementScript = Player.GetComponent<PlayerMovement>();
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3Int enemyPos = Vector3Int.FloorToInt(transform.position);
            Vector3Int playerPos = Vector3Int.FloorToInt(Player.transform.position);

            if (_playerHasSword)
            {
                Enemy.SetActive(false);
            }

            if (playerPos == enemyPos)
            {
                // if (_hasDamaged) return;  
                if (!_playerHasSword)
                {
                    playerMovementScript.StopMovement();
                    Player.SendMessage("ReduceHealth", 1);
                    playerMovementScript.ResetPlayer();
                }
                else
                    Enemy.SetActive(false);
            }
        }

        public void TakeDamage()
        {
            _playerHasSword = true;
        }
    }
}
