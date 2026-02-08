using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
  public UnityEngine.UI.Toggle toggle;
  public GameObject knob;
  public TextMeshProUGUI onText;
  public TextMeshProUGUI offText;
  public Sprite ActiveKnob;
  public Sprite InActiveKnob;
  public GameObject KnobMask;


  public Vector2 knobOnPos;
  public Vector2 knobOffPos;
  public float animTime = 0.15f;

  Coroutine anim;

  void Awake()
  {
    UnityEngine.UI.Toggle toggle = GetComponent<UnityEngine.UI.Toggle>();
    toggle.onValueChanged.AddListener(SetState);
    SetState(toggle.isOn);
  }

  void SetState(bool isOn)
  {
    if (anim != null)
      StopCoroutine(anim);

    var knobImage = knob.GetComponent<Image>();
    knobImage.sprite = isOn ? ActiveKnob : InActiveKnob;
    anim = StartCoroutine(AnimateToggle(isOn));
  }

  IEnumerator AnimateToggle(bool isOn)
  {
    var knobTransform = knob.GetComponent<RectTransform>();
    Vector2 start = knobTransform.anchoredPosition;
    Vector2 target = isOn ? knobOnPos : knobOffPos;

    float onTargetAlpha = isOn ? 1f : 0f;
    float offTargetAlpha = isOn ? 0f : 1f;

    float t = 0f;
    while (t < animTime)
    {
      t += Time.deltaTime;
      float lerp = t / animTime;

      knobTransform.anchoredPosition = Vector2.Lerp(start, target, lerp);

      onText.alpha = Mathf.Lerp(1, onTargetAlpha, t);
      offText.alpha = Mathf.Lerp(1, offTargetAlpha, t);

      yield return null;
    }

    knobTransform.anchoredPosition = target;
    onText.alpha = onTargetAlpha;
    offText.alpha = offTargetAlpha;
  }
}
