using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cuppsats
{
    public class AiHead : MonoBehaviour
    {

        public void LookAtPlayer(Vector3 direction)
        {
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }
    }
}
