using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SIMPS
{
    public class Recorder : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Tempo entre gravações.")]
        private float timeBetweenSaves = 10f;

        [SerializeField]
        [Tooltip("Última vez que foi gravado.")]
        private string lastSave;

        private Manager manager;
        private Spawner spawner;
        private Logger logger;
        private ConvergenceChecker checker;
        private float timer;

        private string rootPath = "Logs";
        private string batchPath;
        private string simulationPath;

        private string deathsFile;
        private string learningFile;
        private string infoFile;
        private string infoJson;

        private ulong batchId;
        private ulong simId;
        private string batchStart;
        private string batchEnd;
        private string simStart;
        private string simEnd;

        public string Now { get { return DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"); } }

        private void Awake()
        {
            var core = GameObject.FindWithTag("Core");
            manager = core.GetComponent<Manager>();
            spawner = core.GetComponent<Spawner>();
            logger = core.GetComponent<Logger>();
            checker = core.GetComponent<ConvergenceChecker>();
            timer = 0f;
        }

        private void Start()
        {
            NewBatch();
            Save();
        }

        private void LateUpdate()
        {
            timer += Time.deltaTime;

            if (manager.Restart)
            {
                NewSimulation();
                Save();
                timer = 0f;
            }

            if (manager.Done)
            {
                AddSimulationEndToInfoFile(Now);
                Save();
            }

            if (manager.AllDone)
            {
                AddBatchEndToInfoFile(Now);
            }

            if (checker.Converged)
            {
                UpdateConvergenceIntoInfoFile(Now);
                Save();
            }

            AutoSave();
        }

        #region Save Methods
        private void Save(string path, List<string> records)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                foreach (var record in records)
                {
                    sw.Write(record);
                }

                records.Clear();
            }
        }

        private void Save()
        {
            Save(deathsFile, logger.Deaths);
            Save(learningFile, logger.Learning);
            lastSave = DateTime.Now.ToString();
        }

        private void AutoSave()
        {
            if (timer >= timeBetweenSaves)
            {
                Save();
                timer = 0f;
            }
        }
        #endregion

        #region Batch Methods
        private void NewBatch()
        {
            CreateBatchPath();
            CreateBatchFiles();
            NewSimulation();
        }

        private void CreateBatchPath()
        {
            batchStart = Now;
            batchId = Convert.ToUInt64(DateTime.Now.ToString("yyyyMMddHHmmfff"));
            string batchFolder = string.Format("batch_{0}", batchId);
            batchPath = CreatePath(rootPath, batchFolder);
        }

        private void CreateBatchFiles()
        {
            CreateInfoFile();
        }

        private Batch MountBatch()
        {
            List<Agent> preys = new List<Agent>();
            List<Agent> predators = new List<Agent>();

            foreach (var preyController in spawner.PreyControllers)
            {
                var prey = new Agent(preyController.GlobalIndex, preyController.Name, preyController.ShortName);
                preys.Add(prey);
            }

            foreach (var predatorController in spawner.PredatorControllers)
            {
                var predator = new Agent(predatorController.GlobalIndex, predatorController.Name, predatorController.ShortName);
                predators.Add(predator);
            }

            Batch batch = new Batch(batchId, manager.Batch, manager.Mode, batchStart)
            {
                preys = preys,
                predators = predators
            };

            return batch;
        }
        #endregion

        #region Simulation Methods
        private void NewSimulation()
        {
            CreateSimulationPath();
            CreateSimulationFiles();

            Simulation simulation = MountSimulation();
            AddSimulationToInfoFile(simulation);
        }

        private void CreateSimulationPath()
        {
            simStart = Now;
            simId = Convert.ToUInt64(DateTime.Now.ToString("yyyyMMddHHmmfff"));
            string simulationFolder = string.Format("sim_{0}", simId);
            simulationPath = CreatePath(batchPath, simulationFolder);
        }

        private void CreateSimulationFiles()
        {
            deathsFile = string.Format("{0}/deaths.csv", simulationPath);
            learningFile = string.Format("{0}/learning.csv", simulationPath);

            CreateFile(deathsFile, logger.DeathsHeader);
            CreateFile(learningFile, logger.LearningHeader);
        }

        private Simulation MountSimulation()
        {
            return new Simulation(simId, simulationPath, manager.CanLearn, simStart);
        }
        #endregion

        #region Helping Methods
        private void CreateFile(string path, string content = "")
        {
            File.Create(path).Dispose();

            if (content != "" && content != null)
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(content);
                }
            }
        }

        private void UpdateFile(string path, string content)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(content);
            }
        }

        private string CreatePath(string root, string folder)
        {
            string path = string.Format("{0}/{1}", root, folder);
            Directory.CreateDirectory(path);
            return path;
        }
        #endregion

        #region Info File
        private void CreateInfoFile()
        {
            infoFile = string.Format("{0}/info.json", batchPath);

            Batch batch = MountBatch();
            infoJson = JsonConvert.SerializeObject(batch, Formatting.Indented);

            CreateFile(infoFile, infoJson);
        }

        private void AddSimulationToInfoFile(Simulation simulation)
        {
            var batch = JsonConvert.DeserializeObject<Batch>(infoJson);
            batch.simulations.Add(simulation);
            infoJson = JsonConvert.SerializeObject(batch, Formatting.Indented);
            UpdateFile(infoFile, infoJson);
        }

        private void AddBatchEndToInfoFile(string end)
        {
            var batch = JsonConvert.DeserializeObject<Batch>(infoJson);

            batch.end = end;

            infoJson = JsonConvert.SerializeObject(batch, Formatting.Indented);
            UpdateFile(infoFile, infoJson);
        }

        private void AddSimulationEndToInfoFile(string end)
        {
            var batch = JsonConvert.DeserializeObject<Batch>(infoJson);

            var currentSimulation = batch.simulations[batch.simulations.Count - 1];
            currentSimulation.end = end;
            batch.simulations[batch.simulations.Count - 1] = currentSimulation;

            infoJson = JsonConvert.SerializeObject(batch, Formatting.Indented);
            UpdateFile(infoFile, infoJson);
        }

        private void UpdateConvergenceIntoInfoFile(string time)
        {
            var batch = JsonConvert.DeserializeObject<Batch>(infoJson);

            var currentSimulation = batch.simulations[batch.simulations.Count - 1];
            currentSimulation.convergedAt = time;
            batch.simulations[batch.simulations.Count - 1] = currentSimulation;

            infoJson = JsonConvert.SerializeObject(batch, Formatting.Indented);
            UpdateFile(infoFile, infoJson);
        }
        #endregion
    }
}