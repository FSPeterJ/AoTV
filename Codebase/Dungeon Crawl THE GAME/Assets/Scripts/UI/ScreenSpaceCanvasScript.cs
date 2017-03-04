using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceCanvasScript : MonoBehaviour
{
    private List<DepthUIScript> panels = new List<DepthUIScript>();

    private void Awake()
    {
        panels.Clear();
    }

    private void Update()
    {
        Sort();
    }

    public void AddToCanvas(GameObject objectToAdd)
    {
        panels.Add(objectToAdd.GetComponent<DepthUIScript>());
    }

    private void Sort()
    {
        panels.Sort((x, y) => x.depth.CompareTo(y.depth));
        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i] != null)
            {
                panels[i].transform.SetSiblingIndex(i);
            }
        }
    }
}