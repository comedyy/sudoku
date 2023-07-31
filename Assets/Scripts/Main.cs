using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct SlotList
{
    public SlotPossibleValues[] _list;
    public SlotPossibleValues this[int index]{
        get
        {
            return _list[index];
        }
        set
        {
            _list[index] = value;
        }
    }
}

public class Main : MonoBehaviour
{
    public Button _reStart;
    public Button _level132;
    public Button _level131;
    public SlotList[] possibles = new SlotList[9];
    GroupMgr groupMgr = new GroupMgr();
    InputField[,] listInputFiled = new InputField[9, 9];

    // Start is called before the first frame update
    void Start()
    {
        _reStart.onClick.AddListener(Restart);
        _level132.onClick.AddListener(OnClick132);
        _level131.onClick.AddListener(OnClick131);
    }

    private void OnClick131()
    {
        SceneManager.LoadSceneAsync("Scenes/131");
    }

    private void OnClick132()
    {
        SceneManager.LoadSceneAsync("Scenes/132");
    }

    private void Restart()
    {
        _reStart.gameObject.SetActive(false);
        for(int i = 0; i < 9; i++)
        {
            var child = transform.GetChild(i);
            for(int j = 0; j < 9; j++)
            {
                var obj = child.GetChild(j);
                var inputFiled = obj.GetComponent<InputField>();

                var x1 = i % 3 * 3;
                var y1 = i / 3 * 3;
                var x2 = j % 3;
                var y2 = j / 3;
                listInputFiled[y1 + y2, x1 + x2] = inputFiled;
            }
        }

        List<(int, int, int)> inputParams = new List<(int, int, int)>();
        for(int i = 0; i < 9; i++)
        {
            possibles[i]._list = new SlotPossibleValues[9];
            for(int j = 0; j < 9; j++)
            {
                var inputFiled = listInputFiled[i, j];
                var monoPossible = inputFiled.gameObject.AddComponent<SlotPossibleValues>();
                possibles[i][j] = monoPossible;

                if (inputFiled.text != "")
                {
                    inputParams.Add((i, j, int.Parse(inputFiled.text)));
                }
            }
        }

        groupMgr.Init(possibles);
        foreach(var x in inputParams)
        {
            possibles[x.Item1][x.Item2].SetValue(x.Item3, true);
        }

        if(!groupMgr.IsWin())
        {
            SlotPossibleValues item = GetMinPossibleNode();

            for(int i = 0; i < item.values.Count; i++)
            {
                List<(int, int, int)> inputParams1 = new List<(int, int, int)>(inputParams)
                {
                    (item.x1, item.y1, item.values[i])
                };
                
                if(GetResult(inputParams1))
                {
                    break;
                }
            }
        }

        // show Values
        foreach(var x in listInputFiled)
        {
            var value = x.GetComponent<SlotPossibleValues>();
            if(value.currentValue != -1 && !inputParams.Exists(m=>m.Item1 == value.x1 && m.Item2 == value.y1))
            {
                x.textComponent.color = Color.green;
                x.text = value.currentValue.ToString();
            }
        }
    }

    private SlotPossibleValues GetMinPossibleNode()
    {
        int minCount = 10;
        SlotPossibleValues minValue = null;
        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                var item = possibles[i][j];
                var count = item.values.Count;
                if(minCount > count && count > 1){
                    minCount = count;
                    minValue = possibles[i][j];
                }
            }
        }

        return minValue;
    }

    private bool GetResult(List<(int, int, int)> inputParams)
    {
        groupMgr.Init(possibles);
        foreach(var x in inputParams)
        {
            possibles[x.Item1][x.Item2].SetValue(x.Item3, true);
        }

        if(groupMgr.IsWin())
        {
            return true;
        }
        else
        {
            SlotPossibleValues item = GetMinPossibleNode();

            for(int i = 0; i < item.values.Count; i++)
            {
                List<(int, int, int)> inputParams1 = new List<(int, int, int)>(inputParams)
                {
                    (item.x1, item.y1, item.values[i])
                };
                
                if(GetResult(inputParams1))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
