using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SIMPS
{
    public class SettingsFileStreamer : MonoBehaviour
    {
        public Settings Data { get; private set; }

        private void Awake()
        {
            ReadOrCreate();
            //DontDestroyOnLoad(transform.gameObject);
        }

        private void Start()
        {
            //SceneManager.LoadScene("Simulator");
        }

        private void ReadOrCreate()
        {
            string filename = "settings.json";

            if (!File.Exists(filename))
            {
                File.Create(filename).Dispose();

                Settings settings = new Settings(30, 6, 1, 1, 1, new Timer(2, 0, 0), SimulationMode.Alternating);

                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);

                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.Write(json);
                }
            }
            using (StreamReader sr = new StreamReader("settings.json"))
            {
                string json = sr.ReadToEnd();
                Data = JsonConvert.DeserializeObject<Settings>(json);
            }
        }
    }
}
