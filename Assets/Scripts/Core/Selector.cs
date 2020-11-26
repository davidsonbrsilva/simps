using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] private GameObject selectedAgent;

        public GameObject SelectedAgent
        {
            get { return selectedAgent; }
            set { selectedAgent = value; }
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hits = Physics2D.RaycastAll(worldPoint, Vector2.zero);

                if (hits.Length > 0)
                {
                    bool hitAgent = false;

                    foreach (var hit in hits)
                    {
                        if (hit.collider.transform.CompareTag("Prey") ||
                        hit.collider.transform.CompareTag("Aerial Predator") ||
                        hit.collider.transform.CompareTag("Land Predator") ||
                        hit.collider.transform.CompareTag("Crowling Predator"))
                        {
                            selectedAgent = hit.collider.transform.gameObject;
                            hitAgent = true;
                            break;
                        }
                    }

                    if (!hitAgent)
                    {
                        foreach (var hit in hits)
                        {
                            if (hit.collider.transform.CompareTag("Map"))
                            {
                                selectedAgent = null;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}