using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace App.Code
{
    public class EditorRaycastInfo : IRaycastInfo
    {
        public Vector3 HitPoint { get; }
        public PlaneAlignment PlaneType { get; }
        public GameObject HitObject { get; }

        public EditorRaycastInfo(RaycastHit raycastHit)
        {
            HitPoint = raycastHit.point;
            HitObject = raycastHit.collider.gameObject;
            PlaneType = HitObject.GetComponent<EditorPlane>().Alignment;
        }
    }
}