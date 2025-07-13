using UnityEngine;

namespace Gameplay.Character
{
    public class HeadHitHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _headLayer;
        [SerializeField] private float _maxDistance = 100f;

        private Camera _cam;

        void Awake()
        {
            _cam = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, _maxDistance, _headLayer))
                {
                    var painter = hit.collider.GetComponent<DamagePainter>();
                    if (painter != null)
                    {
                        painter.PaintAt(hit.textureCoord);
                    }
                }
            }
        }
    }
}