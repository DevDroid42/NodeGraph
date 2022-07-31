using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;
using UnityEngine.Events;

public class AutoCompExtended : AutoCompleteComboBox
{
    public bool pauseUpdate;
    public List<string> _prunedPanelItems;
    public List<string> _panelItems;
    public Color ValidSelectionColor;
    public Color matchingItemsRemainingTextColor;
    public UnityEvent OnValChangedEvent = new UnityEvent();
}
