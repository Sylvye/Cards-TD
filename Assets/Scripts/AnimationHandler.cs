using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public bool killAfterComplete;
    public bool randomFirstFrame;
    private int numFrames;
    private SpriteRenderer sr;
    private bool complete = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Assets/Images/EnemyAnimation.png"); // grabs all packed frames
        numFrames = sprites.Length;
        if (randomFirstFrame)
        {
            sr.sprite = sprites[Random.Range(0, sprites.Length-1)]; // sets to a random one
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
