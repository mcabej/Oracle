using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyDetectPlayer : MonoBehaviour
    {
        public GameObject Player;
        public GameObject Enemy;

        private bool _hasDamaged;
        private Vector3 _startPosPlayer;

        void Start()
        {
            _startPosPlayer = Player.transform.position;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3Int enemyPos = Vector3Int.FloorToInt(transform.position);
            Vector3Int playerPos = Vector3Int.FloorToInt(Player.transform.position);

            if (playerPos == enemyPos)
            {
                if (_hasDamaged) return;
            
                DealDamage(false, _startPosPlayer);
                // Enemy.SetActive(false);

                _hasDamaged = true;
            }
        }

        void DealDamage(bool visibility, Vector3 playerPos)
        {
            Player.SendMessage("ReduceHealth", 1);
            Player.SendMessage("SmoothMovement", _startPosPlayer);
            // Player.GetComponent<Renderer>().enabled = visibility;


            
        }
    }
}
