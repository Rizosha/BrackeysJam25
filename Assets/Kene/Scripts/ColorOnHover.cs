using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color hoverColor;
    private Color _defaultColor;
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _defaultColor = _text.color;
        _text.color = _defaultColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.color = _defaultColor;
    }
}
