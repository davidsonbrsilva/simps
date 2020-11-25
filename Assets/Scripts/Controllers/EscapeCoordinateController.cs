using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SIMPS
{
    public class EscapeCoordinateController : MonoBehaviour
    {
        [SerializeField] private float coordinateValue;

        public float MeanPredatorsDistance { get; set; }
        public float MeanTreesDistance { get; set; }
        public float MeanBushesDistance { get; set; }
        public float CoordinateValue { get { return coordinateValue; } set { coordinateValue = value; } }
    }
}