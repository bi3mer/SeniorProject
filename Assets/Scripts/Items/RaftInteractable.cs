using UnityEngine;
using System.Collections;

public class RaftInteractable : InteractableObject {
    [SerializeField]
    private RaftMovement raftMovement;

    /// <summary>
    /// Gets or sets the raft category object.
    /// </summary>
    /// <value>The raft.</value>
    public RaftCategory Raft
    {
        get;
        set;
    }

    /// <summary>
    /// Sets action for raft as board raft
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        raftMovement = GetComponentInChildren<RaftMovement>();
        raftMovement.SetMaxSpeed(Raft.Speed);

        SetAction
        (
            delegate
            {
                BoardRaft();
            }
        );
    }

    /// <summary>
    /// When leaving the trigger area. Used to signal an interactable object is not in range.For raft interactions.
    /// </summary>
    /// <param name="other">Collider with trigger</param>
    void OnTriggerExit(Collider other)
    {
        this.Show = false;
    }

    /// <summary>
    /// When colliding with a trigger. Used for interactable object interaction. For raft interactions.
    /// </summary>
    /// <param name="other">Collider with trigger</param>
    void OnTriggerEnter(Collider other)
    {
        this.Show = true;
    }

    /// <summary>
    /// Player boards the raft and assumes raft controls until the player dismounts.
    /// </summary>
    public void BoardRaft()
    {
        Game.Instance.PlayerInstance.Controller.BoardRaft(raftMovement);

        // update raft's interactivity
        this.Text = raftMovement.DisembarkRaftText;
        this.SetAction(delegate { DisembarkRaft(); });

        // Give the raft the player's animator to control.
        raftMovement.PlayerAnimator = Game.Instance.PlayerInstance.Controller.PlayerAnimator;

        // Notify subscribers
        Game.Instance.EventManager.RaftBoarded();
    }

    /// <summary>
    /// Player disembarks the raft and resumes player movement.
    /// </summary>
    /// <param name="raftMovement"></param>
    public void DisembarkRaft()
    {
        Game.Instance.PlayerInstance.Controller.DisembarkRaft(raftMovement);

        // update raft's interactivity
        this.Text = raftMovement.BoardRaftText;
        this.SetAction(delegate { BoardRaft(); });
    }
}
