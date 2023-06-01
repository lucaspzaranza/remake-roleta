using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFullscreen : MonoBehaviour
{
    void Start()
    {
        print("fullscreen");
        Screen.fullScreen = true;
    }
}
