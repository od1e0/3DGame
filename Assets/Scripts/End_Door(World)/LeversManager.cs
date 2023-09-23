using UnityEngine;

public class LeversManager : MonoBehaviour
{
    private static LeversManager _instance;
    private int _downLeversCount;
    public int DownLeversCount { get => _downLeversCount; }
    public static LeversManager Instance
    {
        get
        {
            if (_instance == null)

            {
                _instance = FindObjectOfType<LeversManager>();
                if (_instance == null)

                {
                    GameObject _gameObject = new GameObject();
                    _gameObject.name = "LeversManager";
                    _instance = _gameObject.AddComponent<LeversManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddDownLeversCount()
    {
        _downLeversCount++;
    }
}