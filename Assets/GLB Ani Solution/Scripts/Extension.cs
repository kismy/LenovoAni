using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTween
{
    public static class Extension
    {
        public static MonoBehaviour mono;
        private static Dictionary<string, Coroutine> coroutineDic = new Dictionary<string, Coroutine>();
        public static void DoScale(this Transform target, Vector3 scale, float speed = 10)
        {
            mono.StartCoroutine(DOSCALE(target, scale, speed));
        }
        public static IEnumerator DOSCALE(Transform target, Vector3 scale, float speed = 10)
        {
            Vector3 delta = (scale - target.localScale) * Time.fixedDeltaTime * speed;
            int count = (int)(1f / (Time.fixedDeltaTime * speed));

            WaitForFixedUpdate wait= new WaitForFixedUpdate();
            while (count > 0)
            {
                count--;
                target.localScale += delta;
                yield return wait;
            }
        }

        public static void DoLocalPosition(this Transform target, Vector3 localPosition, float speed = 1)
        {
            string Key = string.Format("T:{0}", target.name);
            if (coroutineDic.ContainsKey(Key))
            {
                Coroutine coroutine = coroutineDic[Key];
                if (coroutine != null)
                    mono.StopCoroutine(coroutine);
                coroutineDic[Key] = mono.StartCoroutine(DOLOCALPOSITION(target, localPosition, speed));
            }
            else
                coroutineDic.Add(Key, mono.StartCoroutine(DOLOCALPOSITION(target, localPosition, speed)));
        }
        public static IEnumerator DOLOCALPOSITION(Transform target, Vector3 localPosition, float speed = 1)
        {
            WaitForFixedUpdate wait = new WaitForFixedUpdate();

            while (
                 Mathf.Abs(target.localPosition.x - localPosition.x) > 0.001f ||
                 Mathf.Abs(target.localPosition.y - localPosition.y) > 0.001f ||
                 Mathf.Abs(target.localPosition.z - localPosition.z) > 0.001f)
            {
                target.localPosition = Vector3.Lerp(target.localPosition,localPosition, Time.fixedDeltaTime * speed);
                yield return wait;
            }
            target.localPosition = localPosition;

        }

        public static void DoLocalRotation(this Transform target, Vector3 localRotation, float speed = 1)
        {
            string Key = string.Format("R:{0}", target.name);
            if (coroutineDic.ContainsKey(Key))
            { 
                Coroutine coroutine = coroutineDic[Key];
                if(coroutine!=null)
                    mono.StopCoroutine(coroutine);
                coroutineDic[Key] = mono.StartCoroutine(DOLOCALROTATION(target, localRotation, speed));
            }else
                coroutineDic.Add(Key, mono.StartCoroutine(DOLOCALROTATION(target, localRotation, speed)));
        }
        public static IEnumerator DOLOCALROTATION(Transform target, Vector3 localRotation, float speed = 1)
        {
            WaitForFixedUpdate wait = new WaitForFixedUpdate();

            Quaternion quat = Quaternion.Euler(localRotation);
            while (
                Mathf.Abs(target.localRotation.eulerAngles.x - localRotation.x) > 0.01f||
                Mathf.Abs(target.localRotation.eulerAngles.y - localRotation.y) > 0.01f||
                Mathf.Abs(target.localRotation.eulerAngles.z - localRotation.z) > 0.01f)
            {
                //count--;
                //target.localRotation = Quaternion.Euler(target.localRotation.eulerAngles+ delta);
                target.localRotation = Quaternion.Lerp(target.localRotation, quat, Time.fixedDeltaTime*speed);

                yield return wait;
            }
            target.localRotation = quat;
        }

    }
}


