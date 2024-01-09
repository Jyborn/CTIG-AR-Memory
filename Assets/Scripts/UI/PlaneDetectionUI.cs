using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaneDetectionUI : MonoBehaviour
{

   public Canvas CanvasScanning;
   public Canvas GameCanvas;

   public void Start()
   {
      CanvasScanning.enabled = true;
      GameCanvas.enabled = false;
   }

   public void OnNextButton ()
   {
      CanvasScanning.enabled = false;
      GameCanvas.enabled = true;
   }
}
