using UnityEngine;

public class SetWarningDisappear : MonoBehaviour
{
        public void DisappearWarning()
        {
                GameObject turretWarning = GameObject.FindGameObjectWithTag("TurretWarning");
                turretWarning.GetComponent<Animator>().SetBool("IsOpen", false);
        }
}