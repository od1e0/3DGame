using UnityEngine;

public class EndLevelDoor : MonoBehaviour
{
    private bool _isAllLeversDown;
    private bool _isPlayerNear;
    private const int _targetLeversCount = 2;


    public bool IsAllLeversDown => _isAllLeversDown;
    public bool IsPlayerNear => _isPlayerNear;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = true;

            if (LeversManager.Instance.DownLeversCount == _targetLeversCount)
            {
                _isAllLeversDown = true;
            } 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isPlayerNear = false;
    }

}
