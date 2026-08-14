using System.Collections;
using UnityEngine;

namespace CubicChaos.Grid
{
    public class BlockCellEffect : MonoBehaviour
    {
        [SerializeField] private float placeDuration = 0.12f;
        [SerializeField] private float placeOvershoot = 1.25f;
        [SerializeField] private float clearDuration = 0.22f;
        [SerializeField] private float clearBurstScale = 1.2f;

        private Vector3 baseScale;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            baseScale = transform.localScale;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(PlacePopRoutine());
        }

        private IEnumerator PlacePopRoutine()
        {
            transform.localScale = Vector3.zero;
            float elapsed = 0f;

            while (elapsed < placeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / placeDuration;
                float scaleFactor = EaseOutBack(t) * placeOvershoot;
                transform.localScale = baseScale * scaleFactor;
                yield return null;
            }

            transform.localScale = baseScale;
        }

        public void PlayClearAndDestroy(float delay = 0f)
        {
            StopAllCoroutines();
            StartCoroutine(ClearRoutine(delay));
        }

        private IEnumerator ClearRoutine(float delay)
        {
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            Vector3 startScale = transform.localScale;
            Color startColor = spriteRenderer != null ? spriteRenderer.color : Color.white;

            // Kısa bir "genişleme" ile patlama hissi verir, sonra eriyerek küçülür.
            float burstDuration = clearDuration * 0.25f;
            float elapsed = 0f;

            while (elapsed < burstDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / burstDuration;
                transform.localScale = Vector3.Lerp(startScale, startScale * clearBurstScale, EaseOutQuad(t));
                yield return null;
            }

            Vector3 burstScale = transform.localScale;
            float shrinkDuration = clearDuration - burstDuration;
            elapsed = 0f;

            while (elapsed < shrinkDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / shrinkDuration;
                float eased = EaseInCubic(t);
                transform.localScale = Vector3.Lerp(burstScale, Vector3.zero, eased);

                if (spriteRenderer != null)
                {
                    Color color = startColor;
                    color.a = Mathf.Lerp(startColor.a, 0f, eased);
                    spriteRenderer.color = color;
                }

                yield return null;
            }

            Destroy(gameObject);
        }

        private static float EaseOutBack(float t)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;
            float x = t - 1f;
            return 1f + c3 * x * x * x + c1 * x * x;
        }

        private static float EaseOutQuad(float t)
        {
            return 1f - (1f - t) * (1f - t);
        }

        private static float EaseInCubic(float t)
        {
            return t * t * t;
        }
    }
}
