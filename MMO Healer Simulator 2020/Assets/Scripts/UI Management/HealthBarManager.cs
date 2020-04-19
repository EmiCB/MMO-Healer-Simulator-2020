using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour {
    private HealthSystem healthSystem;

    public void Setup(HealthSystem healthSystem) {
        this.healthSystem = healthSystem;

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e) {
        // find bar & change length
        transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercentage(), 1, 1);
    }
}
