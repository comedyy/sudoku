using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public SlotList[] possibles = new SlotList[9];
    GroupMgr groupMgr = new GroupMgr();
    InputField[,] listInputFiled = new InputField[9, 9];

    public bool x1;

    // Start is called before the first frame update
    void Start()
    {
        _reStart.onClick.AddListener(Restart);
    }

    void OnValueChange(int i, int j, int m)
    {
        possibles[i][j].SetValue((m));
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
                inputFiled = obj.GetComponent<InputField>();

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
                int x = i;
                int y = j;

                var inputFiled = listInputFiled[i, j];
                inputFiled.onValueChanged.AddListener((m)=>{
                    OnValueChange(x, y, int.Parse(m));
                });

                var monoPossible = inputFiled.gameObject.AddComponent<SlotPossibleValues>();
                monoPossible.Init(x, y, (m) =>
                {
                    if (m != inputFiled.text)
                    {
                        inputFiled.text = m;
                        inputFiled.textComponent.color = Color.green;
                    }
                });
                possibles[i][j] = monoPossible;

                if (inputFiled.text != "")
                {
                    var value = int.Parse(inputFiled.text);
                    inputParams.Add((i, j, int.Parse(inputFiled.text)));
                }
            }
        }

        groupMgr.Init(possibles);

        foreach(var x in inputParams)
        {
            OnValueChange(x.Item1, x.Item2, x.Item3);
        }
    }

    void Update(){
        if(x1)
        {
            x1 = false;
            foreach(var x in groupMgr.groupBlock[0].possibleValues)
            {
                Debug.LogError(string.Join(",", x.values));
            }
        }
    }
}
