using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Player : MonoThing
{
    public float JumpPower => jumpPower;
    [SerializeField] float jumpPower;
    public float DashPower => dashPower;
    [SerializeField] float dashPower;

    public float DashCooltime { get => dashCooltime; set { dashCooltime = value; } }
    [SerializeField] float dashCooltime;
    public float BaseSpeed => baseSpeed;
    [SerializeField] float baseSpeed;
    public float SurfaceLevel => surfaceLevel;
    [SerializeField] float surfaceLevel;

    public DeathType DeathType => deathType;
    DeathType deathType = DeathType.NotYetDead; 



    new protected void Awake()
    {
        base.Awake();
        AddModule(new DashIndicator(this)).Init();
        AddModule(new PlayerMovement(this)).Init();
        AddModule(new Oxygen(this)).Init(100f);
        AddModule(new Effector(this)).Init();
        AddModule(new StateMachine<Player>(this)).Init(
            new Dictionary<string, State<Player>>(){
                {"Land" , new PlayerLandState() },
                { "Water", new PlayerWaterState()},
                { "Dead", new PlayerDeadState()}
            }, "Land");
        AddModule(new BloodEmiter(this)).Init();


    }

    // Update is called once per frame
    new protected void Update()
    {
        base.Update();
        // print(Rigidbody.linearVelocity.magnitude);
    }

    public void Dead(DeathType deathType)
    {
        this.deathType = deathType;
        GetModule<StateMachine<Player>>().ChangeState("Dead");
    }
}

public enum DeathType
{
    NotYetDead,
    Suffocated,
    Bitten,
    Stab,
    Ground,
    bomb

}