using UnityEngine;

public class Arrow : Projectile
{
    protected override void DoAttack()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * (speed * Time.deltaTime);
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // Close Target
        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            TakeDamage();
        }
    }
}
