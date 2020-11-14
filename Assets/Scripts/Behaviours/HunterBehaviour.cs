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
        private AgentController prey;
        private PolyNavAgent polyNavAgent;
        private Transform rotatable;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            agent = GetComponent<AgentController>();
            prey = null;
            polyNavAgent = GetComponent<PolyNavAgent>();
            rotatable = transform.Find("Rotatable");
        }

        void Update()
        {
            // Se está vendo alguma presa...
            if (agent.Vision.IsSeeingPrey)
            {
                if (prey == null)
                {
                    // Pega os componentes da presa selecionada aleatoriamente entre as avistadas.
                    int selected = Random.Range(0, agent.Vision.Preys.Count);
                    prey = agent.Vision.Preys[selected].GetComponent<AgentController>();
                    //var protection = agentController.Vision.Preys[selected].Find("Rotatable").Find("Marker").GetComponent<ProtectionController>();
                }

                // Se a presa está morta ou protegida contra este predador...
                if (prey.IsDead || ProtectedAgainstMe(prey))
                {
                    // Remove a presa da lista de presas avistadas.
                    if (agent.Vision.Preys.Contains(prey.transform))
                    {
                        agent.Vision.Preys.Remove(prey.transform);
                        prey = null;
                    }
                }
                else
                {
                    // Persegue a presa.
                    polyNavAgent.SetDestination(prey.transform.position);

                    Vector2 movementDirection = polyNavAgent.nextPoint - polyNavAgent.position;
                    float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
                    rotatable.rotation = Quaternion.Slerp(rotatable.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime * turnSpeed);
                }
            }
            else
            {
                if (prey != null)
                {
                    prey = null;
                }
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

        private bool ProtectedAgainstMe(AgentController otherAgent)
        {
            if (agent.IsAerialPredator)
            {
                if (otherAgent.Protection.ProtectedAgainstAerialPredator)
                {
                    return true;
                }
            }
            else if (agent.IsLandPredator)
            {
                if (otherAgent.Protection.ProtectedAgainstLandPredator)
                {
                    return true;
                }
            }

            return false;
        }
    }
}