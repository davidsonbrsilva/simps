using SIMPS;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private int symbols = 10;

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

        public int Symbols { get { return symbols; } private set { symbols = value; } }
        public List<Transform> Preys { get; private set; }
        public List<Transform> LandPredators { get; private set; }
        public List<Transform> AerialPredators { get; private set; }
        public List<Transform> CrowlingPredators { get; private set; }
        public List<Transform> AllPredators { get; private set; }
        public List<Transform> Trees { get; private set; }
        public List<Transform> Bushes { get; private set; }
        public List<Transform> EscapeCoordinates { get; private set; }

        private void Awake()
        {
            Preys = new List<Transform>();
            LandPredators = new List<Transform>();
            AerialPredators = new List<Transform>();
            CrowlingPredators = new List<Transform>();
            AllPredators = new List<Transform>();
            Trees = new List<Transform>();
            Bushes = new List<Transform>();
            EscapeCoordinates = new List<Transform>();

            var map = GameObject.FindWithTag("Map");
            var escapeCoordinates = map.transform.Find("Escape Coordinates");
            var bounds = map.transform.Find("Bounds").GetComponent<BoxCollider2D>().bounds;
            var wall = GameObject.FindWithTag("Wall").GetComponent<BoxCollider2D>();

            float horizontalStep = bounds.size.x / columns;
            float verticalStep = bounds.size.y / lines;

            float xAccumulator = bounds.min.x + horizontalStep / 2;
            float yAccumulator = bounds.min.y + verticalStep / 2;

            for (int line = 0; line < lines; ++line)
            {
                for (int column = 0; column < columns; ++column)
                {
                    if (!(xAccumulator > wall.bounds.min.x &&
                    xAccumulator < wall.bounds.max.x &&
                    yAccumulator > wall.bounds.min.y &&
                    yAccumulator < wall.bounds.max.y))
                    {
                        EscapeCoordinates.Add(Instantiate(
                        escapeCoordinate.Prefabs[0],
                        new Vector3(xAccumulator, yAccumulator, 0f),
                        Quaternion.identity,
                        escapeCoordinates
                    ).transform);
                    }

                    xAccumulator += horizontalStep;
                }

                xAccumulator = bounds.min.x + horizontalStep / 2;
                yAccumulator += verticalStep;
            }

            int preyId = 0;
            int predatorId = 0;
            int bushId = 0;
            int treeId = 0;

            Create(prey, Preys, ref preyId, "p", "Prey", false);
            Create(landPredator, LandPredators, ref predatorId, "l", "Land Predator", true);
            Create(aerialPredator, AerialPredators, ref predatorId, "a", "Aerial Predator", true);
            Create(crowlingPredator, CrowlingPredators, ref predatorId, "c", "Crowling Predator", true);
            Create(bush, Bushes, ref bushId, "b", "Bush", false);
            Create(tree, Trees, ref treeId, "t", "Tree", false);

            Debug.Log(AllPredators.Count);

            /*for (int i = 0; i < prey.Amount; ++i)
            {
                var position = EscapeCoordinates[Random.Range(0, EscapeCoordinates.Count)].transform.position;
                Preys.Add(Instantiate(prey.GetRandomPrefab(), position, Quaternion.identity, prey.Parent).transform);
            }

            for (int i = 0; i < landPredator.Amount; ++i)
            {
                var position = EscapeCoordinates[Random.Range(0, EscapeCoordinates.Count)].transform.position;
                var instance = Instantiate(landPredator.Prefabs[0], position, Quaternion.identity, landPredator.Parent).transform;
                LandPredators.Add(instance);
                AllPredators.Add(instance);
            }

            for (int i = 0; i < aerialPredator.Amount; ++i)
            {
                var position = EscapeCoordinates[Random.Range(0, EscapeCoordinates.Count)].transform.position;
                var instance = Instantiate(aerialPredator.Prefabs[0], position, Quaternion.identity, aerialPredator.Parent).transform;
                AerialPredators.Add(instance);
                AllPredators.Add(instance);
            }

            for (int i = 0; i < crowlingPredator.Amount; ++i)
            {
                var position = EscapeCoordinates[Random.Range(0, EscapeCoordinates.Count)].transform.position;
                var instance = Instantiate(crowlingPredator.Prefabs[0], position, Quaternion.identity, crowlingPredator.Parent).transform;
                CrowlingPredators.Add(instance);
                AllPredators.Add(instance);
            }

            for (int i = 0; i < tree.Amount; ++i)
            {
                var position = EscapeCoordinates[Random.Range(0, EscapeCoordinates.Count)].transform.position;
                Trees.Add(Instantiate(tree.GetRandomPrefab(), position, Quaternion.identity, tree.Parent).transform);
            }

            for (int i = 0; i < bush.Amount; ++i)
            {
                var position = EscapeCoordinates[Random.Range(0, EscapeCoordinates.Count)].transform.position;
                Bushes.Add(Instantiate(bush.GetRandomPrefab(), position, Quaternion.identity, bush.Parent).transform);
            }*/
        }

        private void Create(Instantable instantable, List<Transform> activeObjects, ref int id, string shortname, string name, bool addToAllPredators)
        {
            for (int i = 0; i < instantable.Amount; ++i)
            {
                var position = EscapeCoordinates[Random.Range(0, EscapeCoordinates.Count)].transform.position;
                GameObject clone = Instantiate(instantable.GetRandomPrefab(), position, Quaternion.identity, instantable.Parent);

                activeObjects.Add(clone.transform);

                if (addToAllPredators)
                {
                    AllPredators.Add(clone.transform);
                }

                AgentController controller = clone.GetComponent<AgentController>();

                if (controller != null)
                {
                    controller.GlobalIndex = id;
                    controller.ShortName = shortname + (i + 1);
                    controller.Name = name + " " + (i + 1);
                    id++;
                }
            }
        }
    }
}
