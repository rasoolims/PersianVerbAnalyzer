using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DependencyBasedSentenceAnalyzer
{
    public class VerbBasedSentence
    {
        public List<DependencyBasedToken> SentenceTokens { get; set; }
        public List<VerbInSentence> VerbsInSentence { get; set; }

        public VerbBasedSentence(List<DependencyBasedToken> tokens)
        {
            SentenceTokens = tokens;
            VerbsInSentence=new List<VerbInSentence>();
            for (int i = tokens.Count-1; i>=0; i--)
            {
                if(tokens[i].CPOSTag=="V" && tokens[i].DependencyRelation!="PROG")
                {
                    var vis=new VerbInSentence(i);
                    VerbsInSentence.Add(vis);
                }
                if(tokens[i].DependencyRelation=="POSDEP")
                {

                    foreach (VerbInSentence verbInSentence in VerbsInSentence)
                    {
                        if (verbInSentence.LightVerbIndex == tokens[i - 1].HeadNumber)
                        {
                            verbInSentence.NonVerbalElementIndex = i;
                            verbInSentence.VerbalPrepositionIndex = i - 1;
                        }
                    }
                }
                else if(tokens[i].DependencyRelation=="NVE")
                {
                    foreach (VerbInSentence verbInSentence in VerbsInSentence)
                    {
                        if (verbInSentence.LightVerbIndex == tokens[i].HeadNumber)
                        {
                            verbInSentence.NonVerbalElementIndex = i;
                        }
                    }
                }
            }
        }
    }

    public class VerbInSentence:IComparable 
    {
        public int LightVerbIndex { get; set; }
        public int NonVerbalElementIndex { get; set; }
        public int VerbalPrepositionIndex { get; set; }

        public VerbInSentence(int lvIndex, int nveIndex, int vpIndex)
        {
            LightVerbIndex = lvIndex;
            NonVerbalElementIndex = nveIndex;
            VerbalPrepositionIndex = vpIndex;
        }
        public VerbInSentence(int lvIndex, int nveIndex)
            : this(lvIndex, nveIndex, -1)
        {
        }
        public VerbInSentence(int lvIndex)
            : this(lvIndex, -1, -1)
        {
        }

      public new string ToString()
      {
          return LightVerbIndex + "\t" + NonVerbalElementIndex + "\t" + VerbalPrepositionIndex;
      }

        public int CompareTo(object obj)
        {
            return ToString().CompareTo(obj.ToString());
        }

        public override bool Equals(object obj)
        {
            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return ToString().Equals(obj.ToString());
            
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
