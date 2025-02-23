using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Reference: https://payhip.com/UnityCoderz/blog/unity-tutorials/how-to-add-camera-shake-in-unity-2d
/// </summary>
public class CameraShake : MonoBehaviour
{
    private float shakeDuration = 0f; // Ensure this starts at 0
    public float shakeMagnitude = 0.1f;
    public float dampingSpeed = 1f;

    private Vector3 originalPosition;

    void Awake()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            // Apply shake effect
            transform.position = originalPosition + Random.insideUnitSphere * shakeMagnitude;

            // Decrease shake duration
            shakeDuration -= Time.deltaTime * dampingSpeed;

            // Reset position when duration ends
            if (shakeDuration <= 0)
            {
                shakeDuration = 0f;
                transform.position = originalPosition;
            }
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
