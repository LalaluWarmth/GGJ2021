using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarController : MonoBehaviour
{
    public RectTransform toolsPanel;
    public List<GameObject> toolsList = new List<GameObject>();
    public List<GameObject> toolsInScene = new List<GameObject>();
    public Dictionary<GameObject, GameObject> toolDic = new Dictionary<GameObject, GameObject>();

    void Start()
    {
        InitToolDic();
        OrderTools();
    }


    void Update()
    {
    }

    private void InitToolDic()
    {
        for (int i = 0; i < toolsList.Count; i++)
        {
            toolDic.Add(toolsList[i], toolsInScene[i]);
        }
    }

    private void OrderTools()
    {
        Vector2 lastPos = new Vector2(40, 0);
        Vector2 panelSize = new Vector2(0, 0);
        foreach (var item in toolDic)
        {
            if (item.Key.activeSelf)
            {
                item.Key.GetComponent<RectTransform>().anchoredPosition = lastPos;
                lastPos.x += 60;
                panelSize.x = lastPos.x + 40;
            }
        }

        panelSize.x = panelSize.x - 60;
        toolsPanel.sizeDelta = panelSize;
    }

    public void AddToolToList(GameObject addGO)
    {
        foreach (var toolInDic in toolDic)
        {
            if (addGO == toolInDic.Value)
            {
                toolInDic.Value.SetActive(false);
                toolInDic.Key.SetActive(true);
                OrderTools();
                break;
            }
        }
    }

    public GameObject PickToolFromList(GameObject delGO)
    {
        foreach (var toolInDic in toolDic)
        {
            if (delGO == toolInDic.Key)
            {
                toolInDic.Key.SetActive(false);
                toolInDic.Value.SetActive(true);
                OrderTools();
                return toolInDic.Value;
            }
        }

        return null;
    }
}