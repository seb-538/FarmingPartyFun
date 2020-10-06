using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lettre : MonoBehaviour
{
    public List<GameObject> Pages;
    public GameObject ActivePage;


    public void Next()
    {
        int i = 0;
        if (ActivePage == Pages[Pages.Count - 1])
        {
            ActivePage.SetActive(false);
            ActivePage = null;
        }
        while (Pages[i] != Pages[Pages.Count - 1])
         {
           if (Pages[i] == ActivePage)
            {
                Pages[i].SetActive(false);
                Pages[i + 1].SetActive(true);
                ActivePage = Pages[i + 1];
                break;
            }
            i++;
        }
    }
}
