using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Player : MonoThing, ISavable
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

    [HideInInspector] public int bgmId;

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

        bgmId = AudioManager.Instance.PlaySound("BGM", transform.root, 0.2f, 999);
    }

    // Update is called once per frame
    new protected void Update()
    {
        base.Update();
        // print(Rigidbody.linearVelocity.magnitude);
    }

    public void Dead(DeathType deathType)
    {
        if (deathType == DeathType.NotYetDead) return;
        this.deathType = deathType;
        GetModule<StateMachine<Player>>().ChangeState("Dead");
    }

    public void LoadData(Database data)
    {

    }

    public void SaveData(ref Database data)
    {
        data.attempt += 1;
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