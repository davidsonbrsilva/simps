using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class Manager : MonoBehaviour
    {
        #region Inspector

        #region Global Settings
        [Header("Configurações Gerais")]

        [SerializeField]
        [Tooltip("Selecione para definir o modo de simulação (somente com aprendizado, sem aprendizado ou alternado).")]
        private SimulationMode mode = SimulationMode.OnlyWithLearning;

        [SerializeField]
        [Tooltip("Informe a quantidade de simulações a serem executadas.")]
        private int batch = 30;

        [SerializeField]
        [Tooltip("Informe o tempo de duração máxima da simulação.")]
        private Timer maxSimulationTime;
        #endregion

        #region Current Simulation Stats
        [Header("Informações da Simulação Atual")]

        [SerializeField]
        [Tooltip("O identificador da simulação.")]
        private int id = 1;

        [SerializeField]
        [Tooltip("Tempo da simulação transcorrido.")]
        private float runtime = 0;

        [SerializeField]
        [Tooltip("Informa se as presas prodem aprender nessa simulação.")]
        private bool canLearn;

        [SerializeField]
        [Tooltip("Tempo de início da simulação.")]
        private DateTime start;

        [SerializeField]
        [Tooltip("Tempo de término da simulação.")]
        private DateTime end;

        [SerializeField]
        [Tooltip("Tempo da última gravação no disco.")]
        private DateTime lastSave;

        [SerializeField]
        [Tooltip("Tipo de simulação.")]
        private string type;

        #endregion
        #endregion

        #region Properties
        public bool CanLearn { get { return canLearn; } }
        public bool Restart { get; private set; }
        public bool Started { get; private set; }
        public bool Done { get; private set; }
        public bool AllDone { get; private set; }
        public float Runtime { get { return runtime; } private set { runtime = value; } }
        public float TotalTime { get; private set; }
        public int SimulationId { get { return id; } }
        public int Batch { get { return batch; } set { batch = value; } }
        public SimulationMode Mode { get { return mode; } }
        #endregion

        private void Awake()
        {
            DefineSettings();

            // Se o modo de simulação é sem convergência.
            if (mode == SimulationMode.OnlyWithoutLearning)
            {
                type = "Without learning";
                canLearn = false;
            }
            // Se o modo de simulação é com convergência...
            else if (mode == SimulationMode.OnlyWithLearning)
            {
                type = "With learning";
                canLearn = true;
            }
            // Se o modo de simulação é alternado...
            else if (mode == SimulationMode.Alternating)
            {
                type = "With learning";
                canLearn = true;
            }

            TotalTime = maxSimulationTime.GetTimeInSeconds();
        }

        private void Update()
        {
            Runtime += Time.deltaTime;

            if (Restart)
            {
                Restart = false;
            }

            if (Done)
            {
                Done = false;
                ResetSimulation();
            }

            // Se o tempo de execução da simulação atual terminou...
            if (runtime > TotalTime)
            {
                Debug.Log("e");
                Done = true;
                id++;

                // Se ainda há simulações para serem executadas...
                if (id <= batch)
                {
                    // Se o modo de simulação é alternado...
                    if (mode == SimulationMode.Alternating)
                    {
                        // Se o id é par, a simulação é sem convergência.
                        if (id % 2 == 0)
                        {
                            type = "Without learning";
                            canLearn = false;
                        }
                        // Se o id é impar, a simulação é com convergência.
                        else
                        {
                            type = "With learning";
                            canLearn = true;
                        }
                    }
                }
                else
                {
                    AllDone = true;

                    // Criar um controlador de scenes depois.
                    //SceneManager.LoadScene("results");
                }
            }
        }

        private void ResetSimulation()
        {
            Restart = true;
            runtime = 0f;
            start = DateTime.Now;
        }

        private void DefineSettings()
        {
            try
            {
                SettingsFileStreamer settings = GetComponent<SettingsFileStreamer>();

                if (settings != null)
                {
                    var data = settings.Data;

                    mode = settings.Data.mode;
                    maxSimulationTime.Hours = data.time.Hours;
                    maxSimulationTime.Minutes = data.time.Minutes;
                    maxSimulationTime.Seconds = data.time.Seconds;
                    batch = data.batch;
                }
            }
            catch (Exception e)
            {
                Debug.Log("O arquivo de configurações não foi carregado. Definindo simulações a partir das configurações do Inspector...");
            }

        }
    }
}