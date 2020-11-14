using UnityEngine;

namespace SIMPS
{
    /// <summary>
    /// Controlador de sinal.
    /// </summary>
    public class SignalController : MonoBehaviour
    {
        #region Inspector
        [SerializeField] private int symbol = -1;
        [SerializeField] private GameObject sender;
        #endregion

        #region Properties
        public int Symbol { get { return symbol; } set { symbol = value; } }
        public GameObject Sender { get { return sender; } set { sender = value; } }
        #endregion

        #region Other Methods
        public void DestroySignal()
        {
            Destroy(gameObject);
        }
        #endregion
    }
}
