using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    [SerializeField] Text _txtInEnd;
    [SerializeField] GameObject _player;
    [SerializeField] Button _btnOk;

    private void Awake()
    {
        _btnOk.onClick.AddListener(QuitGame);
        if (_player.GetComponent<PlayerActions>().IsDead)
        {
            _txtInEnd.text = "GAME OVER";
        } else
        {
            _txtInEnd.text = "YOU WIN!!!";
        }
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }
}
