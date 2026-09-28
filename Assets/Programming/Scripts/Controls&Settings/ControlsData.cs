using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ControlSettings", menuName = "", order = 1)]

public class ControlsData : ScriptableObject
{
    //public enum MovementControlType
    //{
    //    Keyboard,
    //    Mouse,
    //    KeyboardMouse
    //}
    //public MovementControlType MovementType;
    //public enum ControlType
    //{
    //    Platformer,
    //    Shooter,
    //}
    //public ControlType TypeControl;
    //public InputActionAsset CharacterControl;

    public enum MovementType
    {
        RotationWithMouse,
        RotationWithKeys,
        SideSteps,
        Other,
    }
    public MovementType MoveType;


    private void Awake()
    {

    }
}