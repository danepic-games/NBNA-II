using TMPro;
using UnityEngine;

namespace Components.Scripts {
    public class DisapearInTimeUnitScript : MonoBehaviour {
        public float intervalToDisapear;
        public TextMeshPro textMeshPro;

        public float initialInterval;

        void Start(){
            initialInterval = intervalToDisapear;
        }

        // Update is called once per frame
        void Update() {
            if (intervalToDisapear >= 0f) {
                intervalToDisapear -= Time.fixedDeltaTime;

                var newAlpha = (1.000f * intervalToDisapear) / initialInterval;
                Color topLeft = new Color(textMeshPro.colorGradient.topLeft.r,
                        textMeshPro.colorGradient.topLeft.g, textMeshPro.colorGradient.topLeft.b, newAlpha);

                Color topRight = new Color(textMeshPro.colorGradient.topRight.r,
                        textMeshPro.colorGradient.topRight.g, textMeshPro.colorGradient.topRight.b, newAlpha);

                Color bottomLeft = new Color(textMeshPro.colorGradient.bottomLeft.r,
                        textMeshPro.colorGradient.bottomLeft.g, textMeshPro.colorGradient.bottomLeft.b, newAlpha);

                Color bottomRight = new Color(textMeshPro.colorGradient.bottomRight.r,
                        textMeshPro.colorGradient.bottomRight.g, textMeshPro.colorGradient.bottomRight.b, newAlpha);

                textMeshPro.colorGradient = new VertexGradient(topLeft, topRight, bottomLeft, bottomRight);
                textMeshPro.colorGradientPreset = new TMP_ColorGradient(topLeft, topRight, bottomLeft, bottomRight);
            }

            if(intervalToDisapear < 0){
                Destroy(gameObject);
            }
        }
    }
}