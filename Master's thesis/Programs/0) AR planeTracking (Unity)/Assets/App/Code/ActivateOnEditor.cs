using UnityEngine;

namespace App.Code
{
    public class ActivateOnEditor : MonoBehaviour
    {
        public GameObject EditorObject;

        private void Awake()
        {
#if UNITY_EDITOR
            EditorObject.SetActive(true);
#else
            EditorObject.SetActive(false);
#endif
        }
    }
}