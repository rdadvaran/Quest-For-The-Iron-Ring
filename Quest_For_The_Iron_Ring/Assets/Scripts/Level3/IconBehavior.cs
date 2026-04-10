using UnityEngine;

public class IconBehavior : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;

    private Task3 levelManager;

    private void Start()
    {
        levelManager = FindObjectOfType<Task3>();
        Destroy(gameObject, lifeTime);
    }

    public void Collect()
    {
        if (levelManager != null)
        {
            levelManager.AddIcon();
        }

        Destroy(gameObject);
    }
}