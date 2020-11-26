using UnityEngine;

namespace SIMPS
{
    /// <summary>
    /// Main Camera Controller
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        // Attributes

        [SerializeField] private float speed = 50f;

        private Camera camera;
        private SpriteRenderer map;
        private Selector selector;
        private Vector3 targetPosition;
        private Vector3 movementVelocity;
        private Vector3 minBounds;
        private Vector3 maxBounds;
        private float halfWidth;
        private float halfHeight;

        private Vector3 oldPosition;

        // Methods

        private void Awake()
        {
            camera = GetComponent<Camera>();
            map = GameObject.FindWithTag("Map").GetComponent<SpriteRenderer>();
            selector = GameObject.FindWithTag("Core").GetComponent<Selector>();

            targetPosition = camera.transform.position;
            movementVelocity = Vector3.zero;

            minBounds = map.bounds.min;
            maxBounds = map.bounds.max;
        }

        private void LateUpdate()
        {
            if (selector.SelectedAgent != null)
            {
                targetPosition = new Vector3(selector.SelectedAgent.transform.position.x, selector.SelectedAgent.transform.position.y, transform.position.z);
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    oldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
                    return;
                }

                if (!Input.GetMouseButton(0)) { return; }


                Vector3 newPosition = camera.ScreenToWorldPoint(Input.mousePosition - oldPosition);
                targetPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            }

            halfWidth = camera.orthographicSize * Screen.width / Screen.height;
            halfHeight = camera.orthographicSize;

            float clampedX = Mathf.Clamp(targetPosition.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
            float clampedY = Mathf.Clamp(targetPosition.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
            targetPosition = new Vector3(clampedX, clampedY, targetPosition.z);

            if (camera.transform.position != targetPosition)
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref movementVelocity, Time.deltaTime * speed);
            }
        }
    }
}

