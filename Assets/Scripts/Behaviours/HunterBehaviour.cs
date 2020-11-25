using UnityEngine;

namespace SIMPS
{
    /// <summary>
    /// Componente para agentes que caçam.
    /// </summary>
    public class HunterBehaviour : SimpsBehaviour
    {
        [SerializeField] private float turnSpeed = 10f;

        private AgentController agent;
        private PursuitController pursuit;
        private PolyNavAgent polyNavAgent;
        private Transform rotatable;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            pursuit = GetComponent<PursuitController>();
            agent = GetComponent<AgentController>();
            polyNavAgent = GetComponent<PolyNavAgent>();
            rotatable = transform.Find("Rotatable");
        }

        /*void Start()
        {
            
            
            
        }*/

        void Update()
        {
            if (agent.Vision.IsSeeingPrey)
            {
                // Persegue a presa.
                polyNavAgent.SetDestination(pursuit.Prey.transform.position);

                Vector2 movementDirection = polyNavAgent.nextPoint - polyNavAgent.position;
                float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
                rotatable.rotation = Quaternion.Slerp(rotatable.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime * turnSpeed);
            }
        }

        private void OnEnable()
        {
            animator.SetBool("IsAlert", true);
        }

        private void OnDisable()
        {
            animator.SetBool("IsAlert", false);
        }


    }
}