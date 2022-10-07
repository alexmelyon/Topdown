using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// + MovePanel
///   + Out circle image
///     + Finger circle image
/// </summary>
public class ArcheroMovement : MonoBehaviour
{
    public enum SpeedType { LINEAR, ACCELERATE, MAX_SPEED }

    [Header("Childs")]
    public Image circle;
    public Image finger;

    [Header("Settings")]
    public SpeedType speedType;
    public AnimationCurve speedCurve = AnimationCurve.Linear(0, 0, 1, 1);


    [Header("Out")]
    //public Vector2 direction = new Vector2();
    public UnityEvent<Vector2> action;

    private Vector2 defPos;

    private void Start()
    {
        defPos = circle.transform.position;
    }

    void Update()
    {
        Camera _camera = null;

        bool isButtonDown = Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began;
        bool isButtonHold = Input.GetMouseButton(0) || Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved;
        bool isButtonUp = Input.GetMouseButtonUp(0) || Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended;
        Vector2 inputPos = new Vector2();
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            inputPos = Input.mousePosition;
        }
        else if (Input.touchCount > 0)
        {
            inputPos = Input.touches[0].position;
        }

        if (isButtonDown)
        {
            var rect = (RectTransform)transform;
            Vector2 point;
            bool res = RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, inputPos, _camera, out point);
            if (res)
            {
                circle.transform.position = point + defPos;
            }
        }
        else if (isButtonHold)
        {
            var rect = circle.rectTransform;
            Vector2 point;
            bool res = RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, inputPos, _camera, out point);
            if (res)
            {
                finger.transform.position = new Vector3(point.x, point.y, 0) + circle.transform.position;
            }
        }
        else if (isButtonUp)
        {
            finger.transform.position = circle.transform.position;
        }

        Vector2 diff = finger.transform.position - circle.transform.position;
        var radius = circle.rectTransform.sizeDelta.x / 2;
        Vector2 direction = diff / radius;
        if (diff.magnitude > radius)
        {
            direction = diff.normalized;
            finger.transform.position = direction * radius;
            finger.transform.position += circle.transform.position;
        }
        diff = finger.transform.position - circle.transform.position;
        direction = diff / radius;
        //Debug.Log("MAGNITUDE " + direction.magnitude);
        switch(speedType)
        {
            case SpeedType.ACCELERATE: direction *= speedCurve.Evaluate(direction.magnitude); break;
            case SpeedType.MAX_SPEED: direction = direction.normalized; break;
            default: break;
        }

        action.Invoke(direction);
    }
}