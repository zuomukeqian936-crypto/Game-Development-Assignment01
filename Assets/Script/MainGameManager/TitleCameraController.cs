using Dreamteck;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TitleCameraController : MonoBehaviour
{
    [Header("Camera Move Settings")]
    [SerializeField] private float _cameraSpeed = 4f;

    [Header("Camera Target Positions"), Tooltip("カメラの移動場所リスト")]
    [SerializeField] private List<GameObject> _targets;

    [Header("Camera Distance Threshold")]
    [SerializeField] private float _cameraDistance = 0.1f;

    private Vector3 _currentPosition;
    private Vector3 _targetPosition;

    public bool CanMoving;

    void Start()
    {
        _currentPosition = Camera.main.transform.position;

        //Debug.Log(_currentPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanMoving) return;

        if(Vector2.Distance(_currentPosition, _targetPosition) < _cameraDistance)
        {
            MainCameraMoveDirection();
        }

        MainCameraMovement();
    }

    /// <summary>
    /// メインカメラの移動方向
    /// </summary>
    private void MainCameraMovement()
    {
        _currentPosition = Vector3.MoveTowards(_currentPosition, _targetPosition, _cameraSpeed * Time.deltaTime);

        Camera.main.transform.position = _currentPosition;
    }

    //カメラの移動方向の設定
    private void MainCameraMoveDirection()
    {
        
        int index = Random.Range(0, _targets.Count);

        //もし同じときは再抽選処理
        if(_currentPosition == new Vector3(_targets[index].transform.position.x, _targets[index].transform.position.y, Camera.main.transform.position.z))
        {
            MainCameraMoveDirection();
        }

        _targetPosition = new Vector3(_targets[index].transform.position.x, _targets[index].transform.position.y, Camera.main.transform.position.z);
    }
}
