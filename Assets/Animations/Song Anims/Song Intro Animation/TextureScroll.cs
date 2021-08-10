using UnityEngine;

namespace Animations.Song_Anims.Song_Intro_Animation {
    public class TextureScroll : MonoBehaviour
    {
        public GameObject quadGameObject;
        private Renderer quadRenderer;

        public float scrollSpeed = 0.5f;

        void Start()
        {
            quadRenderer = quadGameObject.GetComponent<Renderer>();
        }

        void Update()
        {
            Vector2 textureOffset = new Vector2(Time.time*scrollSpeed,0);
            quadRenderer.material.mainTextureOffset = textureOffset;
        }
    }
}
