using SIMPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRadiusController : MonoBehaviour
{
    //[SerializeField] private List<EscapeCoordinateController> escapeCoordinateControllers;
    [SerializeField] private List<GameObject> escapeCoordinates;

    //public List<EscapeCoordinateController> EscapeCoordinateControllers { get { return escapeCoordinateControllers; } private set { escapeCoordinateControllers = value; } }
    public List<GameObject> EscapeCoordinates { get { return escapeCoordinates; } private set { escapeCoordinates = value; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Escape Coordinate"))
        {
            EscapeCoordinates.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Escape Coordinate"))
        {
            EscapeCoordinates.Remove(collision.gameObject);
            collision.GetComponent<EscapeCoordinateController>().CoordinateValue = 0;
        }
    }
}
