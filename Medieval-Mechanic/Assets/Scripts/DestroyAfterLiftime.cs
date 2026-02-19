using UnityEngine;

public class DestroyAfterLiftime : MonoBehaviour
{
    [SerializeField] private float liftime = 1.0f;
    void Start()
    {
        Destroy(gameObject, liftime);
    }

}
