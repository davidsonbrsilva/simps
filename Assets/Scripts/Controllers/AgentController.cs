using UnityEngine;

namespace SIMPS
{
    public class AgentController : MonoBehaviour
    {
        [SerializeField] private string _name;

        public string Name { get { return _name; } set { _name = value; } }
        public string ShortName { get; set; }

        public int GlobalIndex { get; set; }
        public bool IsPrey { get; private set; }
        public bool IsLandPredator { get; private set; }
        public bool IsAerialPredator { get; private set; }
        public bool IsCrowlingPredator { get; private set; }
        public bool IsPredator { get; private set; }
        //public bool CanLearn { get { return Learner != null && Learner.enabled; } }
        public bool CanLearn { get { return true; } }
        public bool IsDead { get; private set; }
        public bool IsHunting { get; set; }
        public EmitterBehaviour Emitter { get; private set; }
        public ExplorerBehaviour Explorer { get; private set; }
        public HunterBehaviour Hunter { get; private set; }
        //public Learner Learner { get; private set; }
        //public Fearful Fearful { get; private set; }
        //public Mortal Mortal { get; private set; }
        public VisionController Vision { get; private set; }
        public CaptureController Capture { get; private set; }
        public ProtectionController Protection { get; private set; }
        //public HearingController Hearing { get; private set; }
        //public EscapeController Escape { get; private set; }

        private Manager manager;

        public void Awake()
        {
            IsPrey = false;
            IsPredator = false;
            IsLandPredator = false;
            IsAerialPredator = false;
            IsCrowlingPredator = false;

            manager = GameObject.FindWithTag("Core").GetComponent<Manager>();

            Transform agentVision = transform.Find("Rotatable").Find("Vision");
            Transform agentMarker = transform.Find("Rotatable").Find("Marker");
            //Transform agentEscape = transform.Find("Escape");

            if (agentMarker.CompareTag("Prey"))
            {
                IsPrey = true;
            }
            else if (agentMarker.CompareTag("Land Predator"))
            {
                IsLandPredator = true;
                IsPredator = true;
            }
            else if (agentMarker.CompareTag("Aerial Predator"))
            {
                IsAerialPredator = true;
                IsPredator = true;
            }
            else if (agentMarker.CompareTag("Crowling Predator"))
            {
                IsCrowlingPredator = true;
                IsPredator = true;
            }

            if (IsPrey)
            {
                Vision = agentVision.GetComponent<VisionController>();
                //Hearing = agentMarker.GetComponent<HearingController>();
                Protection = agentMarker.GetComponent<ProtectionController>();
                //Escape = agentEscape.GetComponent<EscapeController>();

                Emitter = GetComponent<EmitterBehaviour>();
                Explorer = GetComponent<ExplorerBehaviour>();
                Hunter = GetComponent<HunterBehaviour>();
                //Learner = GetComponent<Learner>();
                //Mortal = GetComponent<Mortal>();
                //Fearful = GetComponent<Fearful>();
            }
            else if (IsPredator)
            {
                Vision = agentVision.GetComponent<VisionController>();
                Capture = agentMarker.GetComponent<CaptureController>();

                Explorer = GetComponent<ExplorerBehaviour>();
                Hunter = GetComponent<HunterBehaviour>();
            }
        }

        private void Update()
        {
            if (Hunter && Hunter.enabled && !IsHunting)
            {
                IsHunting = true;
            }
            else if (Hunter && !Hunter.enabled && IsHunting)
            {
                IsHunting = false;
            }
        }

        // Este trecho foi colocado no método Start para que garantidamente o controlador do simulador tenha criado todos os objetos.
        // Ao aidicioná-lo em Awake, IndexOutOfRangeException é encontrado.

        public AgentController GetControllerOf(int agent, bool isPredator = true)
        {
            if (isPredator)
            {
                return manager.Predators[agent].GetComponent<AgentController>();
            }
            else
            {
                return manager.Preys[agent].GetComponent<AgentController>();
            }
        }

        public void Die()
        {
            ChangeSimpsBehavioursTo(false);
        }

        public void Reborn()
        {
            ChangeSimpsBehavioursTo(true);
        }

        public void ChangeSimpsBehavioursTo(bool enabled)
        {
            IsDead = !enabled;

            var monoBehaviours = GetComponents<MonoBehaviour>();

            foreach (var monoBehaviour in monoBehaviours)
            {
                monoBehaviour.enabled = enabled;
            }
        }
    }
}