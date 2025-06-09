using System;
using System.Collections.Generic;
using System.Net.Http;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Code.Services
{
    public class ImageService : IImageService
    {
        private readonly Dictionary<string, Action<Texture2D>> _currentDownloads;
        private readonly Dictionary<string, Texture2D> _downloaded;

        public ImageService()
        {
            _currentDownloads = new Dictionary<string, Action<Texture2D>>();
            _downloaded = new Dictionary<string, Texture2D>();
        }

        public void GetImage(string url, Action<Texture2D> onDownloaded)
        {
            if (_downloaded.ContainsKey(url))
            {
                onDownloaded?.Invoke(_downloaded[url]);
                return;
            }

            if (_currentDownloads.ContainsKey(url))
                _currentDownloads[url] += onDownloaded;
            else
            {
                _currentDownloads.Add(url, onDownloaded);
                StartDownload(url);
            }
        }

        private async UniTaskVoid StartDownload(string url)
        {
            using var httpClient = new HttpClient();
            var bytes = await httpClient.GetByteArrayAsync(url);
            var texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            Complete(url, texture);
        }

        private void Complete(string url, Texture2D texture)
        {
            _downloaded.Add(url, texture);
            _currentDownloads[url]?.Invoke(texture);
            _currentDownloads.Remove(url);
        }
    }
}