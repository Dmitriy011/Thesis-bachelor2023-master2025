using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace App.Code
{
    public class ImageJobHandler : IImageJobHandler
    {
        private class Job
        {
            public string Name { get; }
            public float Width { get; }
            public Texture2D Texture { get; }

            public bool IsCompleted
            {
                get
                {
                    if (!_addImageJobHandle.HasValue)
                        return false;

                    return _addImageJobHandle.Value.IsCompleted;
                }
            }

            private AddReferenceImageJobState _addImageJob;
            private JobHandle? _addImageJobHandle;

            public Job(string name, float width, Texture2D texture)
            {
                Name = name;
                Width = width;
                Texture = texture;
            }

            public void StartJob(MutableRuntimeReferenceImageLibrary imageLibrary)
            {
                _addImageJob = imageLibrary.ScheduleAddImageWithValidationJob(Texture, Name, Width);
                _addImageJobHandle = _addImageJob.jobHandle;
            }
        }

        private readonly Queue<Job> _jobs;
        private readonly MutableRuntimeReferenceImageLibrary _imageLibrary;
        private readonly ARTrackedImageManager _trackedImageManager;

        private Job _currentJob;
        private bool _isDone;

        public ImageJobHandler(MutableRuntimeReferenceImageLibrary imageLibrary,
            ARTrackedImageManager trackedImageManager)
        {
            _imageLibrary = imageLibrary;
            _trackedImageManager = trackedImageManager;
            _jobs = new Queue<Job>();
        }

        public void Add(string name, float width, Texture2D tex)
        {
            _jobs.Enqueue(new Job(name, width, tex));
            _isDone = false;
        }

        public void Run()
        {
            if (_isDone) return;

            if (_currentJob != null)
            {
                if (!_currentJob.IsCompleted)
                    return;

                _currentJob = null;
            }

            if (_jobs.Count == 0)
            {
                _trackedImageManager.enabled = true;
                _isDone = true;
                return;
            }

            _currentJob = _jobs.Dequeue();
            _trackedImageManager.enabled = false;
            _currentJob.StartJob(_imageLibrary);
        }
    }
}