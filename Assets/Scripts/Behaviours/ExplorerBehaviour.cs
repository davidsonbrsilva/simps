using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS {
    public class ExplorerBehaviour : SimpsBehaviour
    {
        #region Inspector
        [SerializeField] private float turnSpeed = 10f;
        #endregion

        #region Private Attributes
        private PolyNavAgent agent;
        private Vector2 targetPosition;
        private Vector2 maxBounds;
        private Vector2 minBounds;
        private Transform rotatable;
        #endregion

        #region Private Properties
        private PolyNavAgent Agent
        {
            get { return agent != null ? agent : agent = GetComponent<PolyNavAgent>(); }
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            var boundsCollider = GameObject.FindWithTag("Bounds").GetComponent<Collider2D>();
            maxBounds = boundsCollider.bounds.max;
            minBounds = boundsCollider.bounds.min;
            rotatable = transform.Find("Rotatable");
        }

        private void Start()
        {
            StartCoroutine("NewPosition");
        }

        private void Update()
        {
            // Rotaciona o agente em direção à posição alvo.
            Vector2 movementDirection = Agent.nextPoint - Agent.position;
            float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            rotatable.rotation = Quaternion.Slerp(rotatable.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime * turnSpeed);

            // Move o agente em direção à posição alvo.
            Agent.SetDestination(targetPosition, (result) => StartCoroutine("NewPosition"));
        }
        #endregion

        #region Other Methods
        private IEnumerator NewPosition()
        {
            yield return new WaitForSeconds(0.4f);
            
            targetPosition = new Vector2(
                Random.Range(minBounds.x, maxBounds.x), 
                Random.Range(minBounds.y, maxBounds.y)
            );
        }

        public override void Restart()
        {
            StartCoroutine("NewPosition");
        }
        #endregion
    }
}