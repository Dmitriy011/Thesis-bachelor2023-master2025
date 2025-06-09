using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace App.Code.Services
{
    public class ImageServiceTest : MonoBehaviour
    {
        public string ImageUrl;
        public RawImage Output;

        private IImageService ImageService =>
            Locator.Get<IImageService>();

        [Button(ButtonStyle.FoldoutButton), DisableInEditorMode]
        private void Load() =>
            ImageService.GetImage(ImageUrl, tex => Output.texture = tex);
    }
}