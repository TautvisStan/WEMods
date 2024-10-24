using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class Utils
    {
        public static Text SetupUIText(ref GameObject UIObject, string Name)
        {
            if (UIObject == null)
            {
                UIObject = new GameObject(Name);
                UIObject.transform.SetParent(LIPNHOMGGHF.JPABICKOAEO.transform, false);
                UIObject.AddComponent<Text>().font = CollectibleCards2.CardMenu.VanillaFont;
                UIObject.AddComponent<Outline>().effectColor = new Color(0, 0, 0, 1);
                UIObject.GetComponent<Outline>().effectDistance = new Vector2(1, 1);
                UIObject.AddComponent<Shadow>().effectDistance = new Vector2(3, -3);

            }
            Text text = UIObject.GetComponent<Text>();
            text.text = "";
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 30;
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 0);
            rectTransform.anchoredPosition = new Vector2(0, 0);
            return text;
        }
    }
}
