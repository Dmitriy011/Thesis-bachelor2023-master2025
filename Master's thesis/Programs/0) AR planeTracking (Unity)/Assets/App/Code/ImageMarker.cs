using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace App.Code
{
    public class ImageMarker : MonoBehaviour
    {
        public Transform Inner;

        [ShowInInspector] public ARTrackedImage TrackedImage { get; private set; }

        public void Init(ARTrackedImage trackedImage) =>
            TrackedImage = trackedImage;

        [Button(ButtonStyle.FoldoutButton), DisableInEditorMode]
        public void Spawned()
        {
            gameObject.SetActive(true);
            SetPosition();
        }

        public void Updated() =>
            SetPosition();

        [Button(ButtonStyle.FoldoutButton), DisableInEditorMode]
        public void Removed() =>
            gameObject.SetActive(false);

        private void SetPosition()
        {
            if (!TrackedImage) return;

            transform.position = TrackedImage.transform.position;
            transform.rotation = TrackedImage.transform.rotation;
            transform.rotation *= Quaternion.Euler(90.0f, 0.0f, 0.0f);
            var height = TrackedImage.referenceImage.height;
            var width = TrackedImage.referenceImage.width;
            var scale = Vector3.one * (height > width ? height : width);
            var aspect = width / height;
            var innerScale = Vector3.one;
            if (height > width)
                innerScale.x *= aspect;
            else
                innerScale.y *= 1 / aspect;

            transform.localScale = scale;
            Inner.localScale = innerScale;
        }
    }
}