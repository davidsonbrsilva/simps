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

        #region Properties
        public bool ProtectedAgainstAerialPredator { get { return protectedAgainstAerialPredator; } set { protectedAgainstAerialPredator = value; } }
        public bool ProtectedAgainstLandPredator { get { return protectedAgainstLandPredator; } set { protectedAgainstLandPredator = value; } }
        #endregion

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
        #endregion
    }
}
