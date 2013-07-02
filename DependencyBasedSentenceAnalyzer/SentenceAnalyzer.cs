using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DependencyBasedSentenceAnalyzer;
using VerbInflector;
using SCICT.NLP.Utility;

namespace DependencyBasedSentenceAnalyzer
{
    public class SentenceAnalyzer
    {
        /// <summary>
        /// It should be assigned before using any method
        /// </summary>
        private static string verbDicPath;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dicPath">verb dictionary path</param>
        public SentenceAnalyzer(string dicPath)
        {
            verbDicPath = dicPath;
        }

        public void ResetVerbDicPath(string newPath)
        {
            verbDicPath = newPath;
        }

        /// <summary>
        /// makes a partial dependency tree in where only verb parts are tagged
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        private static List<DependencyBasedToken> MakePartialDependencyTree(string[] sentence)
        {
            var tree = new List<DependencyBasedToken>();

			var dic = VerbPartTagger.MakePartialTree(sentence,verbDicPath);
			int indexOfOriginalWords = 0;
			bool addZamir = false;
			string zamirString = "";
			NumberType ZamirNumberType = NumberType.INVALID;
			PersonType ZamirShakhsType = PersonType.PERSON_NONE;
			string zamirLemma = "";
			int offset = 0;
			int realPosition = 0;

            foreach (KeyValuePair<int, KeyValuePair<string, KeyValuePair<int, object>>> keyValuePair in dic)
            {
				addZamir = false;
				zamirString = "";
				ZamirShakhsType = PersonType.PERSON_NONE; 
				ZamirNumberType = NumberType.INVALID;
				zamirLemma = "";
				realPosition = keyValuePair.Key + 1;
				int position = keyValuePair.Key + 1 + offset;

                string wordForm = keyValuePair.Value.Key;
                int head = keyValuePair.Value.Value.Key;
                string deprel = "_";
                object obj = keyValuePair.Value.Value.Value;
                string lemma = "_";
                int wordCount = wordForm.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;
                PersonType person = PersonType.PERSON_NONE;
                NumberType number = NumberType.INVALID;
				TensePositivity posit=TensePositivity.POSITIVE;
				TensePassivity voice=TensePassivity.ACTIVE;
                TenseFormationType tma = TenseFormationType.TenseFormationType_NONE;


				indexOfOriginalWords += wordCount;



                var CPOSTag = "_";
                if (obj is VerbInflection)
                {
                    var newObj = (VerbInflection)obj;
                    tma = newObj.TenseForm;
                    var personType = newObj.Person;
                    person = personType;
                    number = NumberType.SINGULAR;
					posit=newObj.Positivity;
					voice=newObj.Passivity;
                    if (personType == PersonType.FIRST_PERSON_PLURAL || personType == PersonType.SECOND_PERSON_PLURAL || personType == PersonType.THIRD_PERSON_PLURAL)
                    {
                        number = NumberType.PLURAL;
                    }
                    lemma = newObj.VerbRoot.SimpleToString();
                    CPOSTag = "V";

					if(newObj.ZamirPeyvasteh!=AttachedPronounType.AttachedPronoun_NONE)
					{
						addZamir = true;
						zamirString = newObj.AttachedPronounString;
						offset++;
						switch (newObj.ZamirPeyvasteh)
						{
						case AttachedPronounType.FIRST_PERSON_PLURAL:
							ZamirNumberType = NumberType.PLURAL;
							ZamirShakhsType = PersonType.FIRST_PERSON_PLURAL;
							zamirLemma = "مان";
							break;
						case AttachedPronounType.FIRST_PERSON_SINGULAR:
							ZamirNumberType = NumberType.SINGULAR;
							ZamirShakhsType = PersonType.FIRST_PERSON_SINGULAR;
							zamirLemma = "م";
							break;
						case AttachedPronounType.SECOND_PERSON_PLURAL:
							ZamirNumberType = NumberType.PLURAL;
							ZamirShakhsType = PersonType.SECOND_PERSON_PLURAL;
							zamirLemma = "تان";
							break;
						case AttachedPronounType.SECOND_PERSON_SINGULAR:
							ZamirNumberType = NumberType.SINGULAR;
							ZamirShakhsType = PersonType.SECOND_PERSON_SINGULAR;
							zamirLemma = "ت";
							break;
						case AttachedPronounType.THIRD_PERSON_PLURAL:
							ZamirNumberType = NumberType.PLURAL;
							ZamirShakhsType = PersonType.THIRD_PERSON_PLURAL;
							zamirLemma = "شان";
							break;
						case AttachedPronounType.THIRD_PERSON_SINGULAR:
							ZamirNumberType = NumberType.SINGULAR;
							ZamirShakhsType = PersonType.THIRD_PERSON_SINGULAR;
							zamirLemma = "ش";
							break;
						}
					}


                }
				if(obj is MostamarSaz)
				{
					var newObj=(MostamarSaz)obj;
					deprel = "PROG";
					lemma = "داشت#دار";

					var headObj =(VerbInflection)dic.ElementAt(head).Value.Value.Value;
					person = headObj.Person;
					number = NumberType.SINGULAR;
					var personType = headObj.Person;
					if (personType == PersonType.FIRST_PERSON_PLURAL || personType == PersonType.SECOND_PERSON_PLURAL || personType == PersonType.THIRD_PERSON_PLURAL)
					{
						number = NumberType.PLURAL;
					}
					tma = TenseFormationType.HAAL_SAADEH;
					if (newObj.Type == "MOSTAMAR_SAAZ_GOZASHTEH")
						tma = TenseFormationType.GOZASHTEH_SADEH;
					CPOSTag = "V";
				}
                if (obj is string)
                {
                    var newObj = (string)obj;
                    if (newObj == "POSDEP")
                    {
                        deprel = newObj;
                    }
                    else if (newObj == "VERBAL-PREPOSIOTION")
                    {
                        deprel = "VPRT";
                    }
                    else if (newObj == "NON-VERBAL-ELEMENT")
                    {
                        deprel = "NVE";
                    }
                    else if (newObj == "MOSTAMAR_SAAZ_HAAL" || newObj == "MOSTAMAR_SAAZ_GOZASHTEH")
                    {
                        deprel = "PROG";
                        lemma = "داشت#دار";

                        var headObj = (VerbInflection)dic.ElementAt(head).Value.Value.Value;
                        person = headObj.Person;
                        number = NumberType.SINGULAR;
                        var personType = headObj.Person;
                        if (personType == PersonType.FIRST_PERSON_PLURAL || personType == PersonType.SECOND_PERSON_PLURAL || personType == PersonType.THIRD_PERSON_PLURAL)
                        {
                            number = NumberType.PLURAL;
                        }
                        tma = TenseFormationType.HAAL_SAADEH;
                        if (newObj == "MOSTAMAR_SAAZ_GOZASHTEH")
                            tma = TenseFormationType.GOZASHTEH_SADEH;
                        CPOSTag = "V";
                    }
                }
				if (!addZamir)
				{

                var mfeat = new MorphoSyntacticFeatures(number, person, tma,posit,voice);
					var dependencyToken = new DependencyBasedToken(position, wordForm, lemma, CPOSTag, "_", head, deprel, wordCount,
                                                               mfeat,Chasbidegi.TANHA);
                tree.Add(dependencyToken);
				}
				else
				{
					var mfeat1 = new MorphoSyntacticFeatures(number, person, tma,posit,voice);
					var mfeat2 = new MorphoSyntacticFeatures(ZamirNumberType, ZamirShakhsType, TenseFormationType.TenseFormationType_NONE,TensePositivity.POSITIVE,TensePassivity.ACTIVE);
					var dependencyToken1 = new DependencyBasedToken(position, wordForm.Substring(0, wordForm.Length - zamirString.Length), lemma, CPOSTag, "_",
					                                                head, deprel, wordCount,
					                                                mfeat1,Chasbidegi.NEXT);
					var dependencyToken2 = new DependencyBasedToken(position + 1, zamirString, zamirLemma, "CPR", "CPR",
					                                                position, "OBJ", 1,
					                                                mfeat2,Chasbidegi.PREV);
					tree.Add(dependencyToken1);
					tree.Add(dependencyToken2);
				}
            }
            return tree;
        }

        /// <summary>
        /// makes a partial dependency tree in where only verb parts are tagged
        /// You need to have a POS tagger and a lemmatizer
        /// The tagset is similar to Bijankhan corpus
        /// </summary>
        /// <param name="sentence">sentence words</param>
        /// <param name="posSentence">POS tags</param>
        /// <param name="lemmas">lemmas</param>
        /// <param name="morphoSyntacticFeatureses">set of morphosyntactic features</param>
        /// <returns></returns>
        private static List<DependencyBasedToken> MakePartialDependencyTree(string[] sentence, string[] posSentence, string[] lemmas, MorphoSyntacticFeatures[] morphoSyntacticFeatureses)
        {
            var tree = new List<DependencyBasedToken>();
            string[] outpos;
            var dic = VerbPartTagger.MakePartialTree(sentence, posSentence, out outpos,lemmas,verbDicPath);
            int indexOfOriginalWords = 0;
            bool addZamir = false;
            string zamirString = "";
            NumberType ZamirNumberType = NumberType.INVALID;
            PersonType ZamirShakhsType = PersonType.PERSON_NONE;
            string zamirLemma = "";
            int offset = 0;
            int realPosition = 0;
           foreach (KeyValuePair<int, KeyValuePair<string, KeyValuePair<int, object>>> keyValuePair in dic)
            {
                addZamir = false;
                zamirString = "";
                ZamirShakhsType = PersonType.PERSON_NONE; 
               ZamirNumberType = NumberType.INVALID;
               zamirLemma = "";
                realPosition = keyValuePair.Key + 1;
               int position = keyValuePair.Key + 1 + offset;
                string wordForm = keyValuePair.Value.Key;
                int head = keyValuePair.Value.Value.Key;
                string deprel = "_";
                object obj = keyValuePair.Value.Value.Value;
                string lemma = "_";
                string FPOS = "_";
                int wordCount = wordForm.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;
                PersonType person = PersonType.PERSON_NONE;
                NumberType number = NumberType.INVALID;
                TenseFormationType tma = TenseFormationType.TenseFormationType_NONE;
				TensePositivity posit=TensePositivity.POSITIVE;
				TensePassivity voice=TensePassivity.ACTIVE;
                if (wordCount == 1)
                {
                    lemma = lemmas[indexOfOriginalWords];
                    person = morphoSyntacticFeatureses[indexOfOriginalWords].Person;
                    number = morphoSyntacticFeatureses[indexOfOriginalWords].Number;
                    tma = morphoSyntacticFeatureses[indexOfOriginalWords].TenseMoodAspect;
					posit=morphoSyntacticFeatureses[indexOfOriginalWords].Positivity;
                }
                indexOfOriginalWords += wordCount;

               if (obj is VerbInflection)
                {
                    var newObj = (VerbInflection)obj;
                    tma = newObj.TenseForm;
                   person = newObj.Person;
					posit=newObj.Positivity;
					voice=newObj.Passivity;
                   if(newObj.Passivity==TensePassivity.ACTIVE)
                   {
                       FPOS = "ACT";
                   }
                   else
                   {
                       FPOS = "PASS";
                   }
                 
                   if(newObj.ZamirPeyvasteh!=AttachedPronounType.AttachedPronoun_NONE)
                   {
                       addZamir = true;
                       zamirString = newObj.AttachedPronounString;
                       offset++;
                       switch (newObj.ZamirPeyvasteh)
                       {
                           case AttachedPronounType.FIRST_PERSON_PLURAL:
                               ZamirNumberType = NumberType.PLURAL;
                               ZamirShakhsType = PersonType.FIRST_PERSON_PLURAL;
                               zamirLemma = "مان";
                               break;
                           case AttachedPronounType.FIRST_PERSON_SINGULAR:
                               ZamirNumberType = NumberType.SINGULAR;
                               ZamirShakhsType = PersonType.FIRST_PERSON_SINGULAR;
                               zamirLemma = "م";
                               break;
                           case AttachedPronounType.SECOND_PERSON_PLURAL:
                               ZamirNumberType = NumberType.PLURAL;
                               ZamirShakhsType = PersonType.SECOND_PERSON_PLURAL;
                               zamirLemma = "تان";
                               break;
                           case AttachedPronounType.SECOND_PERSON_SINGULAR:
                               ZamirNumberType = NumberType.SINGULAR;
                               ZamirShakhsType = PersonType.SECOND_PERSON_SINGULAR;
                               zamirLemma = "ت";
                               break;
                           case AttachedPronounType.THIRD_PERSON_PLURAL:
                               ZamirNumberType = NumberType.PLURAL;
                               ZamirShakhsType = PersonType.THIRD_PERSON_PLURAL;
                               zamirLemma = "شان";
                               break;
                           case AttachedPronounType.THIRD_PERSON_SINGULAR:
                               ZamirNumberType = NumberType.SINGULAR;
                               ZamirShakhsType = PersonType.THIRD_PERSON_SINGULAR;
                               zamirLemma = "ش";
                               break;
                       }
                   }
                    number = NumberType.SINGULAR;
                    if (person == PersonType.PERSON_NONE)
                    {
                        number = NumberType.INVALID;
                    }
                    if (person == PersonType.FIRST_PERSON_PLURAL || person == PersonType.SECOND_PERSON_PLURAL || person == PersonType.THIRD_PERSON_PLURAL)
                    {
                        number = NumberType.PLURAL;
                    }
                    lemma = newObj.VerbRoot.SimpleToString();
                }
                if (obj is string)
                {
                    var newObj = (string)obj;
                    if (newObj == "POSDEP")
                    {
                        deprel = newObj;
                    }
                    else if (newObj == "VERBAL-PREPOSIOTION")
                    {
                        deprel = "VPRT";
                    }
                    else if (newObj == "NON-VERBAL-ELEMENT")
                    {
                        deprel = "NVE";
                    }
                    else if(newObj == "MOSTAMAR_SAAZ_HAAL" ||  newObj == "MOSTAMAR_SAAZ_GOZASHTEH")
                    {
                        deprel = "PROG";
                        lemma = "داشت#دار";
                        var headObj = (VerbInflection)dic.ElementAt(head).Value.Value.Value;
                        person = headObj.Person;
                        number = NumberType.SINGULAR;
						 
                        var personType = headObj.Person;
                        if (personType == PersonType.FIRST_PERSON_PLURAL || personType == PersonType.SECOND_PERSON_PLURAL || personType == PersonType.THIRD_PERSON_PLURAL)
                        {
                            number = NumberType.PLURAL;
                        }
                        tma = TenseFormationType.HAAL_SAADEH;
                        if(newObj == "MOSTAMAR_SAAZ_GOZASHTEH")
                            tma = TenseFormationType.GOZASHTEH_SADEH;
                    }
                }
                if (!addZamir)
                {
                    var mfeat = new MorphoSyntacticFeatures(number, person, tma,posit,voice);
                    var dependencyToken = new DependencyBasedToken(position, wordForm, lemma, outpos[realPosition - 1], FPOS,
                                                                   head, deprel, wordCount,
                                                                   mfeat,Chasbidegi.TANHA);
                    tree.Add(dependencyToken);
                }
                else
                {
                    var mfeat1 = new MorphoSyntacticFeatures(number, person, tma,posit,voice);
                    var mfeat2 = new MorphoSyntacticFeatures(ZamirNumberType, ZamirShakhsType, TenseFormationType.TenseFormationType_NONE,TensePositivity.POSITIVE,TensePassivity.ACTIVE);
                    var dependencyToken1 = new DependencyBasedToken(position, wordForm.Substring(0, wordForm.Length - zamirString.Length), lemma, outpos[realPosition - 1], FPOS,
                                                                   head, deprel, wordCount,
                                                                   mfeat1,Chasbidegi.NEXT);
                    var dependencyToken2 = new DependencyBasedToken(position + 1, zamirString, zamirLemma, "CPR", "CPR",
                                                                   position, "OBJ", 1,
                                                                   mfeat2,Chasbidegi.PREV);
                    tree.Add(dependencyToken1);
                    tree.Add(dependencyToken2);
                }

            }
            return tree;
        }

        /// <summary>
        /// returns a verbbasedsentence
        /// You need to have a POS tagger and a lemmatizer
        /// The tagset is similar to Bijankhan corpus
        /// </summary>
        /// <param name="sentence">sentence words</param>
        /// <param name="posSentence">POS tags</param>
        /// <param name="lemmas">lemmas</param>
        /// <param name="morphoSyntacticFeatureses">set of morphosyntactic features</param>
        /// <returns></returns>
        public static VerbBasedSentence MakeVerbBasedSentence(string[] sentence, string[] posSentence, string[] lemmas, MorphoSyntacticFeatures[] morphoSyntacticFeatureses)
        {
            return new VerbBasedSentence(MakePartialDependencyTree(sentence, posSentence,lemmas,morphoSyntacticFeatureses));
        }

        /// <summary>
        /// returns a verbbasedsentence
        /// </summary>
        /// <param name="sentence">a string of sentence words</param>
        /// <returns></returns>
        public static VerbBasedSentence MakeVerbBasedSentence(string[] sentence)
        {
            return new VerbBasedSentence(MakePartialDependencyTree(sentence));
        }

        /// <summary>
        /// returns a verbbasedsentence
        /// </summary>
        /// <param name="sentence">a raw sentence</param>
        /// <returns></returns>
        public static VerbBasedSentence MakeVerbBasedSentence(string sentence)
        {
            sentence = StringUtil.RefineAndFilterPersianWord(sentence);
			var tokenized=sentence.Replace("  "," ").Replace("  "," ").Split(" ".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
           // var tokenized = PersianWordTokenizer.Tokenize(sentence,false);
            return MakeVerbBasedSentence(tokenized);
        }
    }
}
