using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
	//Scale a value within a given range into a new range
    public static float ScaleValue(float value, float currentMin, float currentMax, float newMin, float newMax)
	{
		//Remove the offset from the current min value and replace it with the new min
		//Multply by the scale fraction of the current and new ranges
		return newMin + (value - currentMin) * (newMax - newMin) / (currentMax - currentMin);
	}

	//Snap a value to the positive value, negative value, or to 0
	public static float SnapValue(float value, float snapValue)
	{
		if (value > snapValue * 0.5f)
			return snapValue;
		else if (value < -snapValue * 0.5f)
			return -snapValue;
		return 0;
	}
}
