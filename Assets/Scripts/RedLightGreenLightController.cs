using System.Collections;
using UnityEngine;

public class RedLightGreenLightController : Photon.PunBehaviour
{
    public class PlayerMovement : Photon.PunBehaviour
    {
        public Vector3 lastPosition;

        void Update()
        {
            // Update lastPosition during movement
            if (photonView.isMine)
            {
                lastPosition = transform.position;
            }
        }
    }
    public enum GameState
    {
        RedLight,
        GreenLight,
        GameOver
    }

    public float redLightDuration = 5f;
    public float greenLightDuration = 5f;

    private GameState currentState;

    private void Start()
    {
        if (photonView.isMine)
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RedLightState());
        yield return StartCoroutine(GreenLightState());
    }

    private IEnumerator RedLightState()
    {
        currentState = GameState.RedLight;
        Debug.Log("Red Light!");

        // Do any UI or game state updates

        yield return new WaitForSeconds(redLightDuration);

        // Transition to the next state
        photonView.RPC("TransitionToGreenLight", PhotonTargets.All);
    }

    private IEnumerator GreenLightState()
    {
        currentState = GameState.GreenLight;
        Debug.Log("Green Light!");

        // Do any UI or game state updates

        yield return new WaitForSeconds(greenLightDuration);

        // Transition to the next state
        photonView.RPC("TransitionToRedLight", PhotonTargets.All);
    }

    [PunRPC]
    private void TransitionToRedLight()
    {
        // Nothing to do here as the coroutine will handle it
    }

    [PunRPC]
    private void TransitionToGreenLight()
    {
        // Nothing to do here as the coroutine will handle it
    }

    private string CheckForPlayerMovement()
    {
        // Implement the logic to check if a player moved during the green light
        // Compare the current position with the last position
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        Vector3 currentPosition = transform.position;

        if (Vector3.Distance(currentPosition, playerMovement.lastPosition) > 0.1f)
        {
            // Player moved, return the player's name as the loser
            return photonView.owner.NickName;
        }

        // Player did not move
        return null;
    }
}

