﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Todo: Move to different scripts by menu names

public static partial class Actions {

    public static void ShowActionsPanel()
    {
        ShowActionsButtons(Selection.ThisShip.GetAvailableActionsList());
    }

    public static void ShowFreeActionsPanel()
    {
        Phases.StartFreeActionSubPhase("Free action");
        ShowActionsButtons(Selection.ThisShip.GetAvailableFreeActionsList());
    }

    private static void ShowActionsButtons(List<ActionsList.GenericAction> actionList)
    {
        HideActionsButtons();

        float offset = 0;
        Vector3 defaultPosition = new Vector3(5, -5, 0);
        foreach (var action in actionList)
        {
            GameObject newButton = MonoBehaviour.Instantiate(Game.PrefabsList.GenericButton, Game.PrefabsList.PanelActions.transform.Find("ActionButtons").transform);
            newButton.name = "Button" + action.Name;
            newButton.transform.GetComponentInChildren<Text>().text = action.Name;
            newButton.GetComponent<RectTransform>().localPosition = defaultPosition + new Vector3(0, -offset, 0);
            offset += 40;
            newButton.GetComponent<Button>().onClick.AddListener(delegate {
                action.ActionTake();
                Selection.ThisShip.AddAlreadyExecutedAction(action);
                Tooltips.EndTooltip();
                CloseActionsPanel();
            });
            Tooltips.AddTooltip(newButton, action.ImageUrl);
            newButton.GetComponent<Button>().interactable = true;
            newButton.SetActive(true);
        }

        if (actionList.Count != 0)
        {
            Game.PrefabsList.PanelActions.GetComponent<RectTransform>().sizeDelta = new Vector2(Game.PrefabsList.PanelActions.GetComponent<RectTransform>().sizeDelta.x, offset + 80);
            Game.PrefabsList.PanelActions.SetActive(true);
        }
        else
        {
            Game.UI.ShowError("Cannot perform any actions");
            Phases.CurrentSubPhase.NextSubPhase();
        }
    }

    public static void HideActionsButtons()
    {
        foreach (Transform button in Game.PrefabsList.PanelActions.transform.Find("ActionButtons").transform)
        {
            if (button.name.StartsWith("Button"))
            {
                MonoBehaviour.Destroy(button.gameObject);
            }
        }
    }

    public static void CloseActionsPanel()
    {
        Game.PrefabsList.PanelActions.SetActive(false);
        //Rework: This needs go next only if this is single-state action
        if (!(Phases.CurrentSubPhase.GetType() == typeof(SubPhases.SelectTargetSubPhase)) && !(Phases.CurrentSubPhase.GetType() == typeof(SubPhases.BarrelRollSubPhase)))
        {
            Phases.Next();
        }
    }

}
