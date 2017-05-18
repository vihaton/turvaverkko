using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ClickableInterface {

    bool isOnlyHeld();

    void Clicked();

    void Held();
}
