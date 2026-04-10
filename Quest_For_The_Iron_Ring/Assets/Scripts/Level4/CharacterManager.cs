using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    public enum CharacterType
    {
        Basic,
        Artist,
        Hacker,
        Gamer,
        Jock,
        GymRat,
        AI
    }

    [SerializeField] private CharacterType selectedCharacter = CharacterType.Basic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public CharacterType GetSelectedCharacter()
    {
        return selectedCharacter;
    }

    public void SetBasic()
    {
        selectedCharacter = CharacterType.Basic;
    }

    public void SetArtist()
    {
        selectedCharacter = CharacterType.Artist;
    }

    public void SetHacker()
    {
        selectedCharacter = CharacterType.Hacker;
    }

    public void SetGamer()
    {
        selectedCharacter = CharacterType.Gamer;
    }

    public void SetJock()
    {
        selectedCharacter = CharacterType.Jock;
    }

    public void SetGymRat()
    {
        selectedCharacter = CharacterType.GymRat;
    }

    public void SetAI()
    {
        selectedCharacter = CharacterType.AI;
    }

    public bool HasLevel4SpeedBoost()
    {
        return selectedCharacter == CharacterType.Jock || selectedCharacter == CharacterType.AI;
    }

    public bool HasLevel4BurnoutRecovery()
    {
        return selectedCharacter == CharacterType.Jock || selectedCharacter == CharacterType.AI;
    }
}