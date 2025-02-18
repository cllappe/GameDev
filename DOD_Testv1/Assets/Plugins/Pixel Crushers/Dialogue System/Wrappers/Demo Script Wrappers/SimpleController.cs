// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers.DialogueSystem.Demo.Wrappers
{

    /// <summary>
    /// This wrapper class keeps references intact if you switch between the 
    /// compiled assembly and source code versions of the original class.
    /// </summary>
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/simple_controller.html")]
    [AddComponentMenu("Pixel Crushers/Dialogue System/Actor/Demo/Simple Controller")]
    [RequireComponent(typeof(CharacterController))]
    public class SimpleController : PixelCrushers.DialogueSystem.Demo.SimpleController
    {
    }

}
