using UnityEngine;

namespace SIMPS
{
    public class AgentController : MonoBehaviour
    {
        [SerializeField] private string _name;

        private Manager manager;

        public string Name { get { return _name; } set { _name = value; } }
        public string ShortName { get; set; }

        public int GlobalIndex { get; set; }
        public bool IsPrey { get; private set; }
        public bool IsLandPredator { get; private set; }
        public bool IsAerialPredator { get; private set; }
        public bool IsCrowlingPredator { get; private set; }
        public bool IsPredator { get; private set; }
        public bool CanLearn { get { return Learner != null && Learner.enabled; } }
        public bool IsDead { get; private set; }
        public bool IsHunting { get; set; }
        public EmitterBehaviour Emitter { get; private set; }
        public ExplorerBehaviour Explorer { get; private set; }
        public HunterBehaviour Hunter { get; private set; }
        public LearnerBehaviour Learner { get; private set; }
        public FearfulBehaviour Fearful { get; private set; }
        public LonelinessBehaviour Loneliness { get; private set; }
        public VisionController Vision { get; private set; }
        public CaptureController Capture { get; private set; }
        public ProtectionController Protection { get; private set; }
        public RecognitionController Hearing { get; private set; }
        public ActionRadiusController ActionRadius { get; private set; }
        public Animator Animator { get; private set; }

        public void Awake()
        {
            IsPrey = false;
            IsPredator = false;
            IsLandPredator = false;
            IsAerialPredator = false;
            IsCrowlingPredator = false;

            Transform agentVision = transform.Find("Rotatable").Find("Vision");
            Transform agentMarker = transform.Find("Rotatable").Find("Marker");
            Transform agentActionRadius = transform.Find("Action Radius");
            manager = GameObject.FindWithTag("Core").GetComponent<Manager>();

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
                Hearing = agentMarker.GetComponent<RecognitionController>();
                Protection = agentMarker.GetComponent<ProtectionController>();
                ActionRadius = agentActionRadius.GetComponent<ActionRadiusController>();

                Emitter = GetComponent<EmitterBehaviour>();
                Explorer = GetComponent<ExplorerBehaviour>();
                Hunter = GetComponent<HunterBehaviour>();
                Learner = GetComponent<LearnerBehaviour>();
                Fearful = GetComponent<FearfulBehaviour>();
                Loneliness = GetComponent<LonelinessBehaviour>();
            }
            else if (IsPredator)
            {
                Vision = agentVision.GetComponent<VisionController>();
                Capture = agentMarker.GetComponent<CaptureController>();

                Explorer = GetComponent<ExplorerBehaviour>();
                Hunter = GetComponent<HunterBehaviour>();
            }

            Animator = GetComponent<Animator>();
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

        public void Die()
        {
            ChangeSimpsBehavioursTo(false);
        }

        public void Reborn()
        {
            ChangeSimpsBehavioursTo(true);

            if (!manager.CanLearn)
            {
                Learner.enabled = false;
            }
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