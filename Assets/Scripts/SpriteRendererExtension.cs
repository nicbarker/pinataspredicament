using System.Collections;
using UnityEngine;

static class SpriteRendererExtension
{
    public static IEnumerator FlashTimes(this SpriteRenderer spriteRenderer, int n)
    {
        for (var i = 0; i < n; i++)
        {
            yield return spriteRenderer.Flash();
        }
        spriteRenderer.color = WithAlpha(spriteRenderer.color, 1.0f);
    }

    public static IEnumerator FlashForever(this SpriteRenderer spriteRenderer)
    {
        while (true)
        {
            yield return spriteRenderer.Flash();
        }
    }

    private static IEnumerator Flash(this SpriteRenderer spriteRenderer)
    {
        spriteRenderer.color = WithAlpha(spriteRenderer.color, 1.0f);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = WithAlpha(spriteRenderer.color, 0.5f);
        yield return new WaitForSeconds(0.1f);
    }

    private static Color WithAlpha(Color color, float alpha)
    {
        return new Color(r: color.r, g: color.g, b: color.b, a: alpha);
    }
}
