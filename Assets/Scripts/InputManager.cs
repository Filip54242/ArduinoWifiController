using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Joystick leftStick;
    public Joystick rightStick;
    public NetworkManager netManager;


    private int activatedDriveButton = 2;



    public void OnDriveButtonChanged(int number)
    {
        activatedDriveButton = number;
    }
    private string CreateMessage()
    {

        var direction = leftStick.GetData().y > 0 ? 1 : -1;
        var speed = Mathf.Abs(leftStick.GetData().y);
        var steeringAngle = Mathf.Abs(rightStick.GetData().x);
        return direction.ToString() + ',' + speed.ToString() + ',' + activatedDriveButton.ToString() + ',' + steeringAngle.ToString() + ',';
    }
    // Update is called once per frame
   private void Update() {
     netManager.UpdateMessage(CreateMessage());
   }
}
