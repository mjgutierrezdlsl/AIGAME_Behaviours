using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AIGAME.FSM
{
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
    public class CollectorController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _stateLabel;

        public Animator Animator { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }

        public Vector3 Position => Rigidbody.position;

        public int ResourceCount { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void AddResource(GameObject resource)
        {
            ResourceCount++;
            Destroy(resource);
        }

        public void OnStateChanged(State state)
        {
            _stateLabel.text = $"State: {state}";
        }
    }
}
