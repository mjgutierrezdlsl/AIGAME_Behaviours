using System;
using UnityEngine;

namespace AIGAME.FSM
{
    [Serializable]
    public class SeekState : PatrolState
    {
        [field: SerializeField] public float Range { get; private set; } = 1f;
        [field: SerializeField] public LayerMask VisibleLayer { get; private set; }
        [SerializeField] private float _moveSpeed = 2f;
        private Collider2D _currentTarget;
        private bool _isFacingLeft;
        private Vector2 _moveDirection;

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _currentTarget = Physics2D.OverlapCircle(Controller.Collector.Position, Range, VisibleLayer);
            Controller.Collector.Animator.SetBool("isMoving", true);
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _currentTarget = null;
            Controller.Collector.Animator.SetBool("isMoving", false);
        }

        protected override void OnStateUpdate()
        {
            base.OnStateUpdate();
            _moveDirection = (_currentTarget.transform.position - Controller.Collector.Position).normalized;
            if (_moveDirection.x < 0)
                _isFacingLeft = true;
            else if (_moveDirection.x > 0) _isFacingLeft = false;

            Controller.Collector.SpriteRenderer.flipX = _isFacingLeft;
        }

        protected override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
            Controller.Collector.Rigidbody.MovePosition((Vector2)Controller.Collector.Position +
                                                        _moveDirection * _moveSpeed * Time.fixedDeltaTime);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (_currentTarget == other)
            {
                // Pass the gameObject to the collector in order to be destroyed
                Controller.Collector.AddResource(other.gameObject);
                // Return to Wander state
                Controller.ChangeState(Controller.Wander);
            }
        }
    }
}