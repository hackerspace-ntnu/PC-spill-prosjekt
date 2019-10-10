using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite playerRegular;
    public Sprite playerGhost;
    void Start()
    {
        // player sprites
        playerRegular = Resources.Load<Sprite>("PlayerSprites/SpriteRegular");
        playerGhost = Resources.Load<Sprite>("PlayerSprites/SpriteShadow");
    }

}
