using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSwitchSprite : MonoBehaviour
{
	[SerializeField] SpriteRenderer spriteOne = null;
	[SerializeField] SpriteRenderer spriteTwo = null;
	[SerializeField] float fadeSpeed = 1f;
	[SerializeField] float minAlpha = 0f;
	[SerializeField] float maxAlpha = 1f;
	int currentSprite;
	int previousSprite;

	void Update()
    {
		float alpha = Mathf.Sin((Time.time) * fadeSpeed);
		Color tmp = spriteOne.color;
		tmp.a = MathHelper.ScaleValue(alpha, -1, 1, minAlpha, maxAlpha);
		spriteOne.color = tmp;

		alpha = maxAlpha - tmp.a;
		tmp = spriteTwo.color;
		tmp.a = alpha;
		spriteTwo.color = tmp;
	}
}
