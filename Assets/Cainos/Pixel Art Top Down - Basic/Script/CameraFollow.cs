using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 1.0f;

        private void LateUpdate()
        {
            if (target == null) return;

            var targetPostion = new Vector3(target.position.x, target.position.y, -10);

            transform.position = Vector3.Lerp(transform.position, targetPostion, lerpSpeed * Time.deltaTime);
        }

    }
}
