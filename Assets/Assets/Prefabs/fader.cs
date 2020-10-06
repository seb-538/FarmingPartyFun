using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class fader : MonoBehaviour
{
    public Image fade;
    // Start is called before the first frame update
    void Start()
    {
        fade.canvasRenderer.SetAlpha(0.0f);
    }

    // Update is called once per frame
    public void fadeIn()
    {
        fade.CrossFadeAlpha(1, 1, true);
    }

    public void fadeOut()
    {
       fade.CrossFadeAlpha(0, 1, true);
    }
}
