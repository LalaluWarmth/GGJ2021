using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BehaviorCD : MonoBehaviour
{
    public float behaviorCoolDown;
    private float _behaviorCoolDownLeft;
    public bool startCountingDown;

    public Text CDText;
    public RectTransform pin;

    void Start()
    {
        startCountingDown = false;
        _behaviorCoolDownLeft = behaviorCoolDown;
    }

    void Update()
    {
        if (startCountingDown)
        {
            _behaviorCoolDownLeft -= Time.deltaTime;
            if (_behaviorCoolDownLeft <= 0)
            {
                startCountingDown = false;
                _behaviorCoolDownLeft = behaviorCoolDown;
                Mathf.Max(0, _behaviorCoolDownLeft);
            }
        }

        CDText.text = _behaviorCoolDownLeft.ToString();
        pin.eulerAngles = new Vector3(0, 0, _behaviorCoolDownLeft / behaviorCoolDown * 360);
    }
}