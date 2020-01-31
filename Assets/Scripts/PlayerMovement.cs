using System;
using System.Collections;
using System.Collections.Generic;
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
        public Tilemap FogTileMap;

        public Rigidbody2D Rb;
        public Animator Animator;
        public SpriteRenderer Sprite;

        private Vector2? _target;
        private bool _onCoolDown;
        private Vector2 _playerStartPos;

        private List<Vector2> _inputKeys = new List<Vector2>();

        private void Start()
        {
            _playerStartPos = Rb.position;
        }

        // Movement 
        void FixedUpdate()
        {
            // Do nothing if player is moving
            if (_isMoving || _onCoolDown) return;


            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");

            if (_movement.x != 0 || _movement.y != 0)
            {
                StartCoroutine(ActionCooldown(0.2f));
                Move(_movement);
            }

//            if (_inputKeys.Count > 2)
//            {
//                foreach (var inputKey in _inputKeys)
//                {
//                    StartCoroutine(ActionCooldown(0.2f));
//                    Move(inputKey);
//                }
//
//            }

            // Animate Sprites
            Animator.SetFloat("Speed", _movement.sqrMagnitude);
        }

        void Move(Vector2 movement)
        {
            Vector2 targetCell = Rb.position + movement;

            bool hasGroundTile = GetCell(GroundTileMap, targetCell) != null; // If target Tile has a ground
            bool hasObstacleTileInner = GetCell(InnerWallTileMap, targetCell) != null; // If target Tile has an obstacle

            if (hasGroundTile && !hasObstacleTileInner && _isMoving == false)
            {
                _isMoving = true;
                _target = targetCell;

                StartCoroutine(SmoothMovement(_target));
            }
        }

        private IEnumerator SmoothMovement(Vector2? end)
        {
            float remainingDistance = (Rb.position - end.Value).sqrMagnitude;

            Sprite.flipX = false;

            if (_movement.x < 0)
            {
                Sprite.flipX = true;
            }

            while (remainingDistance > float.Epsilon)
            {


                Vector2 newPosition = Vector2.MoveTowards(Rb.position, end.Value, _moveSpeed * Time.fixedDeltaTime);
                Rb.position = newPosition;


                remainingDistance = (Rb.position - end).Value.sqrMagnitude;
                yield return null;
            }

            ToggleFog(Vector3Int.FloorToInt(_movement));
            _isMoving = false;
        }

        public void StopMovement()
        {
            _isMoving = false;
        }

        private IEnumerator ActionCooldown(float cooldown)
        {
            _onCoolDown = true;

            while (cooldown > 0f)
            {
                cooldown -= Time.deltaTime;
                yield return null;
            }

            _onCoolDown = false;
        }

        public void ResetPlayer()
        {
            StopMovement();
            StopAllCoroutines();
            Rb.position = _playerStartPos;

            StartCoroutine(ActionCooldown(0.2f));

            _target = null;
        }

        public void ToggleFog(Vector3Int movement)
        {
            Vector3Int currentPlayerTile = FogTileMap.WorldToCell(Rb.position);
            FogTileMap.SetTile(currentPlayerTile, null);
            // FogTileMap.SetTile(currentPlayerTile - Vector3Int.FloorToInt(_movement), FogTileMap.GetTile(currentPlayerTile + Vector3Int.down));
        }

        private TileBase GetCell(Tilemap tileMap, Vector2 cellWorldPos)
        {
            return tileMap.GetTile(tileMap.WorldToCell(cellWorldPos));
        }
    }
}
