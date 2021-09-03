using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkbox : MonoBehaviour
{
    [SerializeField] GameObject checkmark;
    Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(SetCheckmark);
        SetCheckmark(toggle.isOn);
    }

    void SetCheckmark(bool isOn)
    {
        checkmark.SetActive(isOn);
    }

}
