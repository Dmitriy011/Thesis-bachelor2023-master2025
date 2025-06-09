using UnityEngine;

namespace App.Code
{
    public interface IImageJobHandler
    {
        public void Add(string name, float width, Texture2D tex);
        public void Run();
    }
}