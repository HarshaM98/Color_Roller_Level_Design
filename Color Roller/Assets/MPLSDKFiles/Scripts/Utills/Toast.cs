using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MPL.utility.toastMessage
{


    public class Toast : MonoBehaviour
    {
        public RectTransform backgroundTransform;
        public RectTransform messageTransform;

        public static Toast instance;
        [HideInInspector]
        public bool isShowing = false;

        private Queue<AToast> queue = new Queue<AToast>();

        private class AToast
        {
            public string msg;
            public float time;
            public AToast(string msg, float time)
            {
                this.msg = msg;
                this.time = time;
            }
        }

        private void Awake()
        {
            instance = this;
            SetEnabled(false);
        }

        public void SetMessage(string msg)
        {
            messageTransform.GetComponent<Text>().text = msg;
            StartCoroutine(EnlargeBG(0.1f, () =>
            {
                //backgroundTransform.sizeDelta = new Vector2(messageTransform.GetComponent<Text>().preferredWidth + 30, messageTransform.GetComponent<Text>().preferredHeight + 30);
            }));
        }
        private static IEnumerator EnlargeBG(float delay,Action OnDelayDone)
        {
            yield return new WaitForSeconds(delay);
            OnDelayDone.Invoke();
        }
        private void Show(AToast aToast)
        {
            SetMessage(aToast.msg);
            SetEnabled(true);
            GetComponent<Animator>().SetBool("show", true);
            Invoke("Hide", aToast.time);
            isShowing = true;
        }

        public void ShowMessage(string msg, float time = 1.5f)
        {
            Debug.Log("sahmil -- > show toast message");
            AToast aToast = new AToast(msg, time);
            queue.Enqueue(aToast);

            ShowOldestToast();
        }

        private void Hide()
        {
            GetComponent<Animator>().SetBool("show", false);
            Invoke("CompleteHiding", 1);
        }

        private void CompleteHiding()
        {
            SetEnabled(false);
            isShowing = false;
            ShowOldestToast();
        }

        private void ShowOldestToast()
        {
            if (queue.Count == 0) return;
            if (isShowing) return;

            AToast current = queue.Dequeue();
            Show(current);
        }

        private void SetEnabled(bool enabled)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(enabled);
            }
        }

        [ContextMenu("debug show message")]
        public void DebugShowMessage()
        {
            ShowMessage("Your opponent's network seems unstable. We are sorry for the trouble. Please try again.", 3);
        }
    }
}

