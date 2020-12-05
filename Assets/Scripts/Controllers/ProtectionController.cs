using UnityEngine;

namespace SIMPS
{
    /// <summary>
    /// Controlador do tato dos agentes.
    /// </summary>
    public class ProtectionController : MonoBehaviour
    {
        #region Inspector
        [SerializeField] private bool protectedAgainstAerialPredator = false;
        [SerializeField] private bool protectedAgainstLandPredator = false;
        #endregion

        private int frameCounter;

        #region Properties
        public bool ProtectedAgainstAerialPredator { get { return protectedAgainstAerialPredator; } set { protectedAgainstAerialPredator = value; } }
        public bool ProtectedAgainstLandPredator { get { return protectedAgainstLandPredator; } set { protectedAgainstLandPredator = value; } }
        public bool WasCaptured { get; set; }
        #endregion

        private void Awake()
        {
            frameCounter = 0;
        }

        #region UnityMethods
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Bush"))
            {
                protectedAgainstAerialPredator = true;
            }
            else if (collision.CompareTag("Tree"))
            {
                protectedAgainstLandPredator = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Bush"))
            {
                protectedAgainstAerialPredator = false;
            }
            else if (collision.CompareTag("Tree"))
            {
                protectedAgainstLandPredator = false;
            }
        }

        private void Update()
        {
            if (frameCounter > 0)
            {
                frameCounter--;

                if (frameCounter == 0)
                {
                    WasCaptured = false;
                }
            }

            if (WasCaptured)
            {
                frameCounter = 1;
            }
        }
        #endregion
    }
}
