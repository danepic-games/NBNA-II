using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Components.Managers;
using Components.Handlers;
using Components.Controllers;

namespace Components.Controllers {
    public class MatchController : MonoBehaviour {
        public GameObject[] players;
        private int playerLayer;
        private int shadowLayer;
        private int defaultLayer;
        private int mapObjectLayer;
        private int groundLayer;

        public GameObject resultPanel;
        public GameObject iconPlayerOne;
        public GameObject iconPlayerTwo;

        public Transform playerOnePosition;
        public Transform playerTwoPosition;

        public List<ObjectHandler> playersTeamOne = new List<ObjectHandler>();
        public List<ObjectHandler> playersTeamTwo = new List<ObjectHandler>();

        //TODO: Listas são usados por outras classes, tentar trazer essa regra pra essa classe
        public List<Transform> teamOne = new List<Transform>();
        public List<Transform> teamTwo = new List<Transform>();
        public List<Transform> independents = new List<Transform>();

        private Transform startPlayerOnePosition;
        private Transform startPlayerTwoPosition;
        // Use this for initialization
        void Awake() {
            //        var playerOne = CharacterSelectionContext.playerOne;
            //        if (playerOne != null) {
            //            var gameObjPlayerOne = Resources.Load<GameObject>(playerOne.character.gameObjectPath);
            //            var scriptPlayerOne = gameObjPlayerOne.GetComponent<LF2Character>();
            //
            //            scriptPlayerOne.team = playerOne.team;
            //            scriptPlayerOne.playerType = playerOne.player;
            //
            //            Instantiate(gameObjPlayerOne, playerOnePosition.position, Quaternion.identity);
            //        }
            //
            //        var playerTwo = CharacterSelectionContext.playerTwo;
            //        if (playerTwo != null) {
            //            var gameObjPlayerTwo = Resources.Load<GameObject>(playerTwo.character.gameObjectPath);
            //            var scriptPlayerTwo = gameObjPlayerTwo.GetComponent<LF2Character>();
            //
            //            scriptPlayerTwo.team = playerTwo.team;
            //            scriptPlayerTwo.playerType = playerTwo.player;
            //
            //            Instantiate(gameObjPlayerTwo, playerTwoPosition.position, Quaternion.identity);
            //        }

            playerLayer = LayerMask.NameToLayer("Player");
            shadowLayer = LayerMask.NameToLayer("Shadow");
            defaultLayer = LayerMask.NameToLayer("Default");
            mapObjectLayer = LayerMask.NameToLayer("MapObject");
            groundLayer = LayerMask.NameToLayer("Ground");

            Physics.IgnoreLayerCollision(playerLayer, playerLayer);
            Physics.IgnoreLayerCollision(groundLayer, groundLayer);
            Physics.IgnoreLayerCollision(playerLayer, shadowLayer);
            Physics.IgnoreLayerCollision(mapObjectLayer, shadowLayer);
            Physics.IgnoreLayerCollision(defaultLayer, shadowLayer);
        }

        void Start() {
            if (players.Length > 1) {
                startPlayerOnePosition = players[0].transform;
                startPlayerTwoPosition = players[1].transform;
            }

            foreach (Transform player in teamOne) {
                //            playersTeamOne.Add(player.GetComponent<LF2Character>());
            }

            foreach (Transform player in teamTwo) {
                //            playersTeamTwo.Add(player.GetComponent<LF2Character>());
            }
        }

        void Update() {
            if (players.Length > 1) {
                if (Input.GetKeyDown(KeyCode.Backspace)) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }

        void LateUpdate() {
            if (playersTeamOne.Count > 0 && playersTeamOne.TrueForAll(player => player.isDead)) {
                activeResultPanelWithResult(iconPlayerTwo);
            }

            if (playersTeamTwo.Count > 0 && playersTeamTwo.TrueForAll(player => player.isDead)) {
                activeResultPanelWithResult(iconPlayerOne);
            }
        }

        private void activeResultPanelWithResult(GameObject iconPlayerTwo) {
            this.resultPanel.SetActive(true);
            iconPlayerTwo.SetActive(true);
        }
    }
}