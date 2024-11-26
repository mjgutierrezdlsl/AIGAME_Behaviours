using UnityEngine;

namespace AIGAME.FSM
{
    [SelectionBase]
    [RequireComponent(typeof(CollectorController))]
    public class CollectorStateController : StateController
    {
        [field: SerializeField] public WanderState Wander { get; private set; } = new();
        [field: SerializeField] public SeekState Seek { get; private set; } = new();
        [field: SerializeField] public AlertState Alert { get; private set; } = new();
        public CollectorController Collector { get; private set; }

        private void Awake()
        {
            Collector = GetComponent<CollectorController>();
        }

        private void Start()
        {
            ChangeState(Wander);
        }

        private void OnEnable()
        {
            OnStateChanged += UpdateState;
        }

        private void OnDisable()
        {
            OnStateChanged -= UpdateState;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Wander.Range);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Seek.Range);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Alert.Range);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CurrentState is SeekState seekState) seekState.OnTriggerEnter2D(other);
        }

        private void UpdateState(State currentState, State previousState)
        {
            Debug.Log($"New State: {currentState} | Prev State: {previousState}");
        }
    }
}