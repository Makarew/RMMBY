using UnityEngine;

namespace RMMBY
{
    internal class ModToggleButton : MonoBehaviour
    {
        private ModMenuHandler sys;

        private bool isHighlighted;
        private GameObject highlight;

        private void Start()
        {
            sys = GameObject.FindObjectOfType<ModMenuHandler>();
            highlight = transform.Find("Highlight").gameObject;
        }

        private void Update()
        {
            if (!isHighlighted && sys.selectedObject == gameObject)
            {
                isHighlighted = true;
                highlight.SetActive(true);
            }
            else if (isHighlighted && sys.selectedObject != gameObject)
            {
                highlight.SetActive(false);
                isHighlighted = false;
            }
        }
    }
}