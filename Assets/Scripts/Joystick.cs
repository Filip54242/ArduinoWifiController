using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;


public enum JoystickType
{
    ConstrainedVertical,
    ConstrainedHorisontal,
    Free
}

public class Joystick : MonoBehaviour
{
    public JoystickType typeOfJoystick;
    [Header("Return data interval")]
    public int x;
    public int y;

    private GameObject joystickKnob;
    private Slider horisontalSlider;
    private Slider verticalSlider;
    private Button horisontalSliderButton;
    private Button verticalSliderButton;
    private int maxValue = 250;
    private int minValue = -250;

    // Start is called before the first frame update



    private void Start()
    {
        joystickKnob = transform.Find("Knob").gameObject;
        horisontalSlider = transform.Find("HorisontalSlider").GetComponentInChildren<Slider>();
        verticalSlider = transform.Find("VerticalSlider").GetComponentInChildren<Slider>();


        switch (typeOfJoystick)
        {
            case JoystickType.ConstrainedVertical:
                InitVerticalSlider();
                horisontalSlider.gameObject.SetActive(false);
                break;
            case JoystickType.ConstrainedHorisontal:
                InitHorisontalSlider();
                verticalSlider.gameObject.SetActive(false);
                break;
            case JoystickType.Free:
                InitHorisontalSlider();
                InitVerticalSlider();
                break;
            default:
                break;
        }

    }
    private void Update()
    {
        CheckSlider();
        CheckKnob();
    }

    private int NormalizeData(float value)
    {
        return (y - x) * ((int)value - minValue) / (maxValue - minValue) + x;
    }
    public Vector2Int GetRawData()
    {
        return new Vector2Int((int)joystickKnob.transform.localPosition.x, (int)joystickKnob.transform.localPosition.y);
    }
    public Vector2Int GetData()
    {
        return new Vector2Int(NormalizeData(joystickKnob.transform.localPosition.x), NormalizeData(joystickKnob.transform.localPosition.y));
    }
    private void InitHorisontalSlider()
    {
        horisontalSliderButton = horisontalSlider.gameObject.transform.Find("ResetButton").GetComponentInChildren<Button>();
        horisontalSliderButton.onClick.AddListener(() => joystickKnob.transform.localPosition = Vector3.zero);
    }

    private void InitVerticalSlider()
    {
        verticalSliderButton = verticalSlider.gameObject.transform.Find("ResetButton").GetComponentInChildren<Button>();
        verticalSliderButton.onClick.AddListener(() => joystickKnob.transform.localPosition = Vector3.zero);
    }



    private void CheckSlider()
    {
        if (horisontalSlider.value != 0)
        {
            joystickKnob.transform.localPosition = new Vector3(horisontalSlider.value, 0);
        }
        else if (verticalSlider.value != 0)
        {
            joystickKnob.transform.localPosition = new Vector3(0, verticalSlider.value);
        }

    }
    private void CheckKnob()
    {
        switch (typeOfJoystick)
        {
            case JoystickType.ConstrainedVertical:
                joystickKnob.transform.localPosition = new Vector3(0, joystickKnob.transform.localPosition.y);
                break;
            case JoystickType.ConstrainedHorisontal:
                joystickKnob.transform.localPosition = new Vector3(joystickKnob.transform.localPosition.x, 0);
                break;
        }
    }




}
