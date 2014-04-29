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
        /// This method segments the verb into its parts
        /// 
        /// </summary>
        /// <param name="verbtoken">A verb token</param>
        /// <param name="withPlus">If set to <c>true</c> with plus, this will add plus before prefixes and after suffixes.</param>
        public static string naturalsegment(DependencyBasedToken verbtoken, bool withPlus)
        {
            if (verbtoken.MorphoSyntacticFeats.TenseMoodAspect == TenseFormationType.TenseFormationType_NONE && verbtoken.ChasbidegiType == Chasbidegi.PREV)
                return "+" + verbtoken.WordForm;
            if (verbtoken.MorphoSyntacticFeats.TenseMoodAspect == TenseFormationType.TenseFormationType_NONE)
                return verbtoken.WordForm;
            var kardan = "کرد#کن";
            var feat = verbtoken.MorphoSyntacticFeats;
            var lemma = verbtoken.Lemma;
            var person = feat.Person;
            var tma = feat.TenseMoodAspect;
            var posit = feat.Positivity;
            var voice = feat.Voice;
            var prefix = "";
            var wordForm = verbtoken.WordForm.Replace("می‌", "می");
            var wordformsplit = wordForm.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var splitedlemma = lemma.Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (splitedlemma.Length == 3)
                prefix = splitedlemma[0];

            var present = "";
            var past = "";

            if (splitedlemma.Length >= 2)
            {
                present = splitedlemma[splitedlemma.Length - 1];
                past = splitedlemma[splitedlemma.Length - 2];
            }
            else
            {
                if (lemma.IndexOf("#") == 0)
                    present = lemma.Replace("#", "");
                else
                    past = lemma.Replace("#", "");
            }
            var corelemma = past + "#" + present;
            var output = "";
            switch (tma)
            {
                case TenseFormationType.AAYANDEH:
                    {
                        if (voice == TensePassivity.PASSIVE)
                        {
                            if (corelemma != kardan)
                            {
                                output = "";
                                var prefixindex = 0;
                                if (prefix != "")
                                {
                                    prefixindex = prefix.Length;
                                    output = wordformsplit[0].Substring(0, prefixindex) + "+ ";
                                }

                                output += wordformsplit[0].Substring(prefixindex, wordformsplit[0].Length - prefixindex - 1) + " +" + "ه ";

                                var index = wordformsplit[1].IndexOf("خواه");
                                if (index > 0)
                                    output += wordformsplit[1].Substring(0, index) + "+ ";
                                output += "خواه";
                                output += " +" + wordformsplit[1].Substring(index + 4);
                                output += " " + wordformsplit[2];
                            }
                            else
                            {
                                output = "";
                                var index = wordformsplit[0].IndexOf("خواه");
                                if (index > 0)
                                    output += wordformsplit[0].Substring(0, index) + "+ ";
                                output += "خواه";
                                output += " +" + wordformsplit[0].Substring(index + 4);
                                output += " " + wordformsplit[1];
                            }
                        }
                        else
                        {
                            output = "";
                            var prefixindex = 0;
                            if (prefix != "")
                            {
                                prefixindex = prefix.Length;
                                output = wordformsplit[0].Substring(0, prefixindex) + "+ ";
                            }
                            var index = wordformsplit[0].IndexOf("خواه");
                            if (prefixindex < index)
                                output += wordformsplit[0].Substring(prefixindex, index - prefixindex) + "+ ";
                            output += wordformsplit[0].Substring(index, 4);
                            output += " +" + wordformsplit[0].Substring(index + 4);
                            output += " " + wordformsplit[1];

                        }
                        break;
                    }
                case TenseFormationType.AMR:
                    {
                        output = "";
                        var prefixindex = 0;
                        if (prefix != "")
                        {
                            prefixindex = prefix.Length;
                            output += verbtoken.WordForm.Substring(0, prefixindex) + "+ ";
                        }
                        var lemmaindex = verbtoken.WordForm.IndexOf(present.Substring(1, present.Length - 1));
                        if (prefixindex < lemmaindex - 1)
                        {
                            output += verbtoken.WordForm.Substring(prefixindex, lemmaindex - prefixindex - 1) + "+ ";
                        }
                        output += present;
                        if (lemmaindex + present.Length - 1 < verbtoken.WordForm.Length - 1)
                            output += verbtoken.WordForm.Substring(lemmaindex + present.Length - 1);
                        break;
                    }
                case TenseFormationType.GOZASHTEH_ABAD:
                    {
                        if (voice == TensePassivity.ACTIVE)
                        {
                            output = "";
                            if (prefix != "")
                                output += prefix + "+ ";
                            if (posit == TensePositivity.NEGATIVE)
                                output += "ن+ ";
                            output += past;
                            output += " +ه";
                            output += " ";
                            output += "بود";
                            output += " +ه";
                            var pastindex = wordForm.IndexOf("بود");
                            if (!wordForm.EndsWith("است"))
                                output += " +" + wordForm.Substring(pastindex + 4).Replace("‌", "");
                            else
                                output += " است";
                        }
                        else
                        {
                            if (corelemma != kardan)
                            {
                                output = "";
                                if (prefix != "")
                                    output += prefix + "+ ";

                                output += past;
                                output += " +ه";
                                output += " ";
                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "شد";
                                output += " +ه";
                                output += " ";
                                output += "بود";
                                output += " +ه";
                                var pastindex = wordForm.IndexOf("بود");
                                if (!wordForm.EndsWith("است"))
                                    output += " +" + wordForm.Substring(pastindex + 4).Replace("‌", "");
                                else
                                    output += " است";
                            }
                            else
                            {
                                output = "";
                                if (prefix != "")
                                    output += prefix + "+ ";
                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "شد";
                                output += " +ه";
                                output += " ";
                                output += "بود";
                                output += " +ه";
                                var pastindex = wordForm.IndexOf("بود");
                                if (!wordForm.EndsWith("است"))
                                    output += " +" + wordForm.Substring(pastindex + 4).Replace("‌", "");
                                else
                                    output += " است";
                            }
                        }
                        break;
                    }
                case TenseFormationType.GOZASHTEH_BAEED:
                    {
                        if (voice == TensePassivity.ACTIVE)
                        {
                            output = "";
                            if (prefix != "")
                                output += prefix + "+ ";
                            if (posit == TensePositivity.NEGATIVE)
                                output += "ن+ ";
                            output += past;
                            output += " +ه";
                            output += " ";
                            output += "بود";
                            var pastindex = wordForm.IndexOf("بود");
                            if (pastindex + 3 < wordForm.Length)
                                output += " +" + wordForm.Substring(pastindex + 3);
                        }
                        else
                        {
                            if (corelemma != kardan)
                            {
                                output = "";
                                if (prefix != "")
                                    output += prefix + "+ ";

                                output += past;
                                output += " +ه";
                                output += " ";
                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "شد";
                                output += " +ه";
                                output += " ";
                                output += "بود";
                                var pastindex = wordForm.IndexOf("بود");
                                if (pastindex + 3 < wordForm.Length)
                                    output += " +" + wordForm.Substring(pastindex + 3);
                            }
                            else
                            {
                                output = "";
                                if (prefix != "")
                                    output += prefix + "+ ";
                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "شد";
                                output += " +ه";
                                output += " ";
                                output += "بود";
                                var pastindex = wordForm.IndexOf("بود");
                                if (pastindex + 3 < wordForm.Length)
                                    output += " +" + wordForm.Substring(pastindex + 3);
                            }
                        }
                        break;
                    }
                case TenseFormationType.GOZASHTEH_ELTEZAMI:
                    {
                        if (voice == TensePassivity.ACTIVE)
                        {
                            output = "";
                            if (prefix != "")
                                output += prefix + "+ ";
                            if (posit == TensePositivity.NEGATIVE)
                                output += "ن+ ";
                            output += past + " ";
                            output += "+ه ";
                            output += "باش";
                            var bashindex = wordformsplit[1].IndexOf("باش");
                            if (bashindex + 3 < wordformsplit[1].Length)
                                output += " +" + wordformsplit[1].Substring(bashindex + 3);
                        }

                        else
                        {
                            if (corelemma != kardan)
                            {
                                output = "";
                                if (prefix != "")
                                    output += prefix + "+ ";

                                output += past + " ";
                                output += "+ه ";
                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "شد" + " ";
                                output += "+ه ";
                                output += "باش";
                                var bashindex = wordformsplit[2].IndexOf("باش");
                                if (bashindex + 3 < wordformsplit[2].Length)
                                    output += " +" + wordformsplit[2].Substring(bashindex + 3);
                            }
                            else
                            {
                                output = "";
                                if (prefix != "")
                                    output += prefix + "+ ";

                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "شد" + " ";
                                output += "+ه ";
                                output += "باش";
                                var bashindex = wordformsplit[1].IndexOf("باش");
                                if (bashindex + 3 < wordformsplit[1].Length)
                                    output += " +" + wordformsplit[1].Substring(bashindex + 3);
                            }
                        }
                        break;
                    }
                case TenseFormationType.GOZASHTEH_ESTEMRAARI:
                    {
                        if (voice == TensePassivity.ACTIVE)
                        {
                            var miIndex = wordForm.IndexOf("می");
                            if (miIndex >= 0)
                            {

                                var prefixindex = 0;
                                if (prefix != "")
                                {
                                    output += prefix + "+ ";
                                }

                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "می+ ";
                                var pastindex = wordForm.IndexOf(past);
                                output += past;
                                if (pastindex + past.Length < wordForm.Length)
                                    output += " +" + wordForm.Substring(pastindex + past.Length);
                            }
                        }
                        else
                        {
                            if (corelemma != kardan)
                            {
                                output = "";
                                var prefixindex = 0;
                                if (prefix != "")
                                {
                                    prefixindex = prefix.Length;
                                    output = wordformsplit[0].Substring(0, prefixindex) + "+ ";
                                }

                                output += wordformsplit[0].Substring(prefixindex, wordformsplit[0].Length - prefixindex - 1) + " +" + "ه ";



                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "می+ ";
                                output += "شد";
                                var pastindex = wordformsplit[1].IndexOf("شد");
                                if (pastindex + 2 < wordformsplit[1].Length)
                                    output += " +" + wordformsplit[1].Substring(pastindex + 2);
                            }
                            else
                            {
                                var miIndex = wordForm.IndexOf("می");
                                if (miIndex >= 0)
                                {
                                    var prefixindex = 0;
                                    if (prefix != "")
                                    {
                                        prefixindex = prefix.Length;
                                        output += prefix + "+ ";
                                    }


                                    if (posit == TensePositivity.NEGATIVE)
                                        output += "ن+ ";
                                    output += "می+ ";
                                    var pastindex = wordForm.IndexOf("شد");

                                    output += "شد";
                                    if (pastindex + 2 < wordForm.Length)
                                        output += " +" + wordForm.Substring(pastindex + 2);
                                }
                            }
                        }
                        break;
                    }
                case TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI:
                    {
                        if (voice == TensePassivity.ACTIVE)
                        {
                            output = "";
                            if (prefix != "")
                                output += prefix + "+ ";
                            if (posit == TensePositivity.NEGATIVE)
                                output += "ن+ ";
                            output += "می+ ";
                            output += past;
                            output += " +ه";
                            var pastindex = wordForm.IndexOf(past);
                            if (!wordForm.EndsWith("است"))
                                output += " +" + wordForm.Substring(pastindex + 1 + past.Length).Replace("‌", "");
                            else
                                output += " است";
                        }
                        else
                        {
                            if (corelemma != kardan)
                            {
                                output = "";
                                if (prefix != "")
                                    output += prefix + "+ ";

                                output += past;
                                output += " +ه";
                                output += " ";
                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "می+ ";
                                output += "شد";
                                output += " +ه";
                                var pastindex = wordForm.IndexOf("شده");
                                if (pastindex + 3 < wordForm.Length - 1)
                                    if (!wordForm.EndsWith("است"))
                                        output += " +" + wordForm.Substring(pastindex + 4).Replace("‌", "");
                                    else
                                        output += " است";
                            }
                            else
                            {
                                output = "";
                                if (prefix != "")
                                    output += prefix + "+ ";

                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "می+ ";
                                output += "شد";
                                output += " +ه";
                                var pastindex = wordForm.IndexOf("شده");
                                if (pastindex + 3 < wordForm.Length - 1 && !wordForm.EndsWith("است"))
                                    output += " +" + wordForm.Substring(pastindex + 4).Replace("‌", "");
                                else if (pastindex + 3 < wordForm.Length - 1)
                                    output += " است";
                            }
                        }
                        break;
                    }
                case TenseFormationType.GOZASHTEH_NAGHLI_SADEH:
                    {
                        if (voice == TensePassivity.ACTIVE)
                        {
                            output = "";
                            if (prefix != "")
                                output += prefix + "+ ";
                            if (posit == TensePositivity.NEGATIVE)
                                output += "ن+ ";
                            output += past;
                            output += " +ه";
                            var pastindex = wordForm.IndexOf(past);
                            if (pastindex + past.Length + 1 < wordForm.Length)
                                if (!wordForm.EndsWith("است"))
                                    output += " +" + wordForm.Substring(pastindex + 1 + past.Length).Replace("‌", "");
                                else
                                    output += " است";
                        }
                        else
                        {
                            if (corelemma != kardan)
                            {
                                output = "";
                                if (prefix != "")
                                    output += prefix + "+ ";

                                output += past;
                                output += " +ه";
                                output += " ";
                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "شد";
                                output += " +ه";
                                var pastindex = wordForm.IndexOf("شده");
                                if (pastindex + 3 < wordForm.Length)
                                    if (pastindex + 3 < wordForm.Length && !wordForm.EndsWith("است"))
                                        output += " +" + wordForm.Substring(pastindex + 4).Replace("‌", "");
                                    else
                                        output += " است";
                            }
                            else
                            {
                                output = "";
                                if (prefix != "")
                                    output += prefix + "+ ";

                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "شد";
                                output += " +ه";
                                var pastindex = wordForm.IndexOf("شده");
                                if (pastindex + 3 < wordForm.Length)
                                    if (pastindex + 3 < wordForm.Length && !wordForm.EndsWith("است"))
                                        output += " +" + wordForm.Substring(pastindex + 4).Replace("‌", "");
                                    else
                                        output += " است";
                            }
                        }
                        break;
                    }
                case TenseFormationType.GOZASHTEH_SADEH:
                    {
                        if (voice == TensePassivity.ACTIVE)
                        {
                            var prefixindex = 0;
                            if (prefix != "")
                            {
                                prefixindex = prefix.Length;
                                output += verbtoken.WordForm.Substring(0, prefixindex) + "+ ";
                            }
                            var pastindex = verbtoken.WordForm.IndexOf(past);
                            if (prefixindex < pastindex)
                                output += verbtoken.WordForm.Substring(prefixindex, pastindex - prefixindex) + "+ ";
                            output += past;
                            if (pastindex + past.Length < wordForm.Length)
                                output += " +" + verbtoken.WordForm.Substring(pastindex + past.Length);
                        }
                        else
                        {
                            if (corelemma != kardan)
                            {
                                output = "";
                                var prefixindex = 0;
                                if (prefix != "")
                                {
                                    prefixindex = prefix.Length;
                                    output = wordformsplit[0].Substring(0, prefixindex) + "+ ";
                                }

                                output += wordformsplit[0].Substring(prefixindex, wordformsplit[0].Length - prefixindex - 1) + " +" + "ه ";



                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "شد";
                                var pastindex = wordformsplit[1].IndexOf("شد");
                                if (pastindex + 2 < wordformsplit[1].Length)
                                    output += " +" + wordformsplit[1].Substring(pastindex + 2);
                            }
                            else
                            {
                                var prefixindex = 0;
                                if (prefix != "")
                                {
                                    prefixindex = prefix.Length;
                                    output += wordForm.Substring(0, prefixindex) + "+ ";
                                }

                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+";
                                var pastindex = wordForm.IndexOf("شد");
                                output += "شد";
                                if (pastindex + 2 < wordForm.Length)
                                    output += " +" + wordForm.Substring(pastindex + 2);

                            }
                        }
                        break;
                    }
                case TenseFormationType.HAAL_ELTEZAMI:
                    {
                        if (voice == TensePassivity.ACTIVE)
                        {
                            var prefixindex = 0;
                            if (prefix != "")
                            {
                                prefixindex = prefix.Length;
                                output += verbtoken.WordForm.Substring(0, prefixindex) + "+ ";
                            }
                            if (posit == TensePositivity.NEGATIVE)
                                output += "ن+ ";
                            if (posit == TensePositivity.POSITIVE && !present.StartsWith("ب") && wordForm.StartsWith("ب"))
                                output += "ب+ ";
                            var presentindex = verbtoken.WordForm.IndexOf(present.Substring(1, present.Length - 1));

                            output += present;
                            output += " +" + verbtoken.WordForm.Substring(presentindex + present.Length - 1);
                        }
                        else
                        {
                            if (corelemma != kardan)
                            {
                                output = "";
                                var prefixindex = 0;
                                if (prefix != "")
                                {
                                    prefixindex = prefix.Length;
                                    output = wordformsplit[0].Substring(0, prefixindex) + "+ ";
                                }

                                output += wordformsplit[0].Substring(prefixindex, wordformsplit[0].Length - prefixindex - 1) + " +" + "ه ";



                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "شو ";
                                var presentindex = wordformsplit[1].IndexOf("شو");
                                if (presentindex + 2 < wordformsplit[1].Length)
                                    output += " +" + wordformsplit[1].Substring(presentindex + 2);
                            }
                            else
                            {
                                var prefixindex = 0;
                                if (prefix != "")
                                {
                                    prefixindex = prefix.Length;
                                    output += wordForm.Substring(0, prefixindex) + "+ ";
                                }

                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+";
                                var presentindex = wordForm.IndexOf("شو");
                                output += "شو";
                                output += " +" + wordForm.Substring(presentindex + 2);

                            }
                        }
                        break;
                    }
                case TenseFormationType.HAAL_SAADEH:
                    {
                        if (voice == TensePassivity.ACTIVE)
                        {
                            var prefixindex = 0;
                            if (prefix != "")
                            {
                                prefixindex = prefix.Length;
                                output += verbtoken.WordForm.Substring(0, prefixindex) + "+ ";
                            }
                            var presentindex = verbtoken.WordForm.IndexOf(present);
                            if (prefixindex < presentindex)
                                output += verbtoken.WordForm.Substring(prefixindex, presentindex - prefixindex) + "+ ";
                            output += present;
                            if (presentindex + present.Length < wordForm.Length - 1)
                                output += " +" + verbtoken.WordForm.Substring(presentindex + present.Length);
                        }
                        else
                        {
                            if (corelemma != kardan)
                            {
                                output = "";
                                var prefixindex = 0;
                                if (prefix != "")
                                {
                                    prefixindex = prefix.Length;
                                    output = wordformsplit[0].Substring(0, prefixindex) + "+ ";
                                }

                                output += wordformsplit[0].Substring(prefixindex, wordformsplit[0].Length - prefixindex - 1) + " +" + "ه ";



                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "شو";
                                var presentindex = wordformsplit[1].IndexOf("شو");
                                if (presentindex + 2 < wordformsplit[1].Length)
                                    output += " +" + wordformsplit[1].Substring(presentindex + 2);
                            }
                            else
                            {
                                var prefixindex = 0;
                                if (prefix != "")
                                {
                                    prefixindex = prefix.Length;
                                    output += wordForm.Substring(0, prefixindex) + "+ ";
                                }

                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+";
                                var presentindex = wordForm.IndexOf("شو");
                                output += "شو";
                                output += " +" + wordForm.Substring(presentindex + 2);

                            }
                        }
                        break;
                    }
                case TenseFormationType.HAAL_SAADEH_EKHBARI:
                    {
                        if (voice == TensePassivity.ACTIVE)
                        {
                            var miIndex = wordForm.IndexOf("می");
                            if (miIndex >= 0)
                            {

                                var prefixindex = 0;
                                if (prefix != "")
                                {
                                    output += prefix + "+ ";
                                }

                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "می+ ";
                                var presentindex = wordForm.IndexOf(present);
                                output += present;
                                output += " +" + wordForm.Substring(presentindex + present.Length);
                            }
                            else
                            {
                                if (wordForm == "است")
                                    return wordForm;
                                else if (wordForm.StartsWith("نیست"))
                                {
                                    output = "نی+ ";
                                    output += "ست";
                                    if (wordForm.Length > 4)
                                        output += " +" + wordForm.Substring(4);
                                }
                                else if (wordForm.StartsWith("هست"))
                                {
                                    output += "هست";
                                    if (wordForm.Length > 3)
                                        output += " +" + wordForm.Substring(3);
                                }

                            }
                        }
                        else
                        {
                            if (corelemma != kardan)
                            {
                                output = "";
                                var prefixindex = 0;
                                if (prefix != "")
                                {
                                    prefixindex = prefix.Length;
                                    output = wordformsplit[0].Substring(0, prefixindex) + "+ ";
                                }

                                output += wordformsplit[0].Substring(prefixindex, wordformsplit[0].Length - prefixindex - 1) + " +" + "ه ";



                                if (posit == TensePositivity.NEGATIVE)
                                    output += "ن+ ";
                                output += "می+ ";
                                output += "شو ";
                                var presentindex = wordformsplit[1].IndexOf("شو");
                                if (presentindex + 2 < wordformsplit[1].Length)
                                    output += " +" + wordformsplit[1].Substring(presentindex + 2);
                            }
                            else
                            {
                                var miIndex = wordForm.IndexOf("می");
                                if (miIndex >= 0)
                                {
                                    var prefixindex = 0;
                                    if (prefix != "")
                                    {
                                        prefixindex = prefix.Length;
                                        output += prefix + "+ ";
                                    }


                                    if (posit == TensePositivity.NEGATIVE)
                                        output += "ن+ ";
                                    output += "می+ ";
                                    var presentindex = wordForm.IndexOf("شو");

                                    output += "شو";
                                    output += " +" + wordForm.Substring(presentindex + 2);
                                }
                            }
                        }
                        break;
                    }
                case TenseFormationType.PAYEH_MAFOOLI:
                    {
                        output = "";
                        var prefixindex = 0;
                        if (prefix != "")
                        {
                            prefixindex = prefix.Length;
                            output += verbtoken.WordForm.Substring(0, prefixindex) + "+ ";
                        }
                        var pastindex = verbtoken.WordForm.IndexOf(past);
                        if (prefixindex < pastindex)
                            output += verbtoken.WordForm.Substring(prefixindex, pastindex - prefixindex) + "+ ";
                        output += past;
                        output += " +" + verbtoken.WordForm.Substring(pastindex + past.Length);
                        break;
                    }
                case TenseFormationType.TenseFormationType_NONE:
                    {
                        output = verbtoken.WordForm;
                        break;
                    }
            }
            if (!withPlus)
                output = output.Replace("+", "");
            return output;
        }

        /// <summary>
        /// makes a partial dependency tree in where only verb parts are tagged
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        private static List<DependencyBasedToken> MakePartialDependencyTree(string[] sentence)
        {
            var tree = new List<DependencyBasedToken>();

            var dic = VerbPartTagger.MakePartialTree(sentence, verbDicPath);
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
                TensePositivity posit = TensePositivity.POSITIVE;
                TensePassivity voice = TensePassivity.ACTIVE;
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
                    posit = newObj.Positivity;
                    voice = newObj.Passivity;
                    if (personType == PersonType.FIRST_PERSON_PLURAL || personType == PersonType.SECOND_PERSON_PLURAL || personType == PersonType.THIRD_PERSON_PLURAL)
                    {
                        number = NumberType.PLURAL;
                    }
                    lemma = newObj.VerbRoot.SimpleToString();
                    CPOSTag = "V";

                    if (newObj.ZamirPeyvasteh != AttachedPronounType.AttachedPronoun_NONE)
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
                if (obj is MostamarSaz)
                {
                    var newObj = (MostamarSaz)obj;
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

                    var mfeat = new MorphoSyntacticFeatures(number, person, tma, posit, voice);
                    var dependencyToken = new DependencyBasedToken(position, wordForm, lemma, CPOSTag, "_", head, deprel, wordCount,
                                                               mfeat, Chasbidegi.TANHA);
                    tree.Add(dependencyToken);
                }
                else
                {
                    var mfeat1 = new MorphoSyntacticFeatures(number, person, tma, posit, voice);
                    var mfeat2 = new MorphoSyntacticFeatures(ZamirNumberType, ZamirShakhsType, TenseFormationType.TenseFormationType_NONE, TensePositivity.POSITIVE, TensePassivity.ACTIVE);
                    var dependencyToken1 = new DependencyBasedToken(position, wordForm.Substring(0, wordForm.Length - zamirString.Length), lemma, CPOSTag, "_",
                                                                    head, deprel, wordCount,
                                                                    mfeat1, Chasbidegi.NEXT);
                    var dependencyToken2 = new DependencyBasedToken(position + 1, zamirString, zamirLemma, "CPR", "CPR",
                                                                    position, "OBJ", 1,
                                                                    mfeat2, Chasbidegi.PREV);
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
            var dic = VerbPartTagger.MakePartialTree(sentence, posSentence, out outpos, lemmas, verbDicPath);
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
                TensePositivity posit = TensePositivity.POSITIVE;
                TensePassivity voice = TensePassivity.ACTIVE;
                if (wordCount == 1)
                {
                    lemma = lemmas[indexOfOriginalWords];
                    person = morphoSyntacticFeatureses[indexOfOriginalWords].Person;
                    number = morphoSyntacticFeatureses[indexOfOriginalWords].Number;
                    tma = morphoSyntacticFeatureses[indexOfOriginalWords].TenseMoodAspect;
                    posit = morphoSyntacticFeatureses[indexOfOriginalWords].Positivity;
                }
                indexOfOriginalWords += wordCount;

                if (obj is VerbInflection)
                {
                    var newObj = (VerbInflection)obj;
                    tma = newObj.TenseForm;
                    person = newObj.Person;
                    posit = newObj.Positivity;
                    voice = newObj.Passivity;
                    if (newObj.Passivity == TensePassivity.ACTIVE)
                    {
                        FPOS = "ACT";
                    }
                    else
                    {
                        FPOS = "PASS";
                    }

                    if (newObj.ZamirPeyvasteh != AttachedPronounType.AttachedPronoun_NONE)
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
                    }
                }
                if (!addZamir)
                {
                    var mfeat = new MorphoSyntacticFeatures(number, person, tma, posit, voice);
                    var dependencyToken = new DependencyBasedToken(position, wordForm, lemma, outpos[realPosition - 1], FPOS,
                                                                   head, deprel, wordCount,
                                                                   mfeat, Chasbidegi.TANHA);
                    tree.Add(dependencyToken);
                }
                else
                {
                    var mfeat1 = new MorphoSyntacticFeatures(number, person, tma, posit, voice);
                    var mfeat2 = new MorphoSyntacticFeatures(ZamirNumberType, ZamirShakhsType, TenseFormationType.TenseFormationType_NONE, TensePositivity.POSITIVE, TensePassivity.ACTIVE);
                    var dependencyToken1 = new DependencyBasedToken(position, wordForm.Substring(0, wordForm.Length - zamirString.Length), lemma, outpos[realPosition - 1], FPOS,
                                                                   head, deprel, wordCount,
                                                                   mfeat1, Chasbidegi.NEXT);
                    var dependencyToken2 = new DependencyBasedToken(position + 1, zamirString, zamirLemma, "CPR", "CPR",
                                                                   position, "OBJ", 1,
                                                                   mfeat2, Chasbidegi.PREV);
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
            return new VerbBasedSentence(MakePartialDependencyTree(sentence, posSentence, lemmas, morphoSyntacticFeatureses));
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
            var tokenized = sentence.Replace("  ", " ").Replace("  ", " ").Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            // var tokenized = PersianWordTokenizer.Tokenize(sentence,false);
            return MakeVerbBasedSentence(tokenized);
        }
    }
}
