using UnityEngine;

public class EndLevelDoorGUI : MonoBehaviour
{
    [SerializeField] private GameObject _endLevelDoor;

    private GUIStyle _guiStyle = new GUIStyle();

    private EndLevelDoor _door;

    private void Awake()
    {
        _door =  _endLevelDoor.GetComponent<EndLevelDoor>();
    }
    private void OnGUI()
    {
        if (_door.IsPlayerNear)
        {


            if (_door.IsAllLeversDown)
            {
                SetBoxAndStyle(true);              
            }
            else
            {
                SetBoxAndStyle(false);

                _guiStyle.fontStyle = FontStyle.Normal;
                GUI.Label(new Rect(Screen.width / 2 - 95, Screen.height / 2 - 80, 190, 80), "На востоке и западе ущелья расположены колонны." +
                    "Найди их. Они откроют тебе путь!", _guiStyle);
            }
            
        }
    }

    private void SetBoxAndStyle(bool isDoorOpen)
    {
        string messege = isDoorOpen switch  //чтоб не забывать про выражение switch, тут конечно тернарный оператор зашел бы лучше)))
        {
            true => "Путь открыт",
            false => "Дверь заблокирована!"
        };

        int margin = (isDoorOpen) ? 100 : 90;
        
        GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 100), "");
        _guiStyle.normal.textColor = Color.white;
        _guiStyle.fontStyle = FontStyle.Bold;
        _guiStyle.alignment = TextAnchor.MiddleCenter;
        _guiStyle.wordWrap = true;

        GUI.Label(new Rect(Screen.width / 2 - 95, Screen.height / 2 - margin, 190, 90), messege, _guiStyle);
    }


}
