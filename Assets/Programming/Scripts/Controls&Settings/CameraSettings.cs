using NaughtyAttributes;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "", order = 1)]
public class CameraSettings : ScriptableObject
{
    public enum CameraOfType
    {
        Shooter,
        MoveAroundChara,
        MoveChara,
    }

    [Serializable]
    public struct CameraOptions
    {
        public CameraOfType CameraType;

        public float sensitivity;

        public float distance;

        public Vector3 offset;

        [MinMaxSlider(-180, 180)] public Vector2 rotationClamp;
    }
    public CameraOptions Options;
}
