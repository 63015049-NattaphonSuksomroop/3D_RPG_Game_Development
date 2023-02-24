
using Player;
using PlayerInput;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
//using static BattleEnvController;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public enum Team
    {
        Player = 0,
        NPC = 1
    }

    public class AgentDrArm : Agent
    {
        [SerializeField] bool _isPlayer;

        [HideInInspector]
        public Team team;
        EnvironmentParameters m_ResetParams;
        BehaviorParameters m_BehaviorParameters;
        [HideInInspector]
        public bool enableBattle = true;

        [HideInInspector]
        public MlPlayerManager _manager;
        [HideInInspector]
        public MlPlayerManager _enemyManager;
        private PlayerInputController _inputController;
        public Vector3 initialPos;
        public float rotSign;

        float m_Existential;

        private BattleEnvController envController;
        private GameObject selfFire = null;
        private GameObject enemyFire = null;

        internal BattleMode _mode;
        public enum BattleMode
        {
            Easy,
            Normal,
            Hard,
            Demo,
            Pause,
            Default
        }
        // Start is called before the first frame update
        void Start()
        {
            envController = GetComponentInParent<BattleEnvController>();
            _inputController = this.gameObject.GetComponent<PlayerInputController>();
            _manager = this.gameObject.GetComponent<MlPlayerManager>();
            if (m_BehaviorParameters.TeamId == (int)Team.Player)
            {
                _enemyManager = this.transform.parent.transform.Find("ML-NPC").GetComponent<MlPlayerManager>();
                selfFire = envController.playerFire;
                enemyFire = envController.npcFire;
            }
            else
            {
                _enemyManager = this.transform.parent.transform.Find("ML-Player").GetComponent<MlPlayerManager>();
                selfFire = envController.npcFire;
                enemyFire = envController.playerFire;
            }
            _manager.ML_Can.SetNewLockOnTarget(_enemyManager);
            _manager.SetTargetLock(_enemyManager);

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Initialize()
        {
            envController = GetComponentInParent<BattleEnvController>();
            if (envController != null)
            {
                m_Existential = 1f / envController.MaxEnvironmentSteps;
            }
            else
            {
                m_Existential = 1f / MaxStep;
            }

            m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();
            if (m_BehaviorParameters.TeamId == (int)Team.Player)
            {
                team = Team.Player;
                initialPos = new Vector3(transform.position.x - 5f, .5f, transform.position.z);
                rotSign = 1f;
            }
            else
            {
                team = Team.NPC;
                initialPos = new Vector3(transform.position.x + 5f, .5f, transform.position.z);
                rotSign = -1f;
            }

            m_ResetParams = Academy.Instance.EnvironmentParameters;

        }

        public override void OnEpisodeBegin()
        {

        }

        public override void CollectObservations(VectorSensor sensor)
        {
            if (m_BehaviorParameters.TeamId == (int)Team.Player)
            {
                selfFire = envController.playerFire;
                enemyFire = envController.npcFire;
            }
            else
            {
                selfFire = envController.npcFire;
                enemyFire = envController.playerFire;
            }

            // Collect my state and add them to observations
            bool throwSelfFire;
            Vector3 posSelfFire;
            if (selfFire == null)
            {
                throwSelfFire = false;
                posSelfFire = new Vector3(0, 0, 0);
            }
            else
            {
                throwSelfFire = true;
                // Calculate the player's fireball position relative to the agent.
                // Normalize the pos by dividing 30, which is same as RayLength
                posSelfFire = (transform.InverseTransformPoint(selfFire.transform.position)) / 30.0f;
            }
            /* WORKSHOP TO DO: uncomment this out! add awareness of current health/stamina/mana
            sensor.AddObservation(_manager.Stats.CurrentHealth / _manager.Stats.MaxHealth);
            sensor.AddObservation(_manager.Stats.CurrentStamina / _manager.Stats.MaxStamina);
            sensor.AddObservation(_manager.Stats.CurrentMana / _manager.Stats.MaxMana);
            */
            sensor.AddObservation(_manager.RollFlag);
            sensor.AddObservation(_manager.IsInteracting);
            sensor.AddObservation(throwSelfFire);
            sensor.AddObservation(posSelfFire);

            // Collect enemy's state and add them to observations
            bool throwEnemyFire;
            Vector3 posEnemyFire;
            if (enemyFire == null)
            {
                throwEnemyFire = false;
                posEnemyFire = (transform.InverseTransformPoint(_enemyManager.transform.position)) / 30.0f;
            }
            else
            {
                throwEnemyFire = true;
                // Calculate the enemy's fireball position relative to the agent.
                posEnemyFire = (transform.InverseTransformPoint(enemyFire.transform.position)) / 30.0f;
            }
            int isEnemyFacingMe = (Vector3.Dot(_manager.transform.localPosition - _enemyManager.transform.localPosition, _enemyManager.transform.forward)) > 0 ? 1 : 0;

            //WORKSHOP TO DO: add awareness of enemy's current health/stamina/mana

            sensor.AddObservation(_enemyManager.RollFlag);
            sensor.AddObservation(_enemyManager.IsInteracting);
            sensor.AddObservation(throwEnemyFire);
            sensor.AddObservation(posEnemyFire);
            sensor.AddObservation(isEnemyFacingMe);

        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            // Assign the reward (existential penalty)
            //WORKSHOP TO DO: on each tick Add Reward of -m_Existential 

            // Take the action for the agent
            ActAgent(actionBuffers);
        }

        public void ActAgent(ActionBuffers actionBuffers)
        {
            if (!_isPlayer || _mode == BattleMode.Default || _mode == BattleMode.Demo)
            {
                ApplyMlMovement(actionBuffers);
            }
        }

        private void ApplyMlMovement(ActionBuffers actionBuffers)
        {
            // Apply to joystick movement
            var actionZ = Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);
            var actionX = Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);
            Vector2 moveVector = new Vector2(actionZ, actionX);
            _inputController.Move(moveVector);

            // Discrete actions
            if (actionBuffers.DiscreteActions[0] == 1)
            {
                _inputController.Attack();
            }
            else if (actionBuffers.DiscreteActions[0] == 2)
            {
                _inputController.Roll();
            }
            else if (actionBuffers.DiscreteActions[0] == 3)
            {
                _inputController.Fire();
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;
            var discreteActionsOut = actionsOut.DiscreteActions;

            if (enableBattle)
            {
                continuousActionsOut[0] = Input.GetAxis("Horizontal");
                continuousActionsOut[1] = Input.GetAxis("Vertical");
                if (Input.GetKey(KeyCode.Joystick1Button0))
                {
                    // attack
                    discreteActionsOut[0] = 1;
                }
                else if (Input.GetKey(KeyCode.Joystick1Button2))
                {
                    // roll
                    discreteActionsOut[0] = 2;
                }
                else if (Input.GetKey(KeyCode.Joystick1Button1))
                {
                    // fire
                    discreteActionsOut[0] = 3;
                }
                else
                {
                    // do nothing
                    discreteActionsOut[0] = 0;
                }
            }
            else
            {
                // do nothing
                continuousActionsOut[0] = 0.0f;
                continuousActionsOut[1] = 0.0f;
                discreteActionsOut[0] = 0;
            }

        }

        public float Win()
        {
            AddReward(1f);
            return GetCumulativeReward();
        }
        public float Lose(int resetTimer)
        {
            AddReward(-1f + (float)resetTimer / envController.MaxEnvironmentSteps);
            return GetCumulativeReward();
        }


    }

}

