using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VerbInflector
{
    /// <summary>
    /// To represent an instance of a verb with its details
    /// </summary>
    public class Verb:IComparable
    {
        /// <summary>
        /// Shows the preposition of the verb in the cases of compound verbs with prepositional phrases
        /// </summary>
        public string PrepositionOfVerb;

        /// <summary>
        /// non-verbal element
        /// </summary>
        public string NonVerbalElement;

        /// <summary>
        /// Prefix of the verb
        /// </summary>
        public string Prefix;

        /// <summary>
        /// Past tense root of the verb (in Persian there are two types of root for each verb; i.e. present tense and past tense
        /// </summary>
        public string PastTenseRoot;

        /// <summary>
        /// Present tense root of the verb (in Persian there are two types of root for each verb; i.e. present tense and past tense
        /// </summary>
        public string PresentTenseRoot;

        /// <summary>
        /// Shows whether a verb is Transitive or not
        /// </summary>
        public VerbTransitivity Transitivity;

        /// <summary>
        /// Shows verb type; i.e. 
        /// </summary>
        public VerbType Type;

        /// <summary>
        /// shows whether the verb can be used in imperative form or not
        /// </summary>
        public bool CanBeImperative;

        /// <summary>
        /// Shows the type of vowel at the end of the present tense root
        /// </summary>
        public string PresentRootConsonantVowelEndStem;

        /// <summary>
        /// Shows the type of vowel at the start of the past tense root
        /// </summary>
        public string PastRootVowelStart;

        /// <summary>
        /// Shows the type of vowel at the start of the present tense root
        /// </summary>
        public string PresentRootVowelStart;
      
        /// <summary>
        /// Verb Constructor
        /// </summary>
        /// <param name="hz">Preposition of the verb</param>
        /// <param name="bonmazi">Past tense root</param>
        /// <param name="bonmozareh">Present tense root</param>
        /// <param name="psh">Prefix</param>
        /// <param name="flyar">Non-verbal element</param>
        /// <param name="trnst">Transitivity</param>
        /// <param name="type">Verb type</param>
        /// <param name="amrshdn">CanBeImperative</param>
        /// <param name="vowelEnd">PresentRootConsonantVowelEndStem</param>
        /// <param name="maziVowelStart">PastRootVowelStart</param>
        /// <param name="mozarehVowelStart">PresentRootVowelStart</param>
        public Verb(string hz, string bonmazi, string bonmozareh, string psh, string flyar, VerbTransitivity trnst, VerbType type, bool amrshdn, string vowelEnd,string maziVowelStart,string mozarehVowelStart)
        {
            PrepositionOfVerb = hz;
            NonVerbalElement = flyar;
            Prefix = psh;
            PastTenseRoot = bonmazi;
            PresentTenseRoot = bonmozareh;
            Transitivity = trnst;
            Type = type;
            CanBeImperative = amrshdn;
            PresentRootConsonantVowelEndStem = vowelEnd;
            PastRootVowelStart = maziVowelStart;
            PresentRootVowelStart = mozarehVowelStart;
        }
     
        /// <summary>
        /// Shows whether can have an object attached to it as a pronoun
        /// </summary>
        /// <returns>true if verb is not intransitive</returns>
        public bool IsZamirPeyvastehValid()
        {
            return Transitivity!=VerbTransitivity.InTransitive;
        }

        /// <summary>
        /// An overrided method
        /// </summary>
        /// <returns>An string representation of the object</returns>
        public override string ToString()
        {
            string verbStr;
            if (Prefix != "")
                verbStr = PrepositionOfVerb + " " + NonVerbalElement + " " + Prefix + "#" + PastTenseRoot + "---" + PresentTenseRoot;
            else
                verbStr = PrepositionOfVerb + " " + NonVerbalElement + " " + PastTenseRoot + "---" + PresentTenseRoot;

            verbStr = verbStr.Trim();
            verbStr += "\t" + Transitivity + "\t" + Type;
            return verbStr;
        }

        /// <summary>
        /// A special version of ToString Method. Details are omitted.
        /// </summary>
        /// <returns></returns>
        public string SimpleToString()
        {
            string verbStr;
            if (Prefix != "")
                verbStr = PrepositionOfVerb + " " + NonVerbalElement + " " + Prefix + "#" + PastTenseRoot + "#" + PresentTenseRoot;
            else
                verbStr = PrepositionOfVerb + " " + NonVerbalElement + " " + PastTenseRoot + "#" + PresentTenseRoot;
            verbStr = verbStr.Trim();
            return verbStr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>An exact copy of the object with different memory reference values</returns>
        public Verb Clone()
        {
            var vrb=new Verb(PrepositionOfVerb,PastTenseRoot,PresentTenseRoot,Prefix,NonVerbalElement,Transitivity,Type,CanBeImperative,PresentRootConsonantVowelEndStem,PastRootVowelStart,PresentRootVowelStart);
            return vrb;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (this.Equals(obj))
                return 0;
            if (this.GetHashCode() > obj.GetHashCode())
                return 1;
            return -1;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Verb))
                return false;
            var verb = (Verb) obj;
            if(verb.PastTenseRoot==PastTenseRoot && verb.PresentTenseRoot==PresentTenseRoot&& verb.Prefix==Prefix&& verb.PrepositionOfVerb==PrepositionOfVerb && verb.NonVerbalElement==NonVerbalElement && verb.Transitivity==Transitivity && verb.CanBeImperative==CanBeImperative)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return PrepositionOfVerb.GetHashCode() + NonVerbalElement.GetHashCode() + Prefix.GetHashCode() + PastTenseRoot.GetHashCode() +
                   PresentTenseRoot.GetHashCode() + Transitivity.GetHashCode() + Type.GetHashCode() +
                   CanBeImperative.GetHashCode();
        }
        #endregion
    }
}
