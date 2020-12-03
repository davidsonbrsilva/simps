using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class WallController : MonoBehaviour
    {
        [SerializeField] private float ratio;
        [SerializeField] private GameObject boundsObject;
        [SerializeField] private GameObject wallObject;
        [SerializeField] private GameObject wallSpriteObject;

        public float Ratio { get { return ratio; } }

        private void Awake()
        {
            DefineSettings();

            var bounds = boundsObject.GetComponent<BoxCollider2D>();
            var wall = wallObject.GetComponent<BoxCollider2D>();
            var wallSprite = wallSpriteObject.GetComponent<SpriteRenderer>();

            wall.size = new Vector2(wall.size.x, bounds.size.y * ratio);
            wallSprite.size = new Vector2(wall.size.x, bounds.size.y * ratio);
        }

        private void DefineSettings()
        {
            try
            {
                SettingsFileStreamer settings = GameObject.FindWithTag("Core").GetComponent<SettingsFileStreamer>();

                if (settings != null)
                {
                    var data = settings.Data;

                    ratio = settings.Data.wallRatioSize;
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("O arquivo de configurações não foi carregado. Definindo simulações a partir das configurações do Inspector...");
            }
        }
    }
}