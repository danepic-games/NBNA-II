using Model;
using UnityEngine;
using Components.Managers;
using Components.Handlers;
using Components.Controllers;
using Models;

namespace Components.Controllers {
    public class ShadowController : MonoBehaviour {
        public GameObject parent;
        private SpriteRenderer spriteRenderer;
        private SpriteRenderer parentSpriteRenderer;
        private Rigidbody rigidbody;
        public float maxY;

        private Background background;

        // Use this for initialization
        void Start() {
            parentSpriteRenderer = parent.GetComponent<SpriteRenderer>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rigidbody = GetComponent<Rigidbody>();

            if (background == null) {
                background = GameObject.FindGameObjectWithTag("BG").GetComponent<Background>();
            }

            spriteRenderer.sprite = null;
        }

        // Update is called once per frameq
        void Update() {
            spriteRenderer.sprite = parentSpriteRenderer.sprite;
            spriteRenderer.flipX = parentSpriteRenderer.flipX;

            var parentX = parent.transform.position.x;
            var parentZ = parent.transform.position.z;

            var thisTransform = transform.position;
            thisTransform.x = parentX;
            thisTransform.z = parentZ;
            transform.position = thisTransform;
        }

        void LateUpdate() {
            if (transform.position.y > maxY) {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, -5f, rigidbody.velocity.z);

            } else {
                transform.position = new Vector3(transform.position.x, background.resetY, transform.position.z);
            }
        }
    }
}