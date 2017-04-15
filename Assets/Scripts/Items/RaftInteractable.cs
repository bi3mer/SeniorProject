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
    	// do not run set up unless Raft is set
    	if(Raft != null)
    	{
	        base.SetUp();

	        raftMovement = GetComponentInChildren<RaftMovement>();
	        raftMovement.SetMaxSpeed(Raft.Speed);
	        Text = raftMovement.BoardRaftText;

	        SetAction
	        (
	            delegate
	            {
	                BoardRaft();
	            }
	        );
	    }
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
