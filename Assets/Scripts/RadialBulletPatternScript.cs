using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialBulletPatternScript : MonoBehaviour
{
    public int numberOfProjectiles;             // Number of projectiles to shoot.
    public float radialAngle = 360f;
    public float angleOffset = 0f;
    public float projectileSpeed;               // Speed of the projectile.
    public GameObject ProjectilePrefab;         // Prefab to spawn.

    [Header("Private Variables")]
    private Vector3 startPoint;                 // Starting position of the bullet.
    private const float radius = 1F;            // Help us find the move direction.


    void Awake(){
        if(radialAngle > 360){
            radialAngle = 360;
        }
        startPoint = transform.position;
        SpawnProjectile(numberOfProjectiles);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawns x number of projectiles.
    private void SpawnProjectile(int _numberOfProjectiles)
    {
        int projectileAngleDivider = _numberOfProjectiles - 1;
        if(radialAngle >= 360){
            projectileAngleDivider = _numberOfProjectiles;
        }
        float angleStep = radialAngle / projectileAngleDivider;
        float angle = -(radialAngle / 2) + transform.eulerAngles.y + angleOffset;
        float initialAngle = angle;

        for (int i = 0; i <= _numberOfProjectiles - 1; i++)
        {
            
            // Direction calculations.
            float projectileDirXPosition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYPosition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            // Create vectors.
            Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
            Vector3 projectileMoveDirection = (projectileVector - startPoint).normalized * projectileSpeed;

            // Create game objects.
            GameObject tmpObj = Instantiate(ProjectilePrefab, startPoint, Quaternion.identity);
            tmpObj.GetComponent<Rigidbody>().velocity = new Vector3(projectileMoveDirection.x, 0, projectileMoveDirection.y);

            // Destory the gameobject after 10 seconds.
            Destroy(tmpObj, 10F);

            angle += angleStep;
        }
    }
}
