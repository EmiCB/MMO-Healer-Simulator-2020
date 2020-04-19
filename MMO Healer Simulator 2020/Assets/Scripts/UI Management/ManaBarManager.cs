using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBarManager : MonoBehaviour {
    private ManaSystem manaSystem;

    public void Setup(ManaSystem manaSystem) {
        this.manaSystem = manaSystem;

        manaSystem.OnManaChanged += ManaSystem_OnManaChanged;
    }

    private void ManaSystem_OnManaChanged(object sender, System.EventArgs e) {
        // find bar & change length
        transform.Find("Bar").localScale = new Vector3(manaSystem.GetManaPercentage(), 1, 1);
    }
}
