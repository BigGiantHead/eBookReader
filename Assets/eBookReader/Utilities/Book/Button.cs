using UnityEngine;
using System.Collections;

namespace BookData
{
    [System.Serializable]
    public class Button
    {
        public string reference;
        public float width;
        public float height;
        public float posX;
        public float posY;
        public float rotation;
        public string actionName;
        public string actionParameter;

        public Button()
        {
            reference = null;
            width = 0;
            height = 0;
            posX = 0;
            posY = 0;
            rotation = 0;
            actionName = null;
            actionParameter = null;
        }

        public string Title
        {
            get
            {
                return Localization.Instance.GetEntry(reference);
            }
        }
    }
}
