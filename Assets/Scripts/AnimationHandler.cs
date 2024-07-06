using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public string path;
    public bool killAfterComplete;
    public bool randomFirstFrame;
    public float[] randomRotation;
    private int numFrames;
    private SpriteRenderer sr;
    private bool complete = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Sprite[] sprites = Resources.LoadAll<Sprite>(path); // grabs all packed frames
        numFrames = sprites.Length;
        if (randomFirstFrame)
        {
            sr.sprite = sprites[Random.Range(0, sprites.Length-1)]; // sets to a random one
        }
        if (randomRotation.Length > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, randomRotation[Random.Range(0, randomRotation.Length)]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        string name = sr.sprite.name;
        if (killAfterComplete)
        {
            if (name.Substring(name.Length - 1).Equals(numFrames - 1 + ""))
            {
                complete = true;
            }
            else if (complete)
            {
                Destroy(gameObject);
            }
        }
    }
}
