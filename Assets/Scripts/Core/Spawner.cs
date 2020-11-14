using SIMPS;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class Spawner : MonoBehaviour
    {
        [Header("Escape Coordinates")]
        [SerializeField] private Instantable escapeCoordinate;
        [SerializeField] private int columns;
        [SerializeField] private int lines;

        [Header("Agents")]
        [SerializeField] private Instantable prey;
        [SerializeField] private Instantable landPredator;
        [SerializeField] private Instantable aerialPredator;
        [SerializeField] private Instantable crowlingPredator;

        [Header("Hidding Places")]
        [SerializeField] private Instantable tree;
        [SerializeField] private Instantable bush;

        public List<GameObject> Preys { get; private set; }
        public List<GameObject> LandPredators { get; private set; }
        public List<GameObject> AerialPredators { get; private set; }
        public List<GameObject> CrowlingPredators { get; private set; }
        public List<GameObject> AllPredators { get; private set; }
        public List<GameObject> Trees { get; private set; }
        public List<GameObject> Bushes { get; private set; }
        public List<GameObject> EscapeCoordinates { get; private set; }

        private void Awake()
        {
            Preys = new List<GameObject>();
            LandPredators = new List<GameObject>();
            AerialPredators = new List<GameObject>();
            CrowlingPredators = new List<GameObject>();
            AllPredators = new List<GameObject>();
            Trees = new List<GameObject>();
            Bushes = new List<GameObject>();
            EscapeCoordinates = new List<GameObject>();

            var map = GameObject.FindWithTag("Map");
            var escapeCoordinates = map.transform.Find("Escape Coordinates");
            var bounds = map.transform.Find("Bounds").GetComponent<BoxCollider2D>().bounds;

            float horizontalStep = bounds.size.x / columns;
            float verticalStep = bounds.size.y / lines;

            float xAccumulator = bounds.min.x + horizontalStep / 2;
            float yAccumulator = bounds.min.y + verticalStep / 2;

            for (int line = 0; line < lines; ++line)
            {
                for (int column = 0; column < columns; ++column)
                {
                    EscapeCoordinates.Add(Instantiate(
                        escapeCoordinate.Prefabs[0],
                        new Vector3(xAccumulator, yAccumulator, 0f),
                        Quaternion.identity,
                        escapeCoordinates
                    ));
                    xAccumulator += horizontalStep;
                }

                xAccumulator = bounds.min.x + horizontalStep / 2;
                yAccumulator += verticalStep;
            }

            for (int i = 0; i < prey.Amount; ++i)
            {
                var position = EscapeCoordinates[Random.Range(0, EscapeCoordinates.Count)].transform.position;
                Preys.Add(Instantiate(prey.GetRandomPrefab(), position, Quaternion.identity, prey.Parent));
            }

            for (int i = 0; i < landPredator.Amount; ++i)
            {
                var position = EscapeCoordinates[Random.Range(0, EscapeCoordinates.Count)].transform.position;
                var instance = Instantiate(landPredator.Prefabs[0], position, Quaternion.identity, landPredator.Parent);
                LandPredators.Add(instance);
                AllPredators.Add(instance);
            }

            for (int i = 0; i < aerialPredator.Amount; ++i)
            {
                var position = EscapeCoordinates[Random.Range(0, EscapeCoordinates.Count)].transform.position;
                var instance = Instantiate(aerialPredator.Prefabs[0], position, Quaternion.identity, aerialPredator.Parent);
                AerialPredators.Add(instance);
                AllPredators.Add(instance);
            }

            for (int i = 0; i < crowlingPredator.Amount; ++i)
            {
                var position = EscapeCoordinates[Random.Range(0, EscapeCoordinates.Count)].transform.position;
                var instance = Instantiate(crowlingPredator.Prefabs[0], position, Quaternion.identity, crowlingPredator.Parent);
                CrowlingPredators.Add(instance);
                AllPredators.Add(instance);
            }

            for (int i = 0; i < tree.Amount; ++i)
            {
                var position = EscapeCoordinates[Random.Range(0, EscapeCoordinates.Count)].transform.position;
                Trees.Add(Instantiate(tree.GetRandomPrefab(), position, Quaternion.identity, tree.Parent));
            }

            for (int i = 0; i < bush.Amount; ++i)
            {
                var position = EscapeCoordinates[Random.Range(0, EscapeCoordinates.Count)].transform.position;
                Bushes.Add(Instantiate(bush.GetRandomPrefab(), position, Quaternion.identity, bush.Parent));
            }
        }
    }
}
