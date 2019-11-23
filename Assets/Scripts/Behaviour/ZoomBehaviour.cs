using UnityEngine;

namespace Behaviour
{
    public class ZoomBehaviour : MonoBehaviour
    {
        [SerializeField] private Vector3 zoomedScale;
        [SerializeField] private double secondsZoomed;

        private Vector3 _defaultScale;
        private bool _isDoubleTapping;
        private bool _isZoomed;
        private float _timer;

        private void Start()
        {
            _defaultScale = transform.localScale;

            if (zoomedScale.Equals(Vector3.zero))
            {
                zoomedScale = _defaultScale * 1.5f;
            }
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            CheckDoubleTap();

            if (_isDoubleTapping && !_isZoomed)
            {
                ActivateZoom();
                _isZoomed = true;
            }
            else if (_isZoomed)
            {
                if (_timer <= secondsZoomed) return;

                DeactivateZoom();
                ResetTimer();
                _isZoomed = false;
            }
        }

        private void CheckDoubleTap()
        {
            _isDoubleTapping = false;

            foreach (var touch in Input.touches)
            {
                if (touch.tapCount != 2) continue;

                _isDoubleTapping = true;
                Debug.Log("ZOOM: double tap detected");
            }
        }

        private void ActivateZoom()
        {
            var p = transform.position;

            Debug.Log("ZOOM: activating");
            transform.localScale = zoomedScale;
            transform.position = new Vector3(p.x, p.y / 2, p.z);
        }

        private void DeactivateZoom()
        {
            var p = transform.position;
            
            Debug.Log("ZOOM: deactivating");
            transform.localScale = _defaultScale;
            transform.position = new Vector3(p.x, p.y * 2, p.z);
        }

        private void ResetTimer()
        {
            _timer = 0;
        }
    }
}