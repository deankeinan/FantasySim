using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor {
    void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.viewRadius);

        if (!fow.facingRight)
        {
            Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2 + 180, false); //Flip +180
            Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2 - 180, false); //Flip -180
            Vector3 lineA = fow.transform.position + viewAngleA * fow.viewRadius;
            Vector3 lineB = fow.transform.position + viewAngleB * fow.viewRadius;
            Handles.DrawLine(fow.transform.position, lineA);
            Handles.DrawLine(fow.transform.position, lineB);

            Handles.color = Color.red;
            foreach (Transform visibleTarget in fow.visibleTargets)
            {
                Handles.DrawLine(fow.transform.position, visibleTarget.position);
            }
        }
        if (fow.facingRight)
        {
            Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false); //Flip +180
            Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false); //Flip -180
            Vector3 lineA = fow.transform.position + viewAngleA * fow.viewRadius;
            Vector3 lineB = fow.transform.position + viewAngleB * fow.viewRadius;
            Handles.DrawLine(fow.transform.position, lineA);
            Handles.DrawLine(fow.transform.position, lineB);

            Handles.color = Color.red;
            foreach (Transform visibleTarget in fow.visibleTargets)
            {
                Handles.DrawLine(fow.transform.position, visibleTarget.position);
            }
        }
    }
}
