using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class NameTextAnimationScript : MonoBehaviour
{
    public float fadeTime = 1.0f;
    public float fadeBaseValue = 0f;

    private TextMesh nameText;

    private void Start()
    {
        nameText = GetComponent<TextMesh>();
        nameText.color = new Color(nameText.color.r, nameText.color.g, nameText.color.b, 0);
    }

    internal void ChangeText(bool opening, string text)
    {
        if (text != null)
            nameText.text = text;

        Debug.Log("Fade called, opening: " + opening);
        if (opening)
        {
            Fade(true);
        } else
        {
            Fade(false);
        }

    }

    public void Fade(bool fadingIn)
    {
        StopAllCoroutines();
        if (fadingIn)
        {
            StartCoroutine(FadeTo(255, fadeTime));
        }
        else
        {
            StartCoroutine(FadeTo(0, fadeTime));
        }
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        Color nameColor = nameText.color;
        float alpha = nameText.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            float a = Mathf.Lerp(alpha, aValue, t);
            Color newColor = new Color(nameColor.r, nameColor.g, nameColor.b, a);
            nameText.color = newColor;
            yield return null;
        }
    }
}