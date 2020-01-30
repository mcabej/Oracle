using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class AdjacentDetectPlayer : MonoBehaviour
    {
        public GameObject Player;
        public GameObject Arrow;

        private bool _isMoving;
        private float _moveSpeed = 0.1f;
        
        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3Int adjacentLeftPos = Vector3Int.FloorToInt(transform.position + Vector3.left);
            Vector3Int adjacentDownPos = Vector3Int.FloorToInt(transform.position + Vector3.down);
            Vector3Int playerPos = Vector3Int.FloorToInt(Player.transform.position);

            if (_isMoving) return;

            if (playerPos == adjacentLeftPos || playerPos == adjacentDownPos)
            {
                StartCoroutine(SmoothMovement(playerPos));
                // Arrow.SetActive(false);
                // Player.SetActive(false);
            }
        }

        public IEnumerator SmoothMovement(Vector3 end)
        {
            float remainingDistance = (transform.position - end).sqrMagnitude;
            float inverseMoveSpeed = 1 / _moveSpeed;

            _isMoving = true;

            while (remainingDistance > float.Epsilon)
            {
                Vector2 newPosition = Vector2.MoveTowards(transform.position, end, inverseMoveSpeed * Time.fixedDeltaTime);
                transform.position = newPosition;
                remainingDistance = (transform.position - end).sqrMagnitude;

                yield return null;
            }

            _isMoving = false;
        }
    }
}
