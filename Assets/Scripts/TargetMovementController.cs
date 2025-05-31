using UnityEngine;

public class TargetMovementController : MonoBehaviour
{
    [SerializeField] private Transform waypoint;
    [SerializeField] private float moveDuration = 5f;
    [SerializeField] private float endPauseDuration = 2.5f;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    
    private float _timeElapsed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startPosition = transform.position;
        _endPosition = waypoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(_startPosition, _endPosition, Mathf.Clamp01(_timeElapsed / moveDuration));

        if (_timeElapsed >= moveDuration + endPauseDuration)
        {
            _timeElapsed = 0;
            transform.position = _startPosition;
        }
        else
        {
            _timeElapsed += Time.deltaTime;            
        }
    }
}
