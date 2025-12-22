using UnityEngine;
using UnityEngine.InputSystem;

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
            EventManager.TriggerSteer(0); // Reset steering
        }

        if (isTouching)
        {
            Vector2 currentTouch = controls.Player.PrimaryPosition.ReadValue<Vector2>();
            float swipeDelta = (currentTouch.x - touchStartPosition.x) / Screen.width;
            
            // Send the steer value (-1 to 1) through the EventManager
            EventManager.TriggerSteer(Mathf.Clamp(swipeDelta * 2, -1f, 1f));
        }
    }
}