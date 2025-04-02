using UnityEngine;

public interface IDamagable 
{

    void Damage(int damageAmount);

    void Die();

    int maxHealth {  get; set; }
    int currentHealth {  get; set; }

    
}
