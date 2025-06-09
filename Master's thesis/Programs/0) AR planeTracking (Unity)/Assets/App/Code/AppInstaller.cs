using App.Code.Services;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace App.Code
{
    public class AppInstaller : MonoBehaviour
    {
        public EditorRaycastService EditorRaycastService;
        public ARRaycastManager ARRaycastManager;
        public InputService InputService;

        private void Awake()
        {
            Locator.Set<IInputService>(InputService);
            Locator.Set<IImageService>(new ImageService());
#if UNITY_EDITOR
            Locator.Set<IRaycastService>(EditorRaycastService);
#else
            Locator.Set<IRaycastService>(new ARRaycastService(ARRaycastManager));
#endif
        }
    }
}