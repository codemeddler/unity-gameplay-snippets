using UnityEngine;
using UnityEngine.Serialization;

public class Rotator : MonoBehaviour
{
    [FormerlySerializedAs("m_fSpeed")] public float Speed;
    [FormerlySerializedAs("m_bRotateX")] public bool RotateX;
    [FormerlySerializedAs("m_bRotateY")] public bool RotateY;
    [FormerlySerializedAs("m_bRotateZ")] public bool RotateZ;

    private Transform _mTransform;

    private void Start()
    {
        _mTransform = transform;
    }

    private void Update()
    {
        if (RotateZ)
            _mTransform.RotateAround(
                _mTransform.position,
                _mTransform.forward,
                Speed * Time.deltaTime);
        if (RotateX)
            _mTransform.RotateAround(
                _mTransform.position,
                _mTransform.right,
                Speed * Time.deltaTime);
        if (RotateY)
            _mTransform.RotateAround(
                _mTransform.position,
                _mTransform.up,
                Speed * Time.deltaTime);
    }
}