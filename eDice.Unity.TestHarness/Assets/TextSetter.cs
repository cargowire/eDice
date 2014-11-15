using System;
using System.Linq;

using eDice;

using UnityEngine;
using System.Collections;

public class TextSetter : MonoBehaviour
{

    private GUIText diceValue;
    private int lastRoll = 0;
    private string lastActivity = "";

    private eDice.IDiceRegistration registration;

	// Use this for initialization
	void Start() 
    {
        diceValue = (GUIText)GameObject.Find("DiceText").guiText;
	    registration = eDice.eDice.Register();
        registration.Pair();

        this.registration.DongleConnected += this.registration_DongleConnected;
        this.registration.DongleDisconnected += this.registration_DongleDisconnected;
        this.registration.DiceRolled += this.registration_DiceRolled;
        this.registration.DicePower += this.registration_DicePower;
        this.registration.DiceShaken += this.registration_DiceShaken;
    }

    void registration_DongleDisconnected(object sender, DongleEventArgs dongleEventArgs)
    {
        this.lastActivity = string.Join("\n", dongleEventArgs.DongleIds.Select(id => "Dongle " + id + " disconnected").ToArray());
    }

    void registration_DongleConnected(object sender, DongleEventArgs dongleEventArgs)
    {
        this.lastActivity = string.Join("\n", dongleEventArgs.DongleIds.Select(id => "Dongle " + id + " connected").ToArray());
    }

    void registration_DicePower(object sender, eDice.DiceStateEventArgs e)
    {
        this.lastActivity = string.Join("\n", e.DiceStates.Select(state => "Dice " + state.Id + " power" + state.Power).ToArray());
    }

    private void registration_DiceShaken(object sender, DiceStateEventArgs e)
    {
        this.lastActivity = string.Join("\n", e.DiceStates.Select(state => "Dice " + state.Id + " shaken. Power " + state.Power).ToArray());
    }

    void registration_DiceRolled(object sender, eDice.DiceStateEventArgs e)
    {
        this.lastRoll = (e.DiceStates.LastOrDefault() ?? new DiceState()).Value ?? 0;
        this.lastActivity = string.Join("\n", e.DiceStates.Select(state => "Dice " + state.Id + " rolled " + state.Power).ToArray());
    }

    void OnDestroy()
    {
        if(this.registration != null)
        {
            this.registration.DongleConnected -= this.registration_DongleConnected;
            this.registration.DongleDisconnected -= this.registration_DongleDisconnected;
            this.registration.DicePower -= this.registration_DicePower;
            this.registration.DiceShaken -= this.registration_DiceShaken;
            this.registration.DiceRolled -= this.registration_DiceRolled;
            this.registration.Dispose();
            this.registration = null;
        }
    }

	// Update is called once per frame
    void Update()
    {
        diceValue.text = string.Format("Last Dice Roll: {0}\n\nLast Activity: {1}", this.lastRoll, this.lastActivity);
    }
}
