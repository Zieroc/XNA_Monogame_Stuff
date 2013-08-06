using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Windows.Storage;
using System.Xml.Serialization;

namespace TankGame.ScoreData
{
    public class ScoreManager
    {
        #region Variables

        private List<HighScore> scores;
        private static ScoreManager instance;

        #endregion

        #region Constructor

        private ScoreManager()
        {
            scores = new List<HighScore>();
        }

        public static ScoreManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ScoreManager();
            }

            return instance;
        }

        #endregion

        #region Properties

        public List<HighScore> Scores
        {
            get { return scores; }
        }

        public HighScore GetScoreAt(int i)
        {
            if (scores.Count <= i + 1)
            {
                return scores[i];
            }
            else
            {
                HighScore highScore = new HighScore(0, "N/A");
                return highScore;
            }
        }

        public void SetScoreAt(int i, HighScore score)
        {
            if (scores.Count <= i + 1)
            {
                 scores[i] = score;
            }
        }

        #endregion

        #region UpdateScores

        //Check if this score should be in the top ten, if so return true otherwise return false
        public bool CheckScores(int score)
        {
            if (scores.Count < 10)
            {
                return true; //If high scores aren't full this one must be in the top ten
            }
            else
            {
                bool higherScore = false;
                for (int i = 0; i < 10; i++)
                {
                    if (scores[i].Score < score)
                    {
                        higherScore = true;
                    }
                }

                return higherScore;
            }
        }

        public int CheckNewScore(int score)
        {
            bool scorefound = false;
            int loc = 10;

            for (int i = 0; i < scores.Count; i++)
            {
                if (!scorefound)
                {
                    if (scores[i].Score < score)
                    {
                        scorefound = true;
                        loc = i;
                    }
                }
            }

            if (!scorefound)
            {
                //Score was not set so list must not have a full ten scores in it so add score to end of list
                if (scores.Count < 10)
                {
                    loc =  scores.Count;
                }
            }

            return loc;
        }

        public void AddScore(HighScore score)
        {
            bool scoreSet = false;

            for (int i = 0; i < scores.Count; i++)
            {
                if (!scoreSet)
                {
                    if (scores[i].Score < score.Score)
                    {
                        scoreSet = true;
                        scores.Insert(i, score);
                        if (scores.Count == 11)
                        {
                            //We have gone over the top ten
                            scores.RemoveAt(10);
                        }
                    }
                }
            }

            if (!scoreSet)
            {
                //Score was not set so list must not have a full ten scores in it so add score to end of list
                if (scores.Count < 10)
                {
                    scores.Add(score);
                }
            }
        }

        #endregion

        #region Initalize

        public void InitalizeScores()
        {
            try
            {
                Load();
            }
            catch (FileNotFoundException)
            {
            }
        }

        #endregion

        #region Load & Save

        public async void Save()
        {
            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.CreateFileAsync("Scores.dat", CreationCollisionOption.ReplaceExisting);
                using (Stream s = await file.OpenStreamForWriteAsync())
                {
                    //DataContractSerializer ser = new DataContractSerializer(typeof(List<HighScore>));
                    XmlSerializer ser = new XmlSerializer(typeof(List<HighScore>));
                    ser.Serialize(s, scores);
                    s.Dispose();
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
            }
        }

        public async void Load()
        {
            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.CreateFileAsync("Scores.dat", CreationCollisionOption.OpenIfExists);
                using (Stream s = await file.OpenStreamForReadAsync())
                {
                    //XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(s, new XmlDictionaryReaderQuotas());
                    //DataContractSerializer ser = new DataContractSerializer(typeof(List<HighScore>));
                    XmlSerializer ser = new XmlSerializer(typeof(List<HighScore>));
                    scores = (List<HighScore>)ser.Deserialize(s);
                    s.Dispose();
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
        }

        #endregion
    }
}
