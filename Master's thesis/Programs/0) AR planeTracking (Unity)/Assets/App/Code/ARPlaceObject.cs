using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace App.Code
{
    public class ARPlaceObject : MonoBehaviour
    {
        public GameObject ObjectToPlace;
        public GameObject SphereToPlace;

        private List<GameObject> _instances;

        private IInputService _inputService;
        private IRaycastService _raycastService;

        private void Awake()
        {
            _inputService = Locator.Get<IInputService>();
            _raycastService = Locator.Get<IRaycastService>();

            _inputService.OnClick += OnClick;
            _instances = new List<GameObject>();
        }

        private void OnDestroy() =>
            _inputService.OnClick -= OnClick;

        public void Clear()
        {
            foreach (var instance in _instances)
                Destroy(instance);

            _instances.Clear();
        }

        private void SpawnPrefab(GameObject obj, Vector3 position) =>
            _instances.Add(Instantiate(obj, position, Quaternion.identity));

        private void OnClick(PointerEventData pointerEventData)
        {
            var result = _raycastService.Raycast(pointerEventData.position);
            if (result != null)
                SpawnPrefab(result.PlaneType == PlaneAlignment.Vertical
                        ? ObjectToPlace
                        : SphereToPlace,
                    result.HitPoint);
        }
    }
}