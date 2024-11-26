using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AIGAME.FSM
{
    [Serializable]
    public class WanderState : PatrolState
    {
        [field: SerializeField] public float Range { get; private set; }
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _moveInterval = 1f;
        [SerializeField] private float _distanceThreshold = 0.3f;
        private float _elapsedTime;
        private bool _isFacingLeft;
        private Vector2 _moveDirection;
        private Vector2 _targetPosition;

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _targetPosition = Controller.Collector.Position + Random.insideUnitSphere * Range;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            Controller.Collector.Animator.SetBool("isMoving", false);
        }

        protected override void OnStateUpdate()
        {
            base.OnStateUpdate();

            _moveDirection = (_targetPosition - (Vector2)Controller.Collector.Position).normalized;

            if (_moveDirection.x < 0)
                _isFacingLeft = true;
            else if (_moveDirection.x > 0) _isFacingLeft = false;

            Controller.Collector.SpriteRenderer.flipX = _isFacingLeft;

            if (Vector2.Distance(Controller.Collector.Position, _targetPosition) < _distanceThreshold)
            {
                if (_elapsedTime < _moveInterval)
                {
                    _elapsedTime += Time.deltaTime;
                    _moveDirection = Vector2.zero;
                }
                else
                {
                    _elapsedTime = 0f;
                    _targetPosition = Controller.Collector.Position + Random.insideUnitSphere * Range;
                }

                Controller.Collector.Animator.SetBool("isMoving", false);
            }
            else
            {
                Controller.Collector.Animator.SetBool("isMoving", true);
            }

            if (Physics2D.OverlapCircle(Controller.Collector.Position, Controller.Seek.Range,
                    Controller.Seek.VisibleLayer)) Controller.ChangeState(Controller.Seek);
        }

        protected override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
            Controller.Collector.Rigidbody.MovePosition((Vector2)Controller.Collector.Position +
                                                        _moveDirection * (_moveSpeed * Time.fixedDeltaTime));
        }
    }
}