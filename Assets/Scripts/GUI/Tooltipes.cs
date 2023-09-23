using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltipes : MonoBehaviour
{
    private GUIStyle _guiStyle = new GUIStyle();
    private bool _isPause;
    public bool IsPause { set => _isPause = value; }

    private void OnGUI()
    {
        if (!_isPause)
        {
            GUI.Box(new Rect(5, 60, Screen.width / 5, 100), "Подсказки");
            _guiStyle.normal.textColor = Color.white;

            DrawButtonNameLabel(80, "P");
            DrawTooltipeLabel(80, "- изменить цвет");
            GUI.Label(new Rect(10, 90, 1, 1), "освещения", _guiStyle);

            
            DrawButtonNameLabel(110, "Q");
            DrawTooltipeLabel(110, "- выбрать оружие");

            DrawButtonNameLabel(130, "E");
            DrawTooltipeLabel(130, "- вкл/выкл фонарик");
        }
    }

    private void DrawButtonNameLabel (int y, string messege)
    {
        _guiStyle.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(10, y, 1, 1), messege, _guiStyle);
    }

    private void DrawTooltipeLabel(int y, string messege)
    {
        _guiStyle.fontStyle = FontStyle.Normal;
        GUI.Label(new Rect(22, y, 1, 1), messege, _guiStyle);
    }
}
