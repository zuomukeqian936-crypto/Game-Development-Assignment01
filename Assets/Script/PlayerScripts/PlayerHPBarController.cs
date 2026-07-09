using UnityEngine;

public class PlayerHPBarController: MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private RectTransform _rectTransform;
    // Start is called before the first frame update
    public void Init()
    {
        _rectTransform = GetComponent<RectTransform>();

        if(_target == null)
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_target)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(_target.position + _offset);
            _rectTransform.position = screenPos;
        }
    }
}

