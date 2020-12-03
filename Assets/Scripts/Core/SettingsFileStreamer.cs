using System.IO;
using UnityEngine;

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
            string filename = Application.dataPath + "/settings.json";

            if (!File.Exists(filename))
            {
                File.Create(filename).Dispose();

                Settings settings = new Settings(30, 10, 6, 1, 1, 1, 0.9f, new Timer(2, 0, 0), SimulationMode.Alternating);

                //var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                var json = JsonUtility.ToJson(settings, true);

                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.Write(json);
                }
            }

            using (StreamReader sr = new StreamReader(filename))
            {
                string json = sr.ReadToEnd();
                //Data = JsonConvert.DeserializeObject<Settings>(json);
                Data = JsonUtility.FromJson<Settings>(json);
                Debug.Log(Data);
            }
        }
    }
}
