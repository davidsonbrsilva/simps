using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class CaptureController : MonoBehaviour
    {
        public bool HasCaptured { get; private set; }

        private Transform preyTransform;
        private AgentController thisAgent;

        private void Awake()
        {
            thisAgent = transform.parent.parent.GetComponent<AgentController>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Prey"))
            {
                if (thisAgent.IsHunting)
                {
                    preyTransform = collision.transform.parent.parent;
                    var preyAnimator = preyTransform.GetComponent<Animator>();
                    var prey = preyTransform.GetComponent<AgentController>();

                    if (!prey.IsDead)
                    {
                        preyAnimator.SetTrigger("Die");

                        if (!HasCaptured)
                        {
                            HasCaptured = true;
                            prey.Protection.WasCaptured = true;
                        }
                    }
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Prey"))
            {
                if (HasCaptured)
                {
                    HasCaptured = false;

                    var prey = preyTransform.GetComponent<AgentController>();
                    prey.Protection.WasCaptured = false;
                }
            }
        }
    }
}