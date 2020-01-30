using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;


namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        private float _moveSpeed = 5f;
        private Vector2 _movement;
        private bool _isMoving;

        public Tilemap GroundTileMap;
        public Tilemap InnerWallTileMap;

        public Rigidbody2D Rb;
        public Animator Animator;
        public SpriteRenderer Sprite;
    
        // Update is called once per frame
        // Movement Input and Sprite flip on x-axis
        void FixedUpdate()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");

            // Do nothing if player is moving
            if (_isMoving) return;

            // Animate Sprites
            Animator.SetFloat("Speed", _movement.sqrMagnitude);

            if (_movement.x != 0 || _movement.y != 0)
            {
                Move(_movement);
            }
        }

        void Move(Vector2 movement)
        {
            Vector2 targetCell = Rb.position + movement;

            bool hasGroundTile = GetCell(GroundTileMap, targetCell) != null; // If target Tile has a ground
            bool hasObstacleTileInner = GetCell(InnerWallTileMap, targetCell) != null; // If target Tile has an obstacle

            if (hasGroundTile && !hasObstacleTileInner)
            {
                StartCoroutine(SmoothMovement(targetCell));
            }
        }

        private IEnumerator SmoothMovement(Vector2 end)
        {
            float remainingDistance = (Rb.position - end).sqrMagnitude;

            _isMoving = true;

            Sprite.flipX = false;

            if (_movement.x < 0)
            {
                Sprite.flipX = true;
            }

            while (remainingDistance > float.Epsilon)
            {
                Vector2 newPosition = Vector2.MoveTowards(Rb.position, end, _moveSpeed * Time.fixedDeltaTime);
                Rb.position = newPosition;
                remainingDistance = (Rb.position - end).sqrMagnitude;

                yield return null;
            }

            _isMoving = false;
        }

        private TileBase GetCell(Tilemap tileMap, Vector2 cellWorldPos)
        {
            return tileMap.GetTile(tileMap.WorldToCell(cellWorldPos));
        }
    }
}
