using UnityEngine;
using UnityEngine.Events;

/*
 * This class manage the button's animation, the behaviour of each type of button is defined in their own class
 */
public class ButtonAnimation : MonoBehaviour
{
    public bool isPressed = false;
	public bool isPositiveAnimation = false;
	public bool isNegativeAnimation = false;
	public float minScale = 0.5f;
	public float maxScale = 1f;
	public float scalePerUnit = 0.1f;
	public string axesToScale = "y";

	/* 
	 * This is the method for the animation that wants as parameters the max and min scales the button will have during the animation, the scalePerUnit
	 * is how fast the animation is and the axesToScale is nedeed for managing the animation based on the button's transform
	 */
	public void Animation(float minScale, float maxScale, float scalePerUnit, string axesToScale) {
		float axes;
		Vector3 direction;
		switch (axesToScale) {
			case "x":
				axes = transform.localScale.x;
				direction = Vector3.right;
				break;

			case "y":
				axes = transform.localScale.y;
				direction = Vector3.up;
				break;

			case "z":
				axes = transform.localScale.z;
				direction = Vector3.forward;
				break;

			default:
				axes = transform.localScale.y;
				direction = Vector3.up;
				break;
		}

		if (isPositiveAnimation) {
			if (axes > minScale) {
				transform.localScale -= direction * scalePerUnit;
			}
			else {
				isPositiveAnimation = false;
				isNegativeAnimation = true;
			}
		}
		else if (isNegativeAnimation) {
			if (axes < maxScale) {
				transform.localScale += direction * scalePerUnit;
			}
			else {
				isNegativeAnimation = false;
			}
		}
	}
}
