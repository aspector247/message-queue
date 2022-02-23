using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace com.spector.Extensions
{
    public enum ImageAcquisitionResult
    {
        Success, Failure
    }

    public static class ImageExtensions
    {
        public static void SetImageFromUrlAsync(
            this Image image,
            string url,
            Action<ImageAcquisitionResult> resultCallback = null,
            Action<Texture2D> textureCallback = null
        )
        {
            image.StartCoroutine(LoadFromWeb(image, url, SuccessCallback, FailureCallback));
            
            void SuccessCallback(Sprite sprite)
            {
                if (image != null)
                {
                    image.sprite = sprite;
                    image.preserveAspect = true;
                    resultCallback?.Invoke(ImageAcquisitionResult.Success);
                }
                else
                {
                    resultCallback?.Invoke(ImageAcquisitionResult.Failure);
                }
            }

            void FailureCallback(string s)
            {
                Debug.LogWarning($"Error downloading sprite:{url}, error:{s}");
                resultCallback?.Invoke(ImageAcquisitionResult.Failure);
            }
        }

        static IEnumerator LoadFromWeb(Image image, string url, Action<Sprite> onComplete, Action<string> onError)
        {
            UnityWebRequest wr = new UnityWebRequest(url);
            DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
            wr.downloadHandler = texDl;
            yield return wr.SendWebRequest();
            if (wr.result == UnityWebRequest.Result.Success) {
                Texture2D t = texDl.texture;
                Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height),
                    Vector2.zero, 1f);
                onComplete?.Invoke(s);
            }
            onError?.Invoke(wr.error);
        }

        public static void SetTexture(this Image image, Texture2D texture2d)
        {
            image.sprite = CreateNewSprite(texture2d);
        }

        public static Sprite CreateNewSprite(Texture2D texture2d)
        {
            return Sprite.Create(
                texture2d,
                new Rect(0, 0, texture2d.width, texture2d.height),
                Vector2.zero,
                100,
                0,
                SpriteMeshType.FullRect
            );
        }

        public static void EnvelopeParent(this Image image)
        {
            var imageAspect = image.sprite.rect.width / image.sprite.rect.height;
            var aspectFitter = image.GetComponent<AspectRatioFitter>();
            if (aspectFitter != null)
            {
                aspectFitter.aspectRatio = imageAspect;
                image.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
            }
        }
    }
}