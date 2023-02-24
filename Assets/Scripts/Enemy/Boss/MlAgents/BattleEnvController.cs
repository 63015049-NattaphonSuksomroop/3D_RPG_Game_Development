
using Character;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.Barracuda;
using UnityEngine;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class BattleEnvController : MonoBehaviour
    {
        [SerializeField]
        [System.Serializable]
        public class PlayerInfo
        {
            public AgentDrArm Agent;
            [HideInInspector]
            public Vector3 StartingPos;
            [HideInInspector]
            public Quaternion StartingRot;
        }

        /// <summary>
        /// Max Academy steps before this platform resets
        /// </summary>
        /// <returns></returns>
        [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 25000;

        /// <summary>
        /// The area bounds.
        /// </summary>

        /// <summary>
        /// We will be changing the ground material based on success/failue
        /// </summary>

        public enum BattleMode
        {
            Easy,
            Normal,
            Hard,
            Demo,
            Pause,
            Default
        }

        public Boolean Training;
        public BattleMode mode;
        // Brain to use when difficulty is easy
        public NNModel easyBattleBrain;
        // Brain to use when difficulty is normal
        public NNModel normalBattleBrain;
        // Brain to use when difficulty is hard
        public NNModel hardBattleBrain;

        //List of Agents On Platform
        public List<PlayerInfo> AgentsList = new List<PlayerInfo>();

        private SimpleMultiAgentGroup m_PlayerAgentGroup;
        private SimpleMultiAgentGroup m_NPCAgentGroup;

        private int m_ResetTimer;

        [HideInInspector]
        public GameObject playerFire = null;
        [HideInInspector]
        public GameObject npcFire = null;

        public EventHandler<BattleEndEventArgs> BattleEndEvent;

        // Start is called before the first frame update
        void Start()
        {
            // Initialize TeamManager
            m_PlayerAgentGroup = new SimpleMultiAgentGroup();
            m_NPCAgentGroup = new SimpleMultiAgentGroup();
            Debug.Log("[Battle] start.");
            foreach (var item in AgentsList)
            {
                item.StartingPos = item.Agent.transform.position;
                item.StartingRot = item.Agent.transform.rotation;
                if (item.Agent.team == Team.Player)
                {
                    m_PlayerAgentGroup.RegisterAgent(item.Agent);
                    item.Agent.GetComponent<MlPlayerManager>().FiredAbilityEvent = playerFireInvoked;
                }
                else
                {
                    m_NPCAgentGroup.RegisterAgent(item.Agent);
                    item.Agent.GetComponent<MlPlayerManager>().FiredAbilityEvent = npcFireInvoked;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (mode != BattleMode.Default)
            {
                foreach (var item in AgentsList)
                {
                    configureAgent(mode, item.Agent);
                }
                mode = BattleMode.Default;
            }
        }

        void FixedUpdate()
        {
            int retEpisode = 0;
            float playerReward = 0f;
            float NPCReward = 0f;

            // check if agent is dead
            foreach (var item in AgentsList)
            {
                if (item.Agent._manager.Stats.IsDead)
                {
                    float reward = item.Agent.Lose(m_ResetTimer);
                    if (item.Agent.team == Team.Player)
                    {
                        // NPC wins
                        retEpisode = 2;
                        playerReward = reward;
                    }
                    else
                    {
                        // Player wins
                        retEpisode = 1;
                        NPCReward = reward;
                    }
                }
            }
            if (retEpisode != 0)
            {
                foreach (var item in AgentsList)
                {
                    if (!item.Agent._manager.Stats.IsDead)
                    {
                        float reward = item.Agent.Win();
                        if (item.Agent.team == Team.Player)
                        {
                            // Player wins
                            playerReward = reward;
                        }
                        else
                        {
                            // NPC wins
                            NPCReward = reward;
                        }
                    }
                }
                m_NPCAgentGroup.EndGroupEpisode();
                m_PlayerAgentGroup.EndGroupEpisode();
                if (Training)
                {
                    switch (retEpisode)
                    {
                        case 1:
                            Debug.Log("[End Episode] Player win at step " + m_ResetTimer + ".  (player reward: " + playerReward + ", NPC reward: " + NPCReward + ")");
                            break;
                        case 2:
                            Debug.Log("[End Episode] Player lose at step " + m_ResetTimer + ". (player reward: " + playerReward + ", NPC reward: " + NPCReward + ")");
                            break;
                    }
                    ResetScene();
                }
                else
                {
                    mode = BattleMode.Pause;
                    BattleEndEvent?.Invoke(this, new BattleEndEventArgs() { BattleResult = retEpisode });
                }
                return;
            }

            // check timeout
            if (Training)
            {
                m_ResetTimer += 1;
            }
            if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
            {
                foreach (var item in AgentsList)
                {
                    if (item.Agent.team == Team.Player)
                    {
                        playerReward = item.Agent.GetCumulativeReward();
                    }
                    else
                    {
                        NPCReward = item.Agent.GetCumulativeReward();
                    }
                }
                Debug.Log("[End Episode] Timeout at " + m_ResetTimer + ". (both rewards: " + playerReward + ")");
                m_PlayerAgentGroup.GroupEpisodeInterrupted();
                m_NPCAgentGroup.GroupEpisodeInterrupted();
                ResetScene();
                return;
            }

        }

        public void configureAgent(BattleMode mode, AgentDrArm agent, InferenceDevice inferenceDevice = InferenceDevice.Default)
        {
            agent.enableBattle = mode != BattleMode.Pause;
            //agent._mode = mode;
            if (agent.team == Team.Player && mode != BattleMode.Demo && !Training)
            {
                agent.SetModel("BossBattle", null, InferenceDevice.Default);
            }
            else
            {
                switch (mode)
                {
                    case BattleMode.Easy:
                        agent.SetModel("BossBattle", easyBattleBrain, inferenceDevice);
                        break;
                    case BattleMode.Normal:
                        agent.SetModel("BossBattle", normalBattleBrain, inferenceDevice);
                        break;
                    case BattleMode.Hard:
                        agent.SetModel("BossBattle", hardBattleBrain, inferenceDevice);
                        break;
                    case BattleMode.Demo:
                        agent.SetModel("BossBattle", normalBattleBrain, inferenceDevice);
                        break;
                    case BattleMode.Pause:
                        agent.SetModel("BossBattle", null, InferenceDevice.Default);
                        break;
                }
            }
        }

        public void ResetScene()
        {
            m_ResetTimer = 0;

            //Reset Agents
            foreach (var item in AgentsList)
            {
                CharacterController controller = item.Agent.GetComponent<CharacterController>();
                controller.enabled = false;

                if (Training)
                {
                    var randomPos = UnityEngine.Random.Range(-4f, 4f);
                    var newStartPos = item.StartingPos + new Vector3(randomPos, 0f, randomPos);
                    var rot = item.Agent.rotSign * UnityEngine.Random.Range(80.0f, 100.0f);
                    var newRot = Quaternion.Euler(0, rot, 0);
                    item.Agent.transform.SetPositionAndRotation(newStartPos, newRot);
                }
                else
                {
                    item.Agent.transform.SetPositionAndRotation(item.StartingPos, item.StartingRot);
                }
                item.Agent._manager.GetComponent<MlPlayerManager>().ResetPlayer();
                controller.enabled = true;
            }

        }

        public void PauseBattle()
        {
            mode = BattleMode.Pause;
        }

        public void StartBattleEasy()
        {
            mode = BattleMode.Easy;
        }

        public void StartBattleNormal()
        {
            mode = BattleMode.Normal;
        }

        public void StartBattleHard()
        {
            mode = BattleMode.Hard;
        }

        public void StartBattleDemo()
        {
            mode = BattleMode.Demo;
        }

        void playerFireInvoked(object sender, FiredAbilityEventArgs e)
        {
            playerFire = e.AbilityGameObject;
        }

        void npcFireInvoked(object sender, FiredAbilityEventArgs e)
        {
            npcFire = e.AbilityGameObject;
        }

        public class BattleEndEventArgs : EventArgs
        {
            public int BattleResult;
        }
    }

}

