using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIGAME.FSM
{
    [SelectionBase]
    [RequireComponent(typeof(CollectorController))]
    public class CollectorStateController : StateController
    {
        public CollectorController Collector { get; private set; }
        [field: SerializeField] public WanderState Wander { get; private set; } = new();
        [field: SerializeField] public SeekState Seek { get; private set; } = new();
        private void Awake()
        {
            Collector = GetComponent<CollectorController>();
        }
        private void Start()
        {
            ChangeState(Wander);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CurrentState is SeekState seekState)
            {
                seekState.OnTriggerEnter2D(other);
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Wander.Range);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Seek.Range);
        }
    }
}
