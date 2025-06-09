using System;
using UnityEngine;

namespace App.Code.Services
{
    public interface IImageService
    {
        public void GetImage(string url, Action<Texture2D> onDownloaded);
    }
}