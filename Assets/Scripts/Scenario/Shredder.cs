using UnityEngine;

namespace Scenario
{
    public class Shredder : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.SetActive(false);
        }
    }
}
