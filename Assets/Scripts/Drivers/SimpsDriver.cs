using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpsDriver : MonoBehaviour
{
    public float motivation;
    public bool activated;

    public float Motivation { get { return motivation; } }
    public bool Activated { get { return activated; } }
}
