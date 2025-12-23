using UnityEngine;

public class SwipeInputReader : MonoBehaviour
{
    private SnakeControls controls;
    private Vector2 touchStartPosition;
    private bool isTouching;

    void Awake() => controls = new SnakeControls();
    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        // Check for start of touch
        if (controls.Player.PrimaryContact.WasPerformedThisFrame())
        {
            touchStartPosition = controls.Player.PrimaryPosition.ReadValue<Vector2>();
            isTouching = true;
        }

        // Check for release
        if (controls.Player.PrimaryContact.WasReleasedThisFrame())
        {
            isTouching = false;
            //EventManager.TriggerSteer(0); // Reset steering
        }

        if (isTouching)
        {
            Vector2 currentTouch = controls.Player.PrimaryPosition.ReadValue<Vector2>();
            UseJoystickInput(currentTouch);
        }
    }

    private void UseJoystickInput(Vector2 value)
    {
        float angle = Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg;
        float steerValue = Mathf.Clamp(angle / 180f, -1f, 1f);
        EventManager.TriggerSteer(steerValue);
    }

    private void UseMouseInput(Vector2 value)
    {
        float swipeDelta = (value.x - touchStartPosition.x) / Screen.width;
        EventManager.TriggerSteer(Mathf.Clamp(swipeDelta * 2, -1f, 1f));
    }
}