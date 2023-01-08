namespace KaanDonmez.ComponentInheritances
{
    using System.Collections;
    using TMPro;
    using UnityEngine;

    public class InfoText : TextMeshProUGUI
    {
        private const float FadeDuration = 1f;
        private const float ScreenTime = 2f;
        private const float AnimationFrameRate = 60f;

        public override string text
        {
            get => base.text;
            set
            {
                base.text = value;
                StartCoroutine(Fade(FadeType.In));
            }
        }
        
        private enum FadeType
        {
            In,
            Out
        }

        private IEnumerator Fade(FadeType type)
        {
            if (type == FadeType.In)
            {
                Color temp = Color.white;
                float step = (1f / AnimationFrameRate) / (FadeDuration / 2f);
                WaitForSeconds delay = new WaitForSeconds(1f / AnimationFrameRate);

                while (color.a < 1)
                {
                    temp = color;
                    temp.a += step;
                    color = temp;
                    yield return delay;
                }
                yield return new WaitForSeconds(ScreenTime);
                yield return Fade(FadeType.Out);
            }
            else
            {
                Color temp = Color.white;
                float step = (1f / AnimationFrameRate) / (FadeDuration); // 60 FPS
                WaitForSeconds delay = new WaitForSeconds(1f / AnimationFrameRate);

                while (color.a > 0)
                {
                    temp = color;
                    temp.a -= step;
                    color = temp;
                    yield return delay;
                }
            }
        }
    }
}