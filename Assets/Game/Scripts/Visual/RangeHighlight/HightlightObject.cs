using UnityEngine;

public class HightlightObject : MonoBehaviour, IPoolObject<HightlightObject>
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Disable()
    {
        spriteRenderer.enabled = false;
    }

    public HightlightObject Enable()
    {
        spriteRenderer.enabled = true;
        return this;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
}
