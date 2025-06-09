using System.Collections.Generic;
using System.Linq;
using App.Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace App.Code
{
    public class MultipleImageTracking : MonoBehaviour
    {
        public Marker[] Markers;
        public ARTrackedImageManager TrackedImageManager;

        private Dictionary<string, Marker> _imagePrefabs;
        private Dictionary<string, ImageMarker> _imageInstances;

        private MutableRuntimeReferenceImageLibrary _imageLibrary;
        private int _jobCount;
        private IImageJobHandler _imageJobHandler;

        private IImageService ImageService =>
            Locator.Get<IImageService>();

        private void Awake()
        {
            TrackedImageManager.trackedImagesChanged += ImagesUpdated;

            _imagePrefabs = Markers.ToDictionary(x => x.Name);
            _imageInstances = new Dictionary<string, ImageMarker>();

#if !UNITY_EDITOR
            _imageLibrary = TrackedImageManager.CreateRuntimeLibrary() as MutableRuntimeReferenceImageLibrary;
            TrackedImageManager.referenceLibrary = _imageLibrary;
#endif
            _imageJobHandler = new ImageJobHandler(_imageLibrary, TrackedImageManager);
            for (var i = 0; i < Markers.Length; i++)
            {
                var marker = Markers[i];
                ImageService.GetImage(marker.Url, tex => _imageJobHandler.Add(marker.Name, marker.Width, tex));
            }
        }

        private void Update() =>
            _imageJobHandler.Run();

        private void OnDestroy() =>
            TrackedImageManager.trackedImagesChanged -= ImagesUpdated;

        private void ImagesUpdated(ARTrackedImagesChangedEventArgs obj)
        {
            foreach (var trackedImage in obj.added)
            {
                var imageName = trackedImage.referenceImage.name;
                var prefab = _imagePrefabs[imageName].Prefab;
                var instance = Instantiate(prefab);
                instance.Init(trackedImage);
                _imageInstances.Add(imageName, instance);
                instance.Spawned();
            }

            foreach (var trackedImage in obj.updated)
                UpdateMarker(trackedImage);

            foreach (var trackedImage in obj.removed)
                RemoveMarker(trackedImage);
        }

        private void UpdateMarker(ARTrackedImage trackedImage)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
                _imageInstances[trackedImage.referenceImage.name].Spawned();
            else
                _imageInstances[trackedImage.referenceImage.name].Removed();
        }

        private void RemoveMarker(ARTrackedImage trackedImage)
        {
            _imageInstances[trackedImage.referenceImage.name].Removed();
            _imageInstances.Remove(trackedImage.referenceImage.name);
        }

        private async UniTaskVoid Loaded(Marker marker, Texture2D tex)
        {
            Debug.Log(marker.Url);
            TrackedImageManager.enabled = false;
            _jobCount++;
            var job = _imageLibrary.ScheduleAddImageWithValidationJob(tex, marker.Name, marker.Width);
            var handle = job.jobHandle;
            await handle.ToUniTask(PlayerLoopTiming.Update);
            Debug.Log($"Image status: {job.status}");

            _jobCount--;
            if (_jobCount <= 0)
            {
                TrackedImageManager.enabled = true;
                TrackedImageManager.requestedMaxNumberOfMovingImages = Markers.Length;
            }
        }
    }
}