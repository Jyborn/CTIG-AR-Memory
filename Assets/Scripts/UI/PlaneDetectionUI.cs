using UnityEngine;

public class PlaneDetectionUI : MonoBehaviour
{

   public Canvas CanvasScanning;
   public Canvas GameCanvas;
   public void Start()
   {
      CanvasScanning.enabled = true;
      GameCanvas.enabled = false;
   }

   public void ChangeUI ()
   {
      CanvasScanning.enabled = false;
      GameCanvas.enabled = true;
   }
}
