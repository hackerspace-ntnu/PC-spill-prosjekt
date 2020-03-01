using GlobalEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookChain : MonoBehaviour
{
    public HookHead hookHead;

    private PlayerController playerController;
    private SpriteRenderer hookHeadRenderer;

    private Material mat;
    private Vector3 spriteSize;
    private float textureAspectRatio;

    void Start()
    {
        playerController = hookHead.playerController;
        hookHeadRenderer = hookHead.GetComponent<SpriteRenderer>();

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        mat = spriteRenderer.material;

        Sprite sprite = spriteRenderer.sprite;
        spriteSize = sprite.bounds.size;
        textureAspectRatio = (float) sprite.texture.width / sprite.texture.height;

        // Update appearance before first frame update to prevent default appearance in first frame
        Update();
    }

    void Update()
    {
        UpdateStretch();

        // Update material's Y-tiling to make it look like the chain's texture is coming out of the player
        Vector2 spriteWorldSize = SpriteUtils.GetWorldSize(spriteSize, transform);
        float spriteTargetWorldLength = spriteWorldSize.x / textureAspectRatio;
        float yTiling = spriteWorldSize.y / spriteTargetWorldLength;
        mat.SetTextureScale("_MainTex", new Vector2(1f, yTiling));
    }

    /// <summary>
    ///   <para>
    ///     Updates the transform's scale, rotation and position to make it look like the chain is coming out of the player.
    ///   </para>
    /// </summary>
    private void UpdateStretch()
    {
        Vector3 hookPos = SpriteUtils.GetSidePos(hookHeadRenderer, SquareSide.TOP);

        Vector3 playerPos = playerController.transform.position;
        Vector3 centerPos = (playerPos + hookPos) / 2f;
        transform.position = centerPos;

        Vector3 hookDirection = VectorUtils.GetDirectionToVector(playerPos, hookPos);
        transform.up = hookDirection;

        Vector2 localHookDistance = transform.parent.InverseTransformVector(hookPos - playerPos);
        float targetLength = localHookDistance.magnitude;
        float targetScaleY = targetLength / spriteSize.y;
        transform.localScale = new Vector3(transform.localScale.x, targetScaleY, 1f);
    }
}
