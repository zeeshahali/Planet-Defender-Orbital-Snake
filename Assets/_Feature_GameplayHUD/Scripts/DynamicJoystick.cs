using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen; // Required for OnScreenStick

/// <summary>
/// A dynamic joystick that appears on touch, hides on release, and integrates with Unity's new Input System.
/// This script inherits from OnScreenStick.
/// Attach this to a UI Panel that serves as the touch-sensitive area.
/// The panel must have an Image component (can be transparent) with "Raycast Target" enabled.
///
/// Configuration:
/// 1. Assign your custom 'Joystick Base Visual', 'Joystick Handle Visual', and 'Joystick Direction Indicator' RectTransforms.
/// 2. In the Inspector, configure the 'Control Path' (inherited from OnScreenStick) to link to your Input Action (e.g., a Vector2 control like "Player/Move").
/// 3. Set the 'Movement Range' (inherited from OnScreenStick) to define the joystick's maximum travel distance in screen pixels.
/// </summary>
public class DynamicJoystick : OnScreenStick, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Custom Joystick Visuals")]
    [Tooltip("The RectTransform of the joystick's base visual element.")]
    [SerializeField] private RectTransform joystickBaseVisual;

    [Tooltip("The RectTransform of the joystick's handle visual element (the part that moves).")]
        
    [SerializeField] private RectTransform joystickHandleVisual;

    [Tooltip("The RectTransform of an optional visual element that rotates to indicate input direction. Should be a child of the handle or positioned with it.")]
    [SerializeField] private RectTransform joystickDirectionIndicator;

    // Note: 'movementRange' is inherited from OnScreenStick and should be configured in the Inspector.
    // Note: 'controlPath' is inherited from OnScreenControl (base of OnScreenStick) and should be configured in the Inspector.

    private Vector2 _joystickBaseFixedScreenPosition;
    private Canvas _canvas;         // The parent canvas.
    private Camera _eventCamera;    // The camera used for UI events (relevant for ScreenSpace-Camera or WorldSpace canvases).
    
    /// <summary>
    /// Initializes custom visuals and canvas references.
    /// The base OnScreenControl's input system setup is handled by its OnEnable method.
    /// </summary>
    protected void Start() // Removed 'override' and 'base.Start()'
    {
        if (joystickBaseVisual == null || joystickHandleVisual == null)
        {
            Debug.LogError("DynamicJoystick: Joystick Base Visual or Handle Visual is not assigned in the inspector. Disabling script.", this);
            enabled = false;
            return;
        }

        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
        {
            Debug.LogError("DynamicJoystick: This component must be a child of a Canvas. Disabling script.", this);
            enabled = false;
            return;
        }

        // Determine the camera for pointer events if the canvas is not in ScreenSpace-Overlay mode
        if (_canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            _eventCamera = _canvas.worldCamera;
            if (_eventCamera == null && _canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                 Debug.LogWarning("DynamicJoystick: Canvas is in ScreenSpaceCamera mode but no camera is assigned to the Canvas. Pointer events might not work as expected.", this);
            }
        }
    }

    /// <summary>
    /// Called by the EventSystem when a pointer down event occurs on this RectTransform.
    /// Shows and positions the joystick visuals at the touch point.
    /// </summary>
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!enabled || joystickBaseVisual == null || joystickHandleVisual == null) return;
        
        // Show and position the joystick base at the touch point
        joystickBaseVisual.position = eventData.position;
        _joystickBaseFixedScreenPosition = joystickBaseVisual.position;

        // Show and position the joystick handle at the center of the base
        joystickHandleVisual.position = _joystickBaseFixedScreenPosition;

        // Show direction indicator if assigned
        if (joystickDirectionIndicator != null)
        {
            joystickDirectionIndicator.position = joystickHandleVisual.position; // Align with handle
            joystickDirectionIndicator.eulerAngles = Vector3.zero; // Reset rotation
        }

        // Send initial zero value to the Input System via the inherited SendValueToControl method
        SendValueToControl(Vector2.zero);
    }

    /// <summary>
    /// Called by the EventSystem when a drag event occurs on this RectTransform.
    /// Moves the joystick handle, rotates the direction indicator, and sends the input vector to the Input System.
    /// </summary>
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (!enabled || joystickBaseVisual == null || !joystickBaseVisual.gameObject.activeSelf || joystickHandleVisual == null) return;

        Vector2 currentScreenTouchPosition = eventData.position;
        Vector2 directionFromBase = currentScreenTouchPosition - _joystickBaseFixedScreenPosition;
        float distance = directionFromBase.magnitude;

        // Use 'movementRange' (public property from OnScreenStick)
        float currentMovementRange = this.movementRange;

        Vector2 newHandlePosition;
        if (distance > currentMovementRange)
        {
            newHandlePosition = _joystickBaseFixedScreenPosition + directionFromBase.normalized * currentMovementRange;
        }
        else
        {
            newHandlePosition = currentScreenTouchPosition;
        }
        joystickHandleVisual.position = newHandlePosition;

        // Update direction indicator position
        if (joystickDirectionIndicator != null)
        {
            joystickDirectionIndicator.position = joystickHandleVisual.position;
        }

        Vector2 inputSystemVector = Vector2.zero;
        if (currentMovementRange > 0.001f) 
        {
            float clampedDistance = Mathf.Clamp(distance, 0, currentMovementRange);
            inputSystemVector = directionFromBase.normalized * (clampedDistance / currentMovementRange);
        }
        
        if (distance < 0.001f) // If joystick is (almost) at center
        {
            inputSystemVector = Vector2.zero;
            if (joystickDirectionIndicator != null)
            {
                // Optionally keep last rotation or reset:
                // joystickDirectionIndicator.eulerAngles = Vector3.zero; // Resets if at center
            }
        }
        else if (joystickDirectionIndicator != null) // Rotate indicator if there's input
        {
            // Calculate angle from the input vector (Y is up, X is right)
            // Mathf.Atan2 returns angle in radians, convert to degrees
            // Add 90 degrees if your indicator's 'forward' is 'up' by default. Adjust as needed.
            float angle = Mathf.Atan2(inputSystemVector.y, inputSystemVector.x) * Mathf.Rad2Deg;
            joystickDirectionIndicator.eulerAngles = new Vector3(0, 0, angle - 90); // Subtract 90 if sprite's 'up' is default forward
        }

        SendValueToControl(inputSystemVector);
    }

    
    /// <summary>
    /// Called by the EventSystem when a pointer up event occurs on this RectTransform.
    /// Hides the joystick visuals and resets the input in the Input System.
    /// </summary>
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (!enabled || joystickBaseVisual == null || joystickHandleVisual == null) return;
        
        SendValueToControl(Vector2.zero);
    }

    /// <summary>
    /// Called in the editor when a script variable is changed.
    /// Ensures movementRange (inherited) is not negative.
    /// </summary>
    private void OnValidate()
    {
        if (this.movementRange < 0)
        {
            this.movementRange = 0;
        }
    }
}
