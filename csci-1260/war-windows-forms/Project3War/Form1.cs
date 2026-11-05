using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Project3War
{
    public partial class Form1 : Form
    {

        private PlayerHands PlayerHands;


        private void StartGame()
        {
            playerHands = new PlayerHands();

            // Continue setting up players...
        }




        Dictionary<string, ProgressBar> progressBars = new Dictionary<string, ProgressBar>();
        Dictionary<string, Label> scoreLabels = new Dictionary<string, Label>();

        Deck deck;
        PlayerHands playerHands = new PlayerHands();
        PlayedCards playedCards = new PlayedCards();
        List<Card> pot = new List<Card>();

      

        public Form1()
        {
            InitializeComponent();
            comboBoxPlayers.Items.AddRange(new string[] { "2", "3", "4" });
            comboBoxPlayers.SelectedIndex = 0;

        }
        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnPlayRound_Click(object sender, EventArgs e)
        {
            listBoxRounds.Items.Clear();
            playedCards.Clear();
            pot = new List<Card>();

            foreach (var player in playerHands.GetPlayers())
            {
                var hand = playerHands.GetHand(player);
                if (hand.HasCards())
                {
                    var card = hand.PlayCard();
                    playedCards.AddPlayedCard(player, card);
                    pot.Add(card);
                    listBoxRounds.Items.Add($"{player} plays {card}");
                }
                else
                {
                    listBoxRounds.Items.Add($"{player} is out of cards!");
                }
            }

            var highest = playedCards.GetCards().OrderByDescending(kv => kv.Value).ToList();

            if (highest.Count == 0)
            {
                lblWinner.Text = "No cards played.";
                return;
            }

            var topCard = highest[0].Value;
            var winners = highest.Where(kv => kv.Value.CompareTo(topCard) == 0).ToList();

            if (winners.Count == 1)
            {
                string winner = winners[0].Key;
                playerHands.GetHand(winner).AddCards(pot);
                lblWinner.Text = $"{winner} wins the round!";
            }
            else
            {
                lblWinner.Text = "It's a tie! Going to war!";
                var tiedPlayers = winners.Select(kv => kv.Key).ToList();
                string warWinner = ResolveWar(tiedPlayers, pot);

                if (warWinner != null)
                {
                    playerHands.GetHand(warWinner).AddCards(pot);
                    lblWinner.Text = $"{warWinner} wins the war!";
                }
                else
                {
                    lblWinner.Text = "All players ran out of cards during war.";
                }
            }


            foreach (var player in playerHands.GetPlayers())
            {
                if (playerHands.GetHand(player).Cards.Count == 52)
                {
                    MessageBox.Show($"{player} wins the game!");
                    break;
                }
            
            }
            UpdateScoreDisplay();


        }






        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            listBoxRounds.Items.Clear();
            lblWinner.Text = "";

            int playerCount = int.Parse(comboBoxPlayers.SelectedItem.ToString());
            deck = new Deck();
            Stack<Card> shuffled = deck.GetCards();

            playerHands = new PlayerHands();
            playedCards = new PlayedCards();
            pot = new List<Card>();

            for (int i = 1; i <= playerCount; i++)
                playerHands.AddPlayer($"Player {i}");

            int index = 0;
            var players = playerHands.GetPlayers().ToList();
            while (shuffled.Count > 0)
            {
                string player = players[index % playerCount];
                playerHands.GetHand(player).AddCard(shuffled.Pop());
                index++;
            }

            MessageBox.Show("Game Started!");
            UpdateScoreDisplay();




        }

        private void lblWinner_Click(object sender, EventArgs e)
        {

        }


        private string ResolveWar(List<string> tiedPlayers, List<Card> currentPot)
        {
            Dictionary<string, Card> warCards = new Dictionary<string, Card>();
            List<string> stillTied = new List<string>();

            foreach (var player in tiedPlayers)
            {
                var hand = playerHands.GetHand(player);
                if (hand.Cards.Count >= 2)
                {
                    // Burn one card face down
                    hand.PlayCard();

                    // Play one card face up
                    var faceUp = hand.PlayCard();
                    warCards[player] = faceUp;
                    currentPot.Add(faceUp);
                    listBoxRounds.Items.Add($"{player} plays war card: {faceUp}");
                }
                else
                {
                    // Not enough cards — they lose
                    listBoxRounds.Items.Add($"{player} does not have enough cards for war.");
                }
            }

            if (warCards.Count == 0)
                return null; // All players ran out of cards

            var highestCard = warCards.Values.Max();
            stillTied = warCards.Where(kv => kv.Value.CompareTo(highestCard) == 0)
                                .Select(kv => kv.Key).ToList();

            if (stillTied.Count == 1)
            {
                return stillTied[0]; // Clear winner
            }
            else
            {
                listBoxRounds.Items.Add("Another tie in war! Going deeper...");
                return ResolveWar(stillTied, currentPot); // Go deeper into war
            }
        }

       

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void UpdateScoreDisplay()
        {
            string scoreText = "";

            foreach (var player in playerHands.GetPlayers())
            {
                int count = playerHands.GetHand(player).Cards.Count;
                scoreText += $"{player}: {count} cards   |   ";
            }

            lblScores.Text = scoreText;
        }



    }

}
