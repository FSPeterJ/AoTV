using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class EventSystem
{

    //Player Movements
    public delegate void PlayerPositionUpdateHandler(Vector3 PlayerPosition);
    public static event PlayerPositionUpdateHandler onPlayerPositionUpdate;
    public static void PlayerPositionUpdate(Vector3 PlayerPosition)
    {
        if (onPlayerPositionUpdate != null) onPlayerPositionUpdate(PlayerPosition);
    }

    //Mouse to world events
    public delegate void MousePositionUpdateHandler(Vector3 MousePosition);
    public static event MousePositionUpdateHandler onMousePositionUpdate;
    public static void MousePositionUpdate(Vector3 MousePosition)
    {
        if (onMousePositionUpdate != null) onMousePositionUpdate(MousePosition);
    }

    //player damage events
    public delegate void PlayerHealthUpdateHandler(int hp, int hpmax);
    public static event PlayerHealthUpdateHandler onPlayerHealthUpdate;
    public static void PlayerHealthUpdate(int hp, int hpmax)
    {

        if (onPlayerHealthUpdate != null) onPlayerHealthUpdate(hp, hpmax);
    }

    //player Died
    public delegate void PlayerDeathHandler();
    public static event PlayerDeathHandler onPlayerDeath;
    public static void PlayerDeath()
    {
        if (onPlayerDeath != null) onPlayerDeath();
    }

    //Teleport Cooldown
    public delegate void TeleportCooldownHandler(float seconds, float max);
    public static event TeleportCooldownHandler onTeleportCooldown;
    public static void TeleportCooldown(float seconds, float max)
    {
        if (onTeleportCooldown != null) onTeleportCooldown(seconds, max);
    }
    //Player Score
    public delegate void PlayerScorehandler(int score);
    public static event PlayerScorehandler onPlayerScore;
    public static void IncScore(int score)
    {
        if (onPlayerScore != null) onPlayerScore(score);
    }


    //Ranged Cooldown
    public delegate void RangedCooldownHandler(float seconds, float max);
    public static event RangedCooldownHandler onRangedCooldown;
    public static void RangedCooldown(float seconds, float max)
    {
        if (onRangedCooldown != null) onRangedCooldown(seconds, max);
    }

    //Spin Cooldown
    public delegate void SpinCooldownHandler(float seconds, float max);
    public static event SpinCooldownHandler onSpinCooldown;
    public static void SpinCooldown(float seconds, float max)
    {
        if (onSpinCooldown != null) onSpinCooldown(seconds, max);
    }

    //Increase Score
    public delegate void ScoreIncreaseHandler(uint score);
    public static event ScoreIncreaseHandler onScoreIncrease;
    public static void ScoreIncrease(uint score)
    {
        if (onScoreIncrease != null) onScoreIncrease(score);
    }

    //Lives Count
    public delegate void LivesCountHandler(uint lives);
    public static event LivesCountHandler onLivesCount;
    public static void LivesCount(uint lives)
    {
        if (onLivesCount != null) onLivesCount(lives);
    }

    //Spin Time
    public delegate void SpinTimeHandler(float seconds, float max);
    public static event SpinTimeHandler onSpinTime;
    public static void SpinTime(float seconds, float max)
    {
        if (onSpinTime != null) onSpinTime(seconds, max);
    }

    //Game Paused
    public delegate void GamePausedToggleHandler();
    public static event GamePausedToggleHandler onGamePausedToggle;
    public static void GamePausedToggle()
    {
        if (onGamePausedToggle != null) onGamePausedToggle();
    }

    //UI Back Button
    public delegate void UI_BackHandler();
    public static event UI_BackHandler onUI_Back;
    public static void UI_Back()
    {
        if (onUI_Back != null) onUI_Back();
    }
    //Player Grounded
    public delegate void PlayerGroundedHandler(bool grounded);
    public static event PlayerGroundedHandler onPlayerGrounded;
    public static void PlayerGrounded(bool grounded)
    {
        if (onPlayerGrounded != null) onPlayerGrounded(grounded);
    }

    // Story Dialogue
    public delegate void StoryDialogueHandler();
    public static event StoryDialogueHandler onStoryDialogue;
    public static void StoryDialogue()
    {
        if (onStoryDialogue != null) onStoryDialogue();
    }

    // Display Interact
    public delegate void UI_InteractHandler(bool interact);
    public static event UI_InteractHandler onUI_Interact;
    public static void UI_Interact(bool interact)
    {
        if (onUI_Interact != null) onUI_Interact(interact);
    }

    
    public delegate void UI_KeyChangeHandler(int interact);
    public static event UI_KeyChangeHandler onUI_KeyChange;
    public static void UI_KeyChange(int interact)
    {
        if (onUI_KeyChange != null) onUI_KeyChange(interact);
    }

    public delegate void UI_KeyCountHandler(int interact);
    public static event UI_KeyCountHandler onUI_KeyCount;
    public static void UI_KeyCount(int interact)
    {
        if (onUI_KeyCount != null) onUI_KeyCount(interact);
    }

}
