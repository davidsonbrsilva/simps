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

        private void Awake()
        {
            var bounds = boundsObject.GetComponent<BoxCollider2D>();
            var wall = wallObject.GetComponent<BoxCollider2D>();
            var wallSprite = wallSpriteObject.GetComponent<SpriteRenderer>();

            wall.size = new Vector2(wall.size.x, bounds.size.y * ratio);
            wallSprite.size = new Vector2(wall.size.x, bounds.size.y * ratio);
        }
    }
}