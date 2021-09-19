using UnityEngine;

public class RotateToGravityBase : MonoBehaviour
{
    private enum TimingFunction
    {
        linear, easeInOutSine, easeInOutCubic, easeInOutQuint, easeInOutCirc
    }

    [SerializeField]
    private float rotationDuration = 1;

    [SerializeField]
    private TimingFunction timingFunction = TimingFunction.linear;

    private float initialRotationZ;
    private float targetRotationZ;
    private float rotationStartTime;
    private float rotationChange;
    private bool isRotationInProgress = false;

    private void Update()
    {
        if (isRotationInProgress)
        {
            if (Mathf.Abs(rotationChange) < 90)
            {
                float timeFraction = (Time.time - rotationStartTime) / rotationDuration;
                rotationChange = timing(timeFraction) * (targetRotationZ - initialRotationZ);
                float newRotationZ = initialRotationZ + rotationChange;
                transform.rotation = Quaternion.Euler(0, 0, newRotationZ);
            }
            else
            {
                if (targetRotationZ == 360) transform.rotation = Quaternion.identity;
                else transform.rotation = Quaternion.Euler(0, 0, targetRotationZ);
                isRotationInProgress = false;
            }
        }
    }

    public void RotateTo(PlayerMovement.GravityBase gravityBase, PlayerMovement.GravityBase previous)
    {
        initialRotationZ = transform.rotation.eulerAngles.z;

        if (gravityBase == PlayerMovement.GravityBase.leftWall)
        {
            if (previous == PlayerMovement.GravityBase.floor) initialRotationZ = 360;
            targetRotationZ = 270;
        }
        else if (gravityBase == PlayerMovement.GravityBase.rightWall)
        {
            targetRotationZ = 90;
        }
        else if (gravityBase == PlayerMovement.GravityBase.ceiling)
        {
            targetRotationZ = 180;
        }
        else
        {
            // Rotating to the floor.
            if (previous == PlayerMovement.GravityBase.leftWall) targetRotationZ = 360;
            else targetRotationZ = 0;
        }

        rotationStartTime = Time.time;
        rotationChange = 0;
        isRotationInProgress = true;
    }

    private float timing(float x)
    {
        if (timingFunction == TimingFunction.linear) return linear(x);
        if (timingFunction == TimingFunction.easeInOutSine) return easeInOutSine(x);
        if (timingFunction == TimingFunction.easeInOutCubic) return easeInOutCubic(x);
        if (timingFunction == TimingFunction.easeInOutQuint) return easeInOutQuint(x);
        else return easeInOutCirc(x);
    }

    private float linear(float x)
    {
        if (x > 1) return 1;
        return x;
    }

    private float easeInOutSine(float x)
    {
        if (x > 1) return 1;
        return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
    }

    private float easeInOutCubic(float x)
    {
        if (x > 1) return 1;
        return x < 0.5 ? 4 * Mathf.Pow(x, 3) : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }

    private float easeInOutQuint(float x)
    {
        if (x > 1) return 1;
        return x < 0.5 ? 16 * Mathf.Pow(x, 5) : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    }

    private float easeInOutCirc(float x)
    {
        if (x > 1) return 1;
        return x < 0.5 ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
    }
}