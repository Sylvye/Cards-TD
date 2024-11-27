using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    public float[] randomRotation;

    // Start is called before the first frame update
    private void Start()
    {
        if (randomRotation.Length > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, randomRotation[Random.Range(0, randomRotation.Length)]);
        }
    }
}
