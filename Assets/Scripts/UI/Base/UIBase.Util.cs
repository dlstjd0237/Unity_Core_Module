using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public partial class UIBase
{
    public void SetImage(Image InImage, Sprite InSprite)
    {
        InImage.sprite = InSprite;
    }

    public void SetImage(Image InImage, Texture2D InTexture)
    {
        Sprite sprite = Sprite.Create(InTexture, new Rect(0, 0, InTexture.width, InTexture.height), new Vector2(0.5f, 0.5f));
        InImage.sprite = sprite;
    }

    public void SetSlider(Slider InSlider, float InValue)
    {
        InSlider.value = InValue;
    }
    
    public void BindEvent(Slider InSlider, UnityAction<float> InAction)
    {
        InSlider.onValueChanged.AddListener(InAction);
    }

    public void UnbindEvent(Slider InSlider, UnityAction<float> InAction)
    {
        InSlider.onValueChanged.AddListener(InAction);
    }

    public void BindEvent(Button InButton, UnityAction InAction)
    {
        InButton.onClick.AddListener(InAction);
    }
    
    public void UnbindEvent(Button InButton, UnityAction InAction)
    {
        InButton.onClick.RemoveListener(InAction);
    }
    
    public void AllUnbindEvent(Button InButton)
    {
        InButton.onClick.RemoveAllListeners();
    }

    public void BindEvent(Toggle InToggle, UnityAction<bool> InAction)
    {
        InToggle.onValueChanged.AddListener(InAction);
    }

    public void UnbindEvent(Toggle InToggle, UnityAction<bool> InAction)
    {
        InToggle.onValueChanged.RemoveListener(InAction);
    }

    public void AllUnbindEvent(Toggle InToggle)
    {
        InToggle.onValueChanged.RemoveAllListeners();
    }

    public void SetToggle(Toggle InToggle, bool InValue)
    {
        InToggle.isOn = InValue;
    }

    public void SetText(TextMeshProUGUI InText, string InDescription)
    {
        InText.SetText(InDescription);
    }

    public void SetText(TMP_Text InText, string InDescription)
    {
        InText.SetText(InDescription);
    }

    public void SetTextFormat(TMP_Text InText, string InFormat, params object[] InArgs)
    {
        InText.SetText(string.Format(InFormat, InArgs));
    }

    public void SetActive(GameObject InGameObject, bool InActive)
    {
        InGameObject.SetActive(InActive);
    }
}