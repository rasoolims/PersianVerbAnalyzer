using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VerbInflector
{
    public class VerbPartTagger
    {
        /// <summary>
        /// An object containing all sufficient and necessary data in verb dictionary
        /// </summary>
        private static VerbList verbDic = null;

        private static string verbDicPath="";

		/// <summary>
		/// The conjlist is a list containing all possible words that may stop a verb from getting a progressifier
		/// </summary>
		public static List<string> Conjlist=new List<string>();

		public static void ConstructConjList()
		{
			Conjlist = new List<string> ();
			Conjlist.Add ("و");
			Conjlist.Add ("که");
			Conjlist.Add ("اما");
			Conjlist.Add ("تا");
			Conjlist.Add ("گرچه");
			Conjlist.Add ("اگرچه");
			Conjlist.Add ("چرا");
			Conjlist.Add ("یا");
			Conjlist.Add ("زیرا");
			Conjlist.Add ("اگر");
			Conjlist.Add ("لیکن");
			Conjlist.Add ("چون");
			Conjlist.Add ("همچنین");
			Conjlist.Add ("چرا‌که");
			Conjlist.Add ("ولی");
			Conjlist.Add ("هر‌چند");
			Conjlist.Add ("چراکه");
			Conjlist.Add ("-");
			Conjlist.Add ("هرچند");
			Conjlist.Add ("وگرنه");
			Conjlist.Add ("چنانچه");
			Conjlist.Add ("بلکه");
			Conjlist.Add ("والا");
			Conjlist.Add ("هرچه");
			Conjlist.Add ("ولی‌");
			Conjlist.Add ("ولیکن");
			Conjlist.Add ("بس‌که");
			Conjlist.Add ("ولو");
			Conjlist.Add ("لکن");
			Conjlist.Add ("یعنی");
			Conjlist.Add ("هنوز");
			Conjlist.Add ("مگر");
			Conjlist.Add ("خواه");
			Conjlist.Add ("پس");
			Conjlist.Add ("چو");
			Conjlist.Add ("اینکه");
			Conjlist.Add ("چه");
			Conjlist.Add ("بنابراین");
			Conjlist.Add ("الی");
			Conjlist.Add ("وقتی");
			Conjlist.Add ("اگه");
			Conjlist.Add ("منتهی");
			Conjlist.Add ("،");
			Conjlist.Add ("اگرنه");
			Conjlist.Add ("منتها");
			Conjlist.Add ("بی‌آنکه");
			Conjlist.Add ("والّا");
		}

        public static Dictionary<string, int> StaticDic = new Dictionary<string, int>();

        /// <summary>
        /// retrieves verb strings from dictioanary
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="verbDicPath"></param>
        /// <returns></returns>
        private static Dictionary<int, List<VerbInflection>> GetVerbParts(string[] sentence, string[] posSentence)
        {
			if (Conjlist.Count == 0)
				ConstructConjList ();
            var dic = new Dictionary<int, List<VerbInflection>>();
            if (verbDic == null)
            {
                verbDic = new VerbList(verbDicPath);
            }
            for (int i = 0; i < sentence.Length; i++)
            {
                if (posSentence[i] == "V" || posSentence[i] == "N" || posSentence[i] == "ADJ")
                {
                    bool add = false;

                    //     dic.Add(i, VerbList.VerbShapes.ContainsKey(sentence[i]) ? VerbList.VerbShapes[sentence[i]] : null);

                    if (posSentence[i] == "V")
                    {
                        dic.Add(i, VerbList.VerbShapes.ContainsKey(sentence[i]) ? VerbList.VerbShapes[sentence[i]] : null);
                        add = true;
                    }
                    else if (posSentence[i] == "N" || posSentence[i] == "ADJ")
                    {
                        if (VerbList.VerbShapes.ContainsKey(sentence[i]))
                        {
                            var data = VerbList.VerbShapes[sentence[i]];
                            foreach (var verbInflection in data)
                            {
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                                {
                                    add = true;
                                    dic.Add(i, data);
                                    break;
                                }
                            }

                        }
                        if (!add)
                        {
                            dic.Add(i, null);
                        }
                    }
                    if (dic[i] == null)
                    {
                        bool find = false;
                        if (sentence[i].StartsWith("می"))
                        {
                            string newSen = sentence[i].Remove(0, 2).Insert(0, "می‌");
                            if (VerbList.VerbShapes.ContainsKey(newSen))
                            {
                                sentence[i] = newSen;
                                find = true;
                                dic[i] = VerbList.VerbShapes[newSen];
                            }
                        }
                        else if (sentence[i].StartsWith("نمی"))
                        {
                            string newSen = sentence[i].Remove(0, 2).Insert(0, "نمی‌");
                            if (VerbList.VerbShapes.ContainsKey(newSen))
                            {
                                sentence[i] = newSen;
                                find = true;
                                dic[i] = VerbList.VerbShapes[newSen];
                            }
                        }
                        else if (sentence[i].Contains("ئی"))
                        {
                            string newSen = sentence[i].Replace("ئی", "یی");
                            if (VerbList.VerbShapes.ContainsKey(newSen))
                            {
                                sentence[i] = newSen;
                                find = true;
                                dic[i] = VerbList.VerbShapes[newSen];
                            }
                        }

                        if (!find)
                        {
                            if (!StaticDic.ContainsKey(sentence[i]))
                            {
                                StaticDic.Add(sentence[i], 1);
                            }
                            else
                            {
                                StaticDic[sentence[i]]++;
                            }
                        }
                    }
                }
                else
                {
                    dic.Add(i, null);

                }

            }
            return dic;
        }

        /// <summary>
        /// retrieves verb strings from dictioanary
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="verbDicPath"></param>
        /// <returns></returns>
        private static Dictionary<int, List<VerbInflection>> GetVerbParts(string[] sentence)
        {
			if (Conjlist.Count == 0)
				ConstructConjList ();
            var dic = new Dictionary<int, List<VerbInflection>>();
            if (verbDic == null)
            {
                verbDic = new VerbList(verbDicPath);
            }
            for (int i = 0; i < sentence.Length; i++)
            {
                dic.Add(i, VerbList.VerbShapes.ContainsKey(sentence[i]) ? VerbList.VerbShapes[sentence[i]] : null);
            }
            return dic;
        }

        /// <summary>
        /// reset the verb dictionary according to the verb dictionary path
        /// </summary>
        /// <param name="newDicPath"></param>
        private static void ResetDicPath(string newDicPath)
        {
            if (newDicPath != verbDicPath)
            {
                verbDicPath = newDicPath;
                verbDic = new VerbList(verbDicPath);
            }
        }

        /// <summary>
        /// finds simple and prefix verbs (do not consider compound verbs)
        /// </summary>
        /// Arguments as in AnalyzeSentenceConsiderCompoundVerbs
        /// <param name="sentence"></param>
        /// <param name="posSentence"></param>
        /// <param name="posTokens"></param>
        /// <param name="verbDicPath"></param>
        /// <returns></returns>
        public static Dictionary<int, KeyValuePair<string, object>> AnalyzeSentence(string[] sentence, string[] posSentence, out string[] posTokens, string[] lemmas, out string[] outlemmas)
        {
            var bestDic = new Dictionary<int, KeyValuePair<string, object>>();
            var initDic = GetVerbTokens(sentence, posSentence, out posTokens, lemmas, out outlemmas);

            var mostamars = new Dictionary<int, int>();
            for (int i = 0; i < initDic.Count; i++)
            {
                if (initDic[i].Value != null)
                {
                    var verbInflection = initDic[i].Value;

                    if (verbInflection.VerbRoot.Type == VerbType.SADEH &&
                        verbInflection.ZamirPeyvasteh == AttachedPronounType.AttachedPronoun_NONE &&
                        verbInflection.Positivity == TensePositivity.POSITIVE &&
                        (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH || verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI) &&
                        verbInflection.VerbRoot.PastTenseRoot == "داشت")
                    {
                        int key = i;
                        int value = -1;
                        for (int j = i + 1; j < initDic.Count; j++)
                        {
                            if (initDic[j].Value != null)
                            {
                                var newinfl = initDic[j].Value;
                                if (newinfl.Positivity == TensePositivity.POSITIVE &&
                                    newinfl.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                    newinfl.VerbRoot.PastTenseRoot != "داشت" && newinfl.VerbRoot.PresentTenseRoot != "است" && newinfl.VerbRoot.PresentTenseRoot != "هست")
                                {
                                    value = j;
                                    break;
                                }
								else
									break;
                            }
							if(Conjlist.Contains(sentence[j]))
								break;
                        }
                        if (value > 0)
                        {
                            mostamars.Add(key, value);
                        }
                    }

                    if (verbInflection.VerbRoot.Type == VerbType.SADEH &&
                        verbInflection.ZamirPeyvasteh == AttachedPronounType.AttachedPronoun_NONE &&
                        verbInflection.Positivity == TensePositivity.POSITIVE &&
                        verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                        verbInflection.VerbRoot.PastTenseRoot == "داشت")
                    {
                        int key = i;
                        int value = -1;
                        for (int j = i + 1; j < initDic.Count; j++)
                        {
                            if (initDic[j].Value != null)
                            {
                                var newinfl = initDic[j].Value;

                                if (newinfl.Positivity == TensePositivity.POSITIVE &&
                                    newinfl.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    newinfl.VerbRoot.PastTenseRoot != "داشت")
                                {
                                    value = j;
                                    break;
                                }
								else
									break;

                            }
							if(Conjlist.Contains(sentence[j]))
								break;
                        }
                        if (value > 0)
                        {
                            mostamars.Add(key, value);
                        }
                    }
                }
            }
            for (int i = 0; i < initDic.Count; i++)
            {
                if (initDic[i].Value != null)
                {
                    if (mostamars.ContainsKey(i))
                    {
						var mostamarVal = new MostamarSaz((VerbInflection)initDic[i].Value,-1,"");
                        if (initDic[i].Value.TenseForm == TenseFormationType.HAAL_SAADEH || initDic[i].Value.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                        {
							mostamarVal = new MostamarSaz((VerbInflection)initDic[i].Value, mostamars[i],"MOSTAMAR_SAAZ_HAAL");
                        }
                        if (initDic[i].Value.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                        {
							mostamarVal =new MostamarSaz((VerbInflection)initDic[i].Value, mostamars[i],"MOSTAMAR_SAAZ_GOZASHTEH");

                        }
                        bestDic.Add(i, new KeyValuePair<string, object>(initDic[i].Key, mostamarVal));
                    }
                    else
                    {
                        bestDic.Add(i, new KeyValuePair<string, object>(initDic[i].Key, initDic[i].Value));

                    }
                }
                else
                {
                    bestDic.Add(i, new KeyValuePair<string, object>(initDic[i].Key, initDic[i].Value));
                }
            }
            return bestDic;
        }

        /// <summary>
        /// finds simple and prefix verbs (do not consider compound verbs)
        /// </summary>
        /// Arguments as in AnalyzeSentenceConsiderCompoundVerbs 
        /// <param name="sentence"></param>
        /// <param name="verbDicPath"></param>
        /// <returns></returns>
        public static Dictionary<int, KeyValuePair<string, object>> AnalyzeSentence(string[] sentence)
        {
            var bestDic = new Dictionary<int, KeyValuePair<string, object>>();
            var initDic = GetVerbTokens(sentence);

            var mostamars = new Dictionary<int, int>();
            for (int i = 0; i < initDic.Count; i++)
            {
                if (initDic[i].Value != null)
                {
                    var verbInflection = initDic[i].Value;

                    if (verbInflection.VerbRoot.Type == VerbType.SADEH &&
                        verbInflection.ZamirPeyvasteh == AttachedPronounType.AttachedPronoun_NONE &&
                        verbInflection.Positivity == TensePositivity.POSITIVE &&
                        (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH || verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI) &&
                        verbInflection.VerbRoot.PastTenseRoot == "داشت")
                    {
                        int key = i;
                        int value = -1;
                        for (int j = i + 1; j < initDic.Count; j++)
                        {
                            if (initDic[j].Value != null)
                            {
                                var newinfl = initDic[j].Value;
                                if (newinfl.Positivity == TensePositivity.POSITIVE &&
                                    (newinfl.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI) &&
                                    newinfl.VerbRoot.PastTenseRoot != "داشت" && newinfl.VerbRoot.PresentTenseRoot != "است" && newinfl.VerbRoot.PresentTenseRoot != "هست")
                                {
                                    value = j;
                                    break;
                                }
								else
									break;
                            }
							if(Conjlist.Contains(sentence[j]))
								break;
                        }
                        if (value > 0)
                        {
                            mostamars.Add(key, value);
                        }
                    }

                    if (verbInflection.VerbRoot.Type == VerbType.SADEH &&
                        verbInflection.ZamirPeyvasteh == AttachedPronounType.AttachedPronoun_NONE &&
                        verbInflection.Positivity == TensePositivity.POSITIVE &&
                        verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                        verbInflection.VerbRoot.PastTenseRoot == "داشت")
                    {
                        int key = i;
                        int value = -1;
                        for (int j = i + 1; j < initDic.Count; j++)
                        {
                            if (initDic[j].Value != null)
                            {
                                var newinfl = initDic[j].Value;

                                if (newinfl.Positivity == TensePositivity.POSITIVE &&
                                    newinfl.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    newinfl.VerbRoot.PastTenseRoot != "داشت")
                                {
                                    value = j;
                                    break;
                                }
								else
									break;
                            }
							if(Conjlist.Contains(sentence[j]))
								break;
                        }
                        if (value > 0)
                        {
                            mostamars.Add(key, value);
                        }
                    }
                }
            }
            for (int i = 0; i < initDic.Count; i++)
            {
                if (initDic[i].Value != null)
                {
                    if (mostamars.ContainsKey(i))
                    {
						var mostamarVal =new MostamarSaz((VerbInflection)initDic[i].Value,-1,"");
                        if (initDic[i].Value.TenseForm == TenseFormationType.HAAL_SAADEH || initDic[i].Value.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                        {
							mostamarVal = new MostamarSaz((VerbInflection)initDic[i].Value,mostamars[i], "MOSTAMAR_SAAZ_HAAL");
                        }
                        if (initDic[i].Value.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                        {
							mostamarVal = new MostamarSaz((VerbInflection)initDic[i].Value,mostamars[i],"MOSTAMAR_SAAZ_GOZASHTEH");

                        }
                        bestDic.Add(i, new KeyValuePair<string, object>(initDic[i].Key, mostamarVal));
                    }
                    else
                    {
                        bestDic.Add(i, new KeyValuePair<string, object>(initDic[i].Key, initDic[i].Value));

                    }
                }
                else
                {
                    bestDic.Add(i, new KeyValuePair<string, object>(initDic[i].Key, initDic[i].Value));
                }
            }
            return bestDic;
        }

        /// <summary>
        /// creates an object set of words,verbs and their inflections
        /// </summary>
        /// <param name="sentence">sentence words</param>
        /// <param name="posSentence">sentence words part of speech set</param>
        /// <param name="posTokens">the new sentence words part of speech set (after finding the verbs)</param>
        /// <param name="verbDicPath">path to the verb dictionary</param>
        /// <returns></returns>
        public static Dictionary<int, KeyValuePair<string, object>> AnalyzeSentenceConsiderCompoundVerbs(string[] sentence, string[] posSentence, out string[] posTokens, string[] lemmas)
        {
            string[] outlemmas;
            var bestDic = AnalyzeSentence(sentence, posSentence, out posTokens, lemmas, out outlemmas);
            for (int i = posTokens.Length - 1; i >= 0; i--)
            {
                if (bestDic[i].Value is VerbInflection)
                {
					bool ispassive=false;
                    var verbValue = ((VerbInflection)bestDic[i].Value).VerbRoot;
					if (((VerbInflection)bestDic[i].Value).Passivity==TensePassivity.PASSIVE)
						ispassive=true;
                    if (VerbList.CompoundVerbDic.ContainsKey(verbValue))
                    {
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (posTokens[j] == "DET")
                            {

                                continue;
                            }
                            if (posTokens[j] == "N")
                            {

                                if (VerbList.CompoundVerbDic[verbValue].ContainsKey(bestDic[j].Key))
                                {
                                    if (j > 0 &&
                                        VerbList.CompoundVerbDic[verbValue][bestDic[j].Key].ContainsKey(
                                            bestDic[j - 1].Key))
                                    {

										if(!ispassive || (ispassive && VerbList.CompoundVerbDic[verbValue][bestDic[j].Key][bestDic[j - 1].Key]))
										{
	                                        var item1 = new KeyValuePair<string, object>(bestDic[j].Key,
	                                                                                     new KeyValuePair<string, int>(
	                                                                                         "NON-VERBAL-ELEMENT", i));
	                                        bestDic[j] = item1;
	                                        var item2 = new KeyValuePair<string, object>(bestDic[j - 1].Key,
	                                                                                     new KeyValuePair<string, int>(
	                                                                                        "VERBAL-PREPOSIOTION", i));
	                                        bestDic[j - 1] = item2;
	                                        i = j - 2;
	                                        break;
										}

                                    }
                                    else if ((VerbList.CompoundVerbDic[verbValue][bestDic[j].Key].ContainsKey("") ||
                                              VerbList.CompoundVerbDic[verbValue][outlemmas[j]].ContainsKey("")) &&
                                             posTokens[j] != "P")
                                    {
                                        if (j <= 1 || !(j > 1 && (VerbList.CompoundVerbDic[verbValue].ContainsKey(bestDic[j - 1].Key) && VerbList.CompoundVerbDic[verbValue][bestDic[j - 1].Key].ContainsKey(""))))
                                        {
											if(!ispassive || (ispassive && VerbList.CompoundVerbDic[verbValue][bestDic[j].Key][""]))
											{
                                            var item1 = new KeyValuePair<string, object>(bestDic[j].Key,
                                                                                         new KeyValuePair
                                                                                             <string, int>(
                                                                                             "NON-VERBAL-ELEMENT", i));
                                            bestDic[j] = item1;
                                            i = j - 1;
                                            break;
											}
                                        }
                                    }
                                }
                                else if (VerbList.CompoundVerbDic[verbValue].ContainsKey(outlemmas[j]))
                                {
                                    if (VerbList.CompoundVerbDic[verbValue][outlemmas[j]].ContainsKey(""))
                                    {
                                        if (VerbList.CompoundVerbDic[verbValue][outlemmas[j]].ContainsKey("") &&
                                            posTokens[j] != "P")
                                        {
											if(!ispassive || (ispassive && VerbList.CompoundVerbDic[verbValue][bestDic[j].Key][""]))
											{
                                            var item1 = new KeyValuePair<string, object>(bestDic[j].Key,
                                                                                         new KeyValuePair
                                                                                             <string, int>(
                                                                                             "NON-VERBAL-ELEMENT", i));
                                            bestDic[j] = item1;
                                            i = j - 1;
                                            break;
											}
                                        }
                                    }
                                }
                            }
                            else if (posTokens[j] == "V" || posTokens[j] == "PUNC" || posTokens[j] == "ADV" ||
                                     posTokens[j] == "POSTP" || sentence[j] == "را")
                            {
                                i = j - 1;
                                if (posTokens[j] == "V")
                                    i = j + 1;
                                break;
                            }
                        }
                    }
                }
				else if(bestDic[i].Value is MostamarSaz)
				{
					var thisValue=(MostamarSaz)(bestDic[i].Value);
					if (thisValue.Type=="MOSTAMAR_SAAZ_HAAL" ||  thisValue.Type=="MOSTAMAR_SAAZ_GOZASHTEH")
					{
						var verbValue =thisValue.Inflection.VerbRoot;
						var ispassive=false;
						if (thisValue.Inflection.Passivity==TensePassivity.PASSIVE)
							ispassive=true;
						if (VerbList.CompoundVerbDic.ContainsKey(verbValue))
						{
							for (int j = i - 1; j >= 0; j--)
							{
								if (posTokens[j] == "DET")
								{
									
									continue;
								}
								if (posTokens[j] == "N")
								{
									
									if (VerbList.CompoundVerbDic[verbValue].ContainsKey(bestDic[j].Key))
									{
										if (j > 0 &&
										    VerbList.CompoundVerbDic[verbValue][bestDic[j].Key].ContainsKey(
											bestDic[j - 1].Key))
										{
											if(!ispassive || (ispassive && VerbList.CompoundVerbDic[verbValue][bestDic[j].Key][bestDic[j - 1].Key]))
											{
											var item1 = new KeyValuePair<string, object>(bestDic[j].Key,
											                                             new KeyValuePair<string, int>(
												"NON-VERBAL-ELEMENT", i));
											bestDic[j] = item1;
											var item2 = new KeyValuePair<string, object>(bestDic[j - 1].Key,
											                                             new KeyValuePair<string, int>(
												"VERBAL-PREPOSIOTION", i));
											bestDic[j - 1] = item2;
											i = j - 2;
											break;
											}
											
										}
										else if ((VerbList.CompoundVerbDic[verbValue][bestDic[j].Key].ContainsKey("") ||
										          VerbList.CompoundVerbDic[verbValue][outlemmas[j]].ContainsKey("")) &&
										         posTokens[j] != "P")
										{
											if (j <= 1 || !(j > 1 && (VerbList.CompoundVerbDic[verbValue].ContainsKey(bestDic[j - 1].Key) && VerbList.CompoundVerbDic[verbValue][bestDic[j - 1].Key].ContainsKey(""))))
											{
												if(!ispassive || (ispassive && VerbList.CompoundVerbDic[verbValue][bestDic[j].Key][""]))
												{
												var item1 = new KeyValuePair<string, object>(bestDic[j].Key,
												                                             new KeyValuePair
												                                             <string, int>(
													"NON-VERBAL-ELEMENT", i));
												bestDic[j] = item1;
												bestDic[i]=new KeyValuePair<string, object>(bestDic[i].Key,thisValue.Inflection);
												i = j - 1;
												break;
												}
											}
										}
									}
									else if (VerbList.CompoundVerbDic[verbValue].ContainsKey(outlemmas[j]))
									{
										if (VerbList.CompoundVerbDic[verbValue][outlemmas[j]].ContainsKey(""))
										{
											if (VerbList.CompoundVerbDic[verbValue][outlemmas[j]].ContainsKey("") &&
											    posTokens[j] != "P")
											{
												if(!ispassive || (ispassive && VerbList.CompoundVerbDic[verbValue][bestDic[j].Key][""]))
												{
												var item1 = new KeyValuePair<string, object>(bestDic[j].Key,
												                                             new KeyValuePair
												                                             <string, int>(
													"NON-VERBAL-ELEMENT", i));
												bestDic[j] = item1;
												bestDic[i]=new KeyValuePair<string, object>(bestDic[i].Key,thisValue.Inflection);
												i = j - 1;
												break;
												}
											}
										}
									}
								}
								else if (posTokens[j] == "V" || posTokens[j] == "PUNC" || posTokens[j] == "ADV" ||
								         posTokens[j] == "POSTP" || sentence[j] == "را")
								{
									i = j - 1;
									if (posTokens[j] == "V")
										i = j + 1;
									break;
								}
							}
						}
					}
				}
            }
            return bestDic;
        }

        /// <summary>
        /// creates an object set of words, verbs and inflections of sentence words
        /// </summary>
        /// <param name="sentence">sentence words</param>
        /// <param name="verbDicPath">path of the verb dictionary</param>
        /// <returns></returns>
        public static Dictionary<int, KeyValuePair<string, object>> AnalyzeSentenceConsiderCompoundVerbs(string[] sentence)
        {
            var bestDic = AnalyzeSentence(sentence);
            for (int i = bestDic.Count - 1; i >= 0; i--)
            {
                if (bestDic[i].Value is VerbInflection)
                {
                    var verbValue = ((VerbInflection)bestDic[i].Value).VerbRoot;
						var ispassive=false;
					if(((VerbInflection)bestDic[i].Value).Passivity==TensePassivity.PASSIVE)
						ispassive=true;
                    if (VerbList.CompoundVerbDic.ContainsKey(verbValue))
                    {
                        for (int j = i - 1; j >= 0; j--)
                        {
							if(bestDic[j].Value is VerbInflection){
								i = j - 1;
								break;
							}
                            if (VerbList.CompoundVerbDic[verbValue].ContainsKey(bestDic[j].Key))
                            {
                                if (j > 0 &&
                                    VerbList.CompoundVerbDic[verbValue][bestDic[j].Key].ContainsKey(
                                        bestDic[j - 1].Key))
                                {
									if(!ispassive || (ispassive && VerbList.CompoundVerbDic[verbValue][bestDic[j].Key][bestDic[j - 1].Key]))
									{
                                    var item1 = new KeyValuePair<string, object>(bestDic[j].Key,
                                                                                 new KeyValuePair<string, int>(
                                                                                     "NON-VERBAL-ELEMENT", i));
                                    bestDic[j] = item1;
                                    var item2 = new KeyValuePair<string, object>(bestDic[j - 1].Key,
                                                                                 new KeyValuePair<string, int>(
                                                                                     "VERBAL-PREPOSIOTION", i));
                                    bestDic[j - 1] = item2;
                                    i = j - 2;
                                    break;
									}
                                }
                                else if (VerbList.CompoundVerbDic[verbValue][bestDic[j].Key].ContainsKey(""))
                                {
									if(!ispassive || (ispassive && VerbList.CompoundVerbDic[verbValue][bestDic[j].Key][""]))
									{
                                    var item1 = new KeyValuePair<string, object>(bestDic[j].Key,
                                                                                 new KeyValuePair<string, int>(
                                                                                     "NON-VERBAL-ELEMENT", i));
                                    bestDic[j] = item1;
                                    i = j - 1;
                                    break;
									}
                                }
                            }
                        }
                    }
                }

				else if(bestDic[i].Value is MostamarSaz)
				{
					var thisValue=(MostamarSaz)(bestDic[i].Value);
					if (thisValue.Type=="MOSTAMAR_SAAZ_HAAL" ||  thisValue.Type=="MOSTAMAR_SAAZ_GOZASHTEH")
					{
						var verbValue =thisValue.Inflection.VerbRoot;
						var ispassive=false;
						if(thisValue.Inflection.Passivity==TensePassivity.PASSIVE)
							ispassive=true;
						if (VerbList.CompoundVerbDic.ContainsKey(verbValue))
						{
							for (int j = i - 1; j >= 0; j--)
							{
								if(bestDic[j].Value is VerbInflection){
									i = j - 1;
									break;
								}
								if (VerbList.CompoundVerbDic[verbValue].ContainsKey(bestDic[j].Key))
								{
									if (j > 0 &&
									    VerbList.CompoundVerbDic[verbValue][bestDic[j].Key].ContainsKey(
										bestDic[j - 1].Key))
									{
										if(!ispassive || (ispassive && VerbList.CompoundVerbDic[verbValue][bestDic[j].Key][bestDic[j - 1].Key]))
										{
										var item1 = new KeyValuePair<string, object>(bestDic[j].Key,
										                                             new KeyValuePair<string, int>(
											"NON-VERBAL-ELEMENT", i));
										bestDic[j] = item1;
										var item2 = new KeyValuePair<string, object>(bestDic[j - 1].Key,
										                                             new KeyValuePair<string, int>(
											"VERBAL-PREPOSIOTION", i));
										bestDic[j - 1] = item2;
										bestDic[i]=new KeyValuePair<string, object>(bestDic[i].Key,thisValue.Inflection);
										i = j - 2;
										break;
										}
									}
									else if (VerbList.CompoundVerbDic[verbValue][bestDic[j].Key].ContainsKey(""))
									{
										if(!ispassive || (ispassive && VerbList.CompoundVerbDic[verbValue][bestDic[j].Key][""]))
										{
										var item1 = new KeyValuePair<string, object>(bestDic[j].Key,
										                                             new KeyValuePair<string, int>(
											"NON-VERBAL-ELEMENT", i));
										bestDic[j] = item1;
										bestDic[i]=new KeyValuePair<string, object>(bestDic[i].Key,thisValue.Inflection);
										i = j - 1;
										break;
										}
									}
								}
							}
						}
					}
				}
            }	
            return bestDic;
        }

        /// <summary>
        /// returns a partial  dependency tree
        /// </summary>
        /// <param name="sentence">array of words in the sentence</param>
        /// <param name="posTokens">array of POS tags in the sentence</param>
        /// <param name="lemmas">array of lemmas in the sentence</param>
        /// <param name="verbDicPath">path of dictionary file</param>
        /// <returns></returns>
        public static Dictionary<int, KeyValuePair<string, KeyValuePair<int, object>>> MakePartialTree(string[] sentence, string[] posSentence, out string[] posTokens, string[] lemmas, string dicPath)
        {
            ResetDicPath(dicPath);
            var dic = AnalyzeSentenceConsiderCompoundVerbs(sentence, posSentence, out posTokens, lemmas);
            var partialTree = new Dictionary<int, KeyValuePair<string, KeyValuePair<int, object>>>();
            foreach (int key in dic.Keys)
            {
                string value = dic[key].Key;
                if (dic[key].Value is VerbInflection)
                {
                    partialTree.Add(key, new KeyValuePair<string, KeyValuePair<int, object>>(value, new KeyValuePair<int, object>(-1, dic[key].Value)));

                }
				else if (dic[key].Value is MostamarSaz)
				{
					var newValue = (MostamarSaz)dic[key].Value;
					partialTree.Add(key, new KeyValuePair<string, KeyValuePair<int, object>>(value, new KeyValuePair<int, object>(newValue.Head,newValue)));
					
				}
                else if (dic[key].Value is KeyValuePair<string, int>)
                {
                    var newValue = (KeyValuePair<string, int>)dic[key].Value;
                    if (key > 0 && dic[key - 1].Value is KeyValuePair<string, int>)
                    {
                        var prevValue = (KeyValuePair<string, int>)dic[key - 1].Value;
                        if (prevValue.Key == "VERBAL-PREPOSIOTION")
                            partialTree.Add(key,
                                            new KeyValuePair<string, KeyValuePair<int, object>>(value,
                                                                                                new KeyValuePair
                                                                                                    <int, object>(
                                                                                                    key - 1,
                                                                                                   "POSDEP")));
                        else
                        {

                            partialTree.Add(key,
                                            new KeyValuePair<string, KeyValuePair<int, object>>(value,
                                                                                                new KeyValuePair
                                                                                                    <int, object>(
                                                                                                    newValue.Value - 1,
                                                                                                    newValue.Key)));
                        }

                    }
                    else
                    {
                        partialTree.Add(key, new KeyValuePair<string, KeyValuePair<int, object>>(value, new KeyValuePair<int, object>(newValue.Value, newValue.Key)));
                    }
                }
                else
                {
                    partialTree.Add(key, new KeyValuePair<string, KeyValuePair<int, object>>(value, new KeyValuePair<int, object>(-1, "")));

                }

            }
            return partialTree;
        }
        
        /// <summary>
        /// returns a partial  dependency tree
        /// </summary>
        /// <param name="sentence">sentence words</param>
        /// <param name="verbDicPath">path to the verb dictionary</param>
        /// <returns></returns>
        public static Dictionary<int, KeyValuePair<string, KeyValuePair<int, object>>> MakePartialTree(string[] sentence, string dicPath)
        {
            ResetDicPath(dicPath);
            var dic = AnalyzeSentenceConsiderCompoundVerbs(sentence);
            var partialTree = new Dictionary<int, KeyValuePair<string, KeyValuePair<int, object>>>();
            foreach (int key in dic.Keys)
            {
                string value = dic[key].Key;
                if (dic[key].Value is VerbInflection)
                {
                    partialTree.Add(key, new KeyValuePair<string, KeyValuePair<int, object>>(value, new KeyValuePair<int, object>(-1, dic[key].Value)));

                }
				else if (dic[key].Value is MostamarSaz)
				{
					var newValue = (MostamarSaz)dic[key].Value;
					partialTree.Add(key, new KeyValuePair<string, KeyValuePair<int, object>>(value, new KeyValuePair<int, object>(newValue.Head,newValue)));

				}
                else if (dic[key].Value is KeyValuePair<string, int>)
                {
                    var newValue = (KeyValuePair<string, int>)dic[key].Value;
                    if (key > 0 && dic[key - 1].Value is KeyValuePair<string, int>)
                    {
                        var prevValue = (KeyValuePair<string, int>)dic[key - 1].Value;
                        if (prevValue.Key == "VERBAL-PREPOSIOTION")
                            partialTree.Add(key,
                                            new KeyValuePair<string, KeyValuePair<int, object>>(value,
                                                                                                new KeyValuePair
                                                                                                    <int, object>(
                                                                                                    key - 1,
                                                                                                    "POSDEP")));
                        else
                        {

                            partialTree.Add(key,
                                            new KeyValuePair<string, KeyValuePair<int, object>>(value,
                                                                                                new KeyValuePair
                                                                                                    <int, object>(
                                                                                                    newValue.Value - 1,
                                                                                                    newValue.Key)));
                        }

                    }
                    else
                    {
                        partialTree.Add(key, new KeyValuePair<string, KeyValuePair<int, object>>(value, new KeyValuePair<int, object>(newValue.Value, newValue.Key)));
                    }
                }
                else
                {
                    partialTree.Add(key, new KeyValuePair<string, KeyValuePair<int, object>>(value, new KeyValuePair<int, object>(-1, "")));

                }

            }
            return partialTree;
        }

        /// <summary>
        /// Get a very initial data about verbal information of each token in the sentence
        /// </summary>
        /// <param name="sentence">an array of tokens in the sentence</param>
        /// <param name="posSentence">an array of POS tags of tokens in the sentence</param>
        /// <param name="verbDicPath">path of the verb dictionary file</param>
        /// <returns></returns>
        private static Dictionary<int, List<int>> GetGoodResult(string[] sentence, string[] posSentence)
        {
            var inflectionList = GetVerbParts(sentence, posSentence);

            var stateList = new Dictionary<int, List<int>>();
            stateList.Add(-1, new List<int>());
            stateList[-1].Add(0);
            string tempPishvand = "";
            for (int i = 0; i < inflectionList.Count; i++)
            {
                stateList.Add(i, new List<int>());

                var valuePair = inflectionList[i];

                if (valuePair == null)
                {
                    if (stateList[i - 1].Contains(6))
                    {
                        if (!stateList[i - 1].Contains(38))
                            stateList[i - 1].Add(38);
                        stateList[i - 1].Remove(6);
                    }
                    if (stateList[i - 1].Contains(-1))
                    {
                        if (!stateList[i - 1].Contains(48))
                            stateList[i - 1].Add(48);
                        stateList[i - 1].Remove(-1);
                    }
                    if (stateList[i - 1].Contains(-3))
                    {
                        if (!stateList[i - 1].Contains(52))
                            stateList[i - 1].Add(52);
                        stateList[i - 1].Remove(-3);
                    }
                    if (stateList[i - 1].Contains(8))
                    {
                        if (!stateList[i - 1].Contains(35))
                            stateList[i - 1].Add(35);
                        stateList[i - 1].Remove(8);
                    }
                    if (stateList[i - 1].Contains(5))
                    {
                        if (!stateList[i - 1].Contains(35))
                            stateList[i - 1].Add(35);
                        stateList[i - 1].Remove(5);
                    }
                    if (stateList[i - 1].Contains(7))
                    {
                        if (!stateList[i - 1].Contains(40))
                            stateList[i - 1].Add(40);
                        stateList[i - 1].Remove(7);
                    }
                    if (stateList[i - 1].Contains(9))
                    {
                        if (!stateList[i - 1].Contains(45))
                            stateList[i - 1].Add(45);
                        stateList[i - 1].Remove(9);
                    }
                    if (stateList[i - 1].Contains(1))
                    {
                        if (!stateList[i - 1].Contains(27))
                            stateList[i - 1].Add(27);
                        stateList[i - 1].Remove(1);
                    }
                    if (stateList[i - 1].Contains(4))
                    {
                        if (!stateList[i - 1].Contains(34))
                            stateList[i - 1].Add(34);
                        stateList[i - 1].Remove(4);
                    }
                    stateList[i].Add(0);
                }
                else
                {
                    int counter = 0;
					bool hasgozashtehsadegh=false;
					foreach (VerbInflection verbInflection in valuePair)
					{
						if (verbInflection.TenseForm==TenseFormationType.GOZASHTEH_SADEH){
							hasgozashtehsadegh=true;
							break;
						}
					}
                    foreach (VerbInflection verbInflection in valuePair)
                    {
                        counter++;

                        /*
                         * State -3: 
                         * State -2: 
                         * State -1: 
                         * State 0: initial form
                         * State 1: "Payeh Mafooli"
                         * State 2: "Xahaed"
                         * State 3: "Payeh Mafooli" + "Xahaed"
                         * State 4: "Payeh Mafooli"
                         * State 5: "Payeh Mafooli" + "Shodeh"
                         * State 6: "Payeh Mafooli"
                         * State 7: "Payeh Mafooli" + "Shodeh"
                         * State 8: "Payeh Mafooli" + "Shodeh"
                         * State 9: "Payeh Mafooli" + "Shodeh"
                         */

                        #region state 0

                        if (stateList[i - 1].Contains(0) || (stateList[i - 1].Count == 0 && stateList[i].Count == 0) || (stateList[i - 1].Count > 0 && stateList[i - 1][0] > 9))
                        {
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!stateList[i].Contains(10))
                                    stateList[i].Add(10);
                            }

                            if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!stateList[i].Contains(14))
                                    stateList[i].Add(14);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!stateList[i].Contains(15))
                                    stateList[i].Add(15);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!stateList[i].Contains(17))
                                    stateList[i].Add(17);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!stateList[i].Contains(18))
                                    stateList[i].Add(18);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!stateList[i].Contains(20))
                                    stateList[i].Add(20);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                            {
                                if (!stateList[i].Contains(46))
                                    stateList[i].Add(46);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                            {
                                if (!stateList[i].Contains(21))
                                    stateList[i].Add(21);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR)
                            {
                                if (!stateList[i].Contains(6))
                                    stateList[i].Add(6);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR)
                            {
                                if (!stateList[i].Contains(3) && !stateList[i].Contains(5) && !stateList[i].Contains(7))
                                {
                                    if (!stateList[i].Contains(-1))
                                        stateList[i].Add(-1);
                                }

                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد" && verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                            {
                                if (!stateList[i].Contains(47))
                                    stateList[i].Add(47);
                            }

                            if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد")
                            {
                                if (!stateList[i].Contains(16))
                                    stateList[i].Add(16);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد")
                            {
                                if (!stateList[i].Contains(19))
                                    stateList[i].Add(19);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "خواست")
                            {
                                if (!stateList[i].Contains(2))
                                    stateList[i].Add(2);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                               (verbInflection.VerbRoot.PastTenseRoot == "بایست" || verbInflection.VerbRoot.PresentTenseRoot == "توان"))
                            {
                                if (!stateList[i].Contains(53))
                                    stateList[i].Add(53);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                verbInflection.VerbRoot.Type == VerbType.AYANDEH_PISHVANDI)
                            {
                                if (!stateList[i].Contains(-2))
                                    stateList[i].Add(-2);
                                tempPishvand = verbInflection.VerbRoot.Prefix;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد")
                            {
                                if (!stateList[i].Contains(11))
                                    stateList[i].Add(11);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد")
                            {
                                if (!stateList[i].Contains(3) && !stateList[i].Contains(5) && !stateList[i].Contains(7))
                                {
                                    if (!stateList[i].Contains(12))
                                        stateList[i].Add(12);
                                }
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد")
                            {
                                if (!stateList[i].Contains(13))
                                    stateList[i].Add(13);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(1))
                                    stateList[i].Add(1);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد")
                            {
                                if (!stateList[i].Contains(9))
                                    stateList[i].Add(9);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                verbInflection.Positivity == TensePositivity.NEGATIVE)
                            {
                                if (!stateList[i].Contains(4))
                                    stateList[i].Add(4);
                            }
                        }

                        #endregion


                        #region state 1

                        if (stateList[i - 1].Contains(1))
                        {
                            bool find1 = false;
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(22))
                                    stateList[i].Add(22);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(23))
                                    stateList[i].Add(23);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(24))
                                    stateList[i].Add(24);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(25))
                                    stateList[i].Add(25);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(26))
                                    stateList[i].Add(26);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(29))
                                    stateList[i].Add(29);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(30))
                                    stateList[i].Add(30);
                                find1 = true;
                            }
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.Positivity == TensePositivity.POSITIVE &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(27))
                                    stateList[i].Add(27);
                                find1 = true;
                            }
                            if (verbInflection.VerbRoot.PresentTenseRoot == "هست" &&
                                verbInflection.Positivity == TensePositivity.NEGATIVE &&
                                verbInflection.ZamirPeyvasteh == AttachedPronounType.AttachedPronoun_NONE &&
                                verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(27))
                                    stateList[i].Add(27);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(5))
                                    stateList[i].Add(5);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH && verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(-3))
                                    stateList[i].Add(-3);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                              verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                              verbInflection.VerbRoot.Type == VerbType.SADEH && verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(51))
                                    stateList[i].Add(51);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(7))
                                    stateList[i].Add(7);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(28))
                                    stateList[i].Add(28);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "خواست" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(3))
                                    stateList[i].Add(3);
                                find1 = true;
                            }
                            if (!find1)
                            {
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    (verbInflection.VerbRoot.PastTenseRoot == "کرد" ||
                                     verbInflection.VerbRoot.PastTenseRoot == "گشت" ||
                                     verbInflection.VerbRoot.PastTenseRoot == "نمود" ||
                                     verbInflection.VerbRoot.PastTenseRoot == "فرمود" ||
                                     verbInflection.VerbRoot.PastTenseRoot == "ساخت") &&
                                    verbInflection.VerbRoot.Type == VerbType.SADEH)
                                {
                                    if (!stateList[i].Contains(17))
                                        stateList[i].Add(17);
                                    stateList[i - 1].Remove(1);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(0);
                                }
                                else if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                         (verbInflection.VerbRoot.PastTenseRoot == "کرد" ||
                                          verbInflection.VerbRoot.PastTenseRoot == "گشت" ||
                                          verbInflection.VerbRoot.PastTenseRoot == "نمود" ||
                                         verbInflection.VerbRoot.PastTenseRoot == "فرمود" || verbInflection.VerbRoot.PastTenseRoot == "ساخت") &&
                                         verbInflection.VerbRoot.Type == VerbType.SADEH)
                                {
                                    if (!stateList[i].Contains(14))
                                        stateList[i].Add(14);
                                    stateList[i - 1].Remove(1);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(0);
                                }
                                else if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                         (verbInflection.VerbRoot.PastTenseRoot == "کرد" ||
                                          verbInflection.VerbRoot.PastTenseRoot == "گشت" ||
                                          verbInflection.VerbRoot.PastTenseRoot == "نمود" ||
                                          verbInflection.VerbRoot.PastTenseRoot == "فرمود" || verbInflection.VerbRoot.PastTenseRoot == "ساخت") &&
                                         verbInflection.VerbRoot.Type == VerbType.SADEH)
                                {
                                    if (!stateList[i].Contains(10))
                                        stateList[i].Add(10);
                                    stateList[i - 1].Remove(1);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(0);
                                }
                                else if (counter == valuePair.Count)
                                {
                                    if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد")
                                    {
                                        if (!(verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                              verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                              verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                              verbInflection.Positivity == TensePositivity.POSITIVE))
                                        {
                                            if (!stateList[i].Contains(10))
                                                stateList[i].Add(10);
                                            stateList[i - 1].Remove(1);
                                            stateList[i - 1].Add(27);
                                        }
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد")
                                    {
                                        if (!(verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                              verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                              verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                              verbInflection.Positivity == TensePositivity.POSITIVE))
                                        {
                                            if (!stateList[i].Contains(14))
                                                stateList[i].Add(14);
                                            stateList[i - 1].Remove(1);
                                            stateList[i - 1].Add(27);
                                        }
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد")
                                    {
                                        if (!stateList[i].Contains(15))
                                            stateList[i].Add(15);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد")
                                    {
                                        if (!stateList[i].Contains(17))
                                            stateList[i].Add(17);
                                        if (
                                            !(verbInflection.VerbRoot.PastTenseRoot == "کرد" &&
                                              verbInflection.VerbRoot.Type == VerbType.SADEH))
                                        {

                                            stateList[i - 1].Remove(1);
                                            stateList[i - 1].Add(27);
                                        }
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد")
                                    {
                                        if (!stateList[i].Contains(18))
                                            stateList[i].Add(18);
                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد")
                                    {
                                        if (!stateList[i].Contains(20))
                                            stateList[i].Add(20);
                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm ==
                                        TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                        verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                    {
                                        if (!stateList[i].Contains(21))
                                            stateList[i].Add(21);
                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm ==
                                        TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                        verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                        verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                    {
                                        if (!stateList[i].Contains(6))
                                            stateList[i].Add(6);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }

                                    if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                        verbInflection.VerbRoot.PastTenseRoot == "شد")
                                    {
                                        if (!stateList[i].Contains(16))
                                            stateList[i].Add(16);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                        verbInflection.VerbRoot.PastTenseRoot == "شد")
                                    {
                                        if (!stateList[i].Contains(19))
                                            stateList[i].Add(19);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                        verbInflection.VerbRoot.PastTenseRoot == "خواست")
                                    {
                                        if (!stateList[i].Contains(2))
                                            stateList[i].Add(2);
                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);

                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                        verbInflection.VerbRoot.PastTenseRoot == "شد")
                                    {
                                        if (!stateList[i].Contains(11))
                                            stateList[i].Add(11);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                        verbInflection.VerbRoot.PastTenseRoot == "شد")
                                    {
                                        if (!stateList[i].Contains(13))
                                            stateList[i].Add(13);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);

                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                        verbInflection.Positivity == TensePositivity.POSITIVE)
                                    {
                                        if (!stateList[i].Contains(1))
                                            stateList[i].Add(1);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                        verbInflection.VerbRoot.PastTenseRoot == "شد")
                                    {
                                        if (!stateList[i].Contains(9))
                                            stateList[i].Add(9);
                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);

                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                        verbInflection.Positivity == TensePositivity.NEGATIVE)
                                    {
                                        if (!stateList[i].Contains(4))
                                            stateList[i].Add(4);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                }
                            }
                            else
                            {
                                stateList[i - 1].Remove(1);
                                stateList[i - 1].Remove(4);
                            }
                        }

                        #endregion


                        #region state 5

                        if (stateList[i - 1].Contains(5))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(35))
                                    stateList[i].Add(35);
                                stateList[i - 1].Remove(5);
                                stateList[i - 1].Remove(9);
                            }
                            if (verbInflection.VerbRoot.PresentTenseRoot == "هست" &&
                                verbInflection.Positivity == TensePositivity.NEGATIVE &&
                                verbInflection.ZamirPeyvasteh == AttachedPronounType.AttachedPronoun_NONE &&
                                verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(35))
                                    stateList[i].Add(35);
                                stateList[i - 1].Remove(5);
                                stateList[i - 1].Remove(9);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(36))
                                    stateList[i].Add(36);
                                stateList[i - 1].Remove(5);
                                stateList[i - 1].Remove(9);
                            }
							if ((verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH ||verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI)&&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(37))
                                    stateList[i].Add(37);
                                stateList[i - 1].Remove(5);
                                stateList[i - 1].Remove(9);
                            }
                        }

                        #endregion

                        #region state 2

                        if (stateList[i - 1].Contains(2))
                        {
                            if (verbInflection.IsPayehFelMasdari() && verbInflection.VerbRoot.PastTenseRoot != "شد" &&
							    verbInflection.VerbRoot.Type == VerbType.SADEH && verbInflection.Positivity==TensePositivity.POSITIVE)//ToCheck
                            {
                                stateList[i].Clear();
                                if (!stateList[i].Contains(31))
                                    stateList[i].Add(31);
                                stateList[i - 1].Clear();
                            }
                            if (verbInflection.IsPayehFelMasdari() && verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                stateList[i].Clear();
                                if (!stateList[i].Contains(32))
                                    stateList[i].Add(32);
                                stateList[i - 1].Clear();
                            }
                        }

                        #endregion

                        #region state -2

                        if (stateList[i - 1].Contains(-2))
                        {
                            if (verbInflection.IsPayehFelMasdari() && verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH && VerbList.VerbPishvandiDic[tempPishvand].Contains(verbInflection.VerbRoot.PastTenseRoot + "|" + verbInflection.VerbRoot.PresentTenseRoot))
                            {
                                stateList[i].Clear();
                                if (!stateList[i].Contains(31))
                                    stateList[i].Add(31);
                                stateList[i - 1].Clear();
                            }
                            else if (verbInflection.IsPayehFelMasdari() && verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH && VerbList.VerbPishvandiDic[tempPishvand].Contains(verbInflection.VerbRoot.PastTenseRoot + "|" + verbInflection.VerbRoot.PresentTenseRoot))
                            {
                                stateList[i].Clear();
                                if (!stateList[i].Contains(32))
                                    stateList[i].Add(32);
                                stateList[i - 1].Clear();
                            }
                            else
                            {
                                stateList[i - 1].Clear();
                                stateList[i].Clear();
                                stateList[i - 1].Add(0);
								if (hasgozashtehsadegh)
									stateList[i - 1].Add(-2);
                                stateList[i].Add(0);
                            }
                        }

                        #endregion

                        #region state -3

                        if (stateList[i - 1].Contains(-3))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                                               verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                                               verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(52))
                                    stateList[i].Add(52);
                                stateList[i - 1].Remove(-3);
                            }
                        }
                        #endregion

                        #region state 3

                        if (stateList[i - 1].Contains(3))
                        {
                            if (verbInflection.IsPayehFelMasdari() && verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                stateList[i].Clear();
                                if (!stateList[i].Contains(33))
                                    stateList[i].Add(33);
                                stateList[i - 1].Clear();
                            }
                        }

                        #endregion

                        #region state 4

                        if (stateList[i - 1].Contains(4))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(34))
                                    stateList[i].Add(34);
                                stateList[i - 1].Remove(4);
                            }
                            if (verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(41))
                                    stateList[i].Add(41);
                                stateList[i - 1].Remove(4);
                            }
                            if (verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(42))
                                    stateList[i].Add(42);
                                stateList[i - 1].Remove(4);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH && verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(-3))
                                    stateList[i].Add(-3);
                                stateList[i - 1].Remove(4);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                             verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                             verbInflection.VerbRoot.Type == VerbType.SADEH && verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(51))
                                    stateList[i].Add(51);
                                stateList[i - 1].Remove(4);
                            }
                            if (verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                if (!stateList[i].Contains(8))
                                    stateList[i].Add(8);
                                stateList[i - 1].Remove(4);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!(verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                      verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                      verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                      verbInflection.Positivity == TensePositivity.POSITIVE))
                                {
                                    if (!stateList[i].Contains(10))
                                        stateList[i].Add(10);

                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                            }

                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                (verbInflection.VerbRoot.PastTenseRoot == "کرد" ||
                                 verbInflection.VerbRoot.PastTenseRoot == "گشت" ||
                                 verbInflection.VerbRoot.PastTenseRoot == "نمود" ||
                              verbInflection.VerbRoot.PastTenseRoot == "فرمود" || verbInflection.VerbRoot.PastTenseRoot == "ساخت") &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(17))
                                    stateList[i].Add(17);
                                stateList[i - 1].Remove(1);
                                stateList[i - 1].Remove(4);
                                stateList[i - 1].Add(0);
                            }
                            else if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                     (verbInflection.VerbRoot.PastTenseRoot == "کرد" ||
                                      verbInflection.VerbRoot.PastTenseRoot == "گشت" ||
                                      verbInflection.VerbRoot.PastTenseRoot == "نمود" ||
                                    verbInflection.VerbRoot.PastTenseRoot == "فرمود" || verbInflection.VerbRoot.PastTenseRoot == "ساخت") &&
                                     verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(14))
                                    stateList[i].Add(14);
                                stateList[i - 1].Remove(1);
                                stateList[i - 1].Remove(4);
                                stateList[i - 1].Add(0);
                            }
                            else if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                     (verbInflection.VerbRoot.PastTenseRoot == "کرد" ||
                                      verbInflection.VerbRoot.PastTenseRoot == "گشت" ||
                                      verbInflection.VerbRoot.PastTenseRoot == "نمود" ||
                                   verbInflection.VerbRoot.PastTenseRoot == "فرمود" || verbInflection.VerbRoot.PastTenseRoot == "ساخت") &&
                                     verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(10))
                                    stateList[i].Add(10);
                                stateList[i - 1].Remove(1);
                                stateList[i - 1].Remove(4);
                                stateList[i - 1].Add(0);
                            }
                            else if (stateList[i - 1].Contains(4))
                            {
                                if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(15))
                                        stateList[i].Add(15);

                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(14))
                                        stateList[i].Add(14);

                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(17))
                                        stateList[i].Add(17);
                                    if (
                                        !(verbInflection.VerbRoot.PastTenseRoot == "کرد" &&
                                          verbInflection.VerbRoot.Type == VerbType.SADEH))
                                    {
                                        stateList[i - 1].Remove(4);
                                        stateList[i - 1].Add(34);

                                    }
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(18))
                                        stateList[i].Add(18);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(20))
                                        stateList[i].Add(20);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }

                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(21))
                                        stateList[i].Add(21);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(6))
                                        stateList[i].Add(6);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }

                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(47))
                                        stateList[i].Add(47);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(16))
                                        stateList[i].Add(16);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(19))
                                        stateList[i].Add(19);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "خواست")
                                {
                                    if (!stateList[i].Contains(2))
                                        stateList[i].Add(2);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(11))
                                        stateList[i].Add(11);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }

                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(13))
                                        stateList[i].Add(13);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Positivity == TensePositivity.POSITIVE)
                                {
                                    if (!stateList[i].Contains(1))
                                        stateList[i].Add(1);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(9))
                                        stateList[i].Add(9);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Positivity == TensePositivity.NEGATIVE)
                                {
                                    if (!stateList[i].Contains(4))
                                        stateList[i].Add(4);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }

                            }
                        }

                        #endregion

                        #region state 6

                        if (stateList[i - 1].Contains(6))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(39))
                                    stateList[i].Add(39);
                                stateList[i - 1].Remove(6);
                            }
                        }

                        #endregion

                        #region state 7

                        if (stateList[i - 1].Contains(7))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(40))
                                    stateList[i].Add(40);
                                stateList[i - 1].Remove(7);
                            }

                            if (verbInflection.VerbRoot.PresentTenseRoot == "هست" &&
                                verbInflection.Positivity == TensePositivity.NEGATIVE &&
                                verbInflection.ZamirPeyvasteh == AttachedPronounType.AttachedPronoun_NONE &&
                                verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(40))
                                    stateList[i].Add(40);
                                stateList[i - 1].Remove(7);
                            }
                        }

                        #endregion

                        #region state 8

                        if (stateList[i - 1].Contains(8))
                        {
                            if (verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(43))
                                    stateList[i].Add(43);
                                stateList[i - 1].Remove(8);
                            }
                            if (verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(44))
                                    stateList[i].Add(44);
                                stateList[i - 1].Remove(8);
                            }
                        }

                        #endregion

                        #region state 9

                        if (stateList[i - 1].Contains(9))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(45))
                                    stateList[i].Add(45);
                                stateList[i - 1].Remove(9);
                            }
                            else if (verbInflection.VerbRoot.PresentTenseRoot == "باش" &&
                                verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(49))
                                    stateList[i].Add(49);
                                stateList[i - 1].Remove(9);
                            }
                            else if (verbInflection.VerbRoot.PresentTenseRoot == "باش" &&
                           verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                           verbInflection.VerbRoot.Type == VerbType.SADEH &&
                           verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(50))
                                    stateList[i].Add(50);
                                stateList[i - 1].Remove(9);
                            }
                            else
                            {

                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!(verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                          verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                          verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                          verbInflection.Positivity == TensePositivity.POSITIVE))
                                    {
                                        if (!stateList[i].Contains(10))
                                            stateList[i].Add(10);
                                        stateList[i - 1].Remove(9);
                                        stateList[i - 1].Add(45);
                                    }
                                }

                                if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(14))
                                        stateList[i].Add(14);
                                    if (stateList[i - 1].Contains(9))
                                    {
                                        stateList[i - 1].Remove(9);
                                        stateList[i - 1].Add(45);
                                    }

                                }
                                if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(15))
                                        stateList[i].Add(15);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(17))
                                        stateList[i].Add(17);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(18))
                                        stateList[i].Add(18);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(20))
                                        stateList[i].Add(20);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(46))
                                        stateList[i].Add(46);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(21))
                                        stateList[i].Add(21);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }

                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                    verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                    verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(-1))
                                        stateList[i].Add(-1);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }

                                if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(16))
                                        stateList[i].Add(16);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(19))
                                        stateList[i].Add(19);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "خواست")
                                {
                                    if (!stateList[i].Contains(2))
                                        stateList[i].Add(2);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(11))
                                        stateList[i].Add(11);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }

                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(13))
                                        stateList[i].Add(13);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Positivity == TensePositivity.POSITIVE)
                                {
                                    if (!stateList[i].Contains(1))
                                        stateList[i].Add(1);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(9))
                                        stateList[i].Add(9);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Positivity == TensePositivity.NEGATIVE)
                                {
                                    if (!stateList[i].Contains(4))
                                        stateList[i].Add(4);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (stateList[i - 1].Contains(9))
                                {
                                    if (!stateList[i - 1].Contains(45))
                                        stateList[i - 1].Add(45);
                                    stateList[i - 1].Remove(9);
                                }
                            }
                        }

                        #endregion

                        #region state -1

                        if (stateList[i - 1].Contains(-1))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(48))
                                    stateList[i].Add(48);
                                stateList[i - 1].Remove(-1);
                            }
                            else
                            {
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!(verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                          verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                          verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                          verbInflection.Positivity == TensePositivity.POSITIVE))
                                    {
                                        if (!stateList[i].Contains(10))
                                            stateList[i].Add(10);
                                        stateList[i - 1].Remove(-1);
                                        stateList[i - 1].Add(48);
                                    }
                                }

                                if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(14))
                                        stateList[i].Add(14);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(15))
                                        stateList[i].Add(15);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);

                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(17))
                                        stateList[i].Add(17);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(18))
                                        stateList[i].Add(18);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(20))
                                        stateList[i].Add(20);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(46))
                                        stateList[i].Add(46);

                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(21))
                                        stateList[i].Add(21);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(6))
                                        stateList[i].Add(6);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                    verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                    verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(-1))
                                        stateList[i].Add(-1);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);

                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(47))
                                        stateList[i].Add(47);

                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(16))
                                        stateList[i].Add(16);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(19))
                                        stateList[i].Add(19);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "خواست")
                                {
                                    if (!stateList[i].Contains(2))
                                        stateList[i].Add(2);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(11))
                                        stateList[i].Add(11);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(12))
                                        stateList[i].Add(12);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(13))
                                        stateList[i].Add(13);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Positivity == TensePositivity.POSITIVE)
                                {
                                    if (!stateList[i].Contains(1))
                                        stateList[i].Add(1);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Positivity == TensePositivity.NEGATIVE)
                                {
                                    if (!stateList[i].Contains(4))
                                        stateList[i].Add(4);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                stateList[i - 1].Remove(-1);
                            }
                        }

                        #endregion


                    }
                    if (stateList[i].Count == 0)
                    {
                        stateList[i].Add(0);
                    }
                    if (!stateList[i].Contains(-2))
                        tempPishvand = "";
                }
            }
            return stateList;
        }

        /// <summary>
        /// Get a very initial data about verbal information of each token in the sentence
        /// </summary>
        /// <param name="sentence">an array of tokens in the sentence</param>
        /// <param name="verbDicPath">path of the verb dictionary file</param>
        /// <returns></returns>
        private static Dictionary<int, List<int>> GetGoodResult(string[] sentence)
        {
            var inflectionList = GetVerbParts(sentence);

            var stateList = new Dictionary<int, List<int>>();
            stateList.Add(-1, new List<int>());
            stateList[-1].Add(0);
            string tempPishvand = "";
            for (int i = 0; i < inflectionList.Count; i++)
            {
                stateList.Add(i, new List<int>());

                var valuePair = inflectionList[i];

                if (valuePair == null)
                {
                    if (stateList[i - 1].Contains(6))
                    {
                        if (!stateList[i - 1].Contains(38))
                            stateList[i - 1].Add(38);
                        stateList[i - 1].Remove(6);
                    }
                    if (stateList[i - 1].Contains(-1))
                    {
                        if (!stateList[i - 1].Contains(48))
                            stateList[i - 1].Add(48);
                        stateList[i - 1].Remove(-1);
                    }
                    if (stateList[i - 1].Contains(-3))
                    {
                        if (!stateList[i - 1].Contains(52))
                            stateList[i - 1].Add(52);
                        stateList[i - 1].Remove(-3);
                    }
                    if (stateList[i - 1].Contains(8))
                    {
                        if (!stateList[i - 1].Contains(35))
                            stateList[i - 1].Add(35);
                        stateList[i - 1].Remove(8);
                    }
                    if (stateList[i - 1].Contains(5))
                    {
                        if (!stateList[i - 1].Contains(35))
                            stateList[i - 1].Add(35);
                        stateList[i - 1].Remove(5);
                    }
                    if (stateList[i - 1].Contains(7))
                    {
                        if (!stateList[i - 1].Contains(40))
                            stateList[i - 1].Add(40);
                        stateList[i - 1].Remove(7);
                    }
                    if (stateList[i - 1].Contains(9))
                    {
                        if (!stateList[i - 1].Contains(45))
                            stateList[i - 1].Add(45);
                        stateList[i - 1].Remove(9);
                    }
                    if (stateList[i - 1].Contains(1))
                    {
                        if (!stateList[i - 1].Contains(27))
                            stateList[i - 1].Add(27);
                        stateList[i - 1].Remove(1);
                    }
                    if (stateList[i - 1].Contains(4))
                    {
                        if (!stateList[i - 1].Contains(34))
                            stateList[i - 1].Add(34);
                        stateList[i - 1].Remove(4);
                    }
                    stateList[i].Add(0);
                }
                else
                {
                    int counter = 0;
					bool hasGozashtehSadeh=false;
					foreach (VerbInflection verbInflection in valuePair)
					{
						if (verbInflection.TenseForm==TenseFormationType.GOZASHTEH_SADEH){
							hasGozashtehSadeh=true;
							break;
						}
					}
                    foreach (VerbInflection verbInflection in valuePair)
                    {
                        counter++;

                        /*
                         * State -3: 
                         * State -2: 
                         * State -1: 
                         * State 0: initial form
                         * State 1: "Payeh Mafooli"
                         * State 2: "Xahaed"
                         * State 3: "Payeh Mafooli" + "Xahaed"
                         * State 4: "Payeh Mafooli"
                         * State 5: "Payeh Mafooli" + "Shodeh"
                         * State 6: "Payeh Mafooli"
                         * State 7: "Payeh Mafooli" + "Shodeh"
                         * State 8: "Payeh Mafooli" + "Shodeh"
                         * State 9: "Payeh Mafooli" + "Shodeh"
                         */

                        #region state 0

                        if (stateList[i - 1].Contains(0) || (stateList[i - 1].Count == 0 && stateList[i].Count == 0) ||
                            (stateList[i - 1].Count > 0 && stateList[i - 1][0] > 9))
                        {
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!stateList[i].Contains(10))
                                    stateList[i].Add(10);
                            }

                            if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!stateList[i].Contains(14))
                                    stateList[i].Add(14);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!stateList[i].Contains(15))
                                    stateList[i].Add(15);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!stateList[i].Contains(17))
                                    stateList[i].Add(17);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!stateList[i].Contains(18))
                                    stateList[i].Add(18);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!stateList[i].Contains(20))
                                    stateList[i].Add(20);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                            {
                                if (!stateList[i].Contains(46))
                                    stateList[i].Add(46);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                            {
                                if (!stateList[i].Contains(21))
                                    stateList[i].Add(21);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR)
                            {
                                if (!stateList[i].Contains(6))
                                    stateList[i].Add(6);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR)
                            {
                                if (!stateList[i].Contains(3) && !stateList[i].Contains(5) && !stateList[i].Contains(7))
                                {
                                    if (!stateList[i].Contains(-1))
                                        stateList[i].Add(-1);
                                }

                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                            {
                                if (!stateList[i].Contains(47))
                                    stateList[i].Add(47);
                            }

                            if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد")
                            {
                                if (!stateList[i].Contains(16))
                                    stateList[i].Add(16);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد")
                            {
                                if (!stateList[i].Contains(19))
                                    stateList[i].Add(19);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "خواست")
                            {
                                if (!stateList[i].Contains(2))
                                    stateList[i].Add(2);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                verbInflection.VerbRoot.Type == VerbType.AYANDEH_PISHVANDI)
                            {
                                if (!stateList[i].Contains(-2))
                                    stateList[i].Add(-2);
                                tempPishvand = verbInflection.VerbRoot.Prefix;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد")
                            {
                                if (!stateList[i].Contains(11))
                                    stateList[i].Add(11);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد")
                            {
                                if (!stateList[i].Contains(3) && !stateList[i].Contains(5) && !stateList[i].Contains(7))
                                {
                                    if (!stateList[i].Contains(12))
                                        stateList[i].Add(12);
                                }
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد")
                            {
                                if (!stateList[i].Contains(13))
                                    stateList[i].Add(13);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(1))
                                    stateList[i].Add(1);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد")
                            {
                                if (!stateList[i].Contains(9))
                                    stateList[i].Add(9);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                verbInflection.Positivity == TensePositivity.NEGATIVE)
                            {
                                if (!stateList[i].Contains(4))
                                    stateList[i].Add(4);
                            }
                        }

                        #endregion

                        #region state 1

                        if (stateList[i - 1].Contains(1))
                        {
                            bool find1 = false;
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(22))
                                    stateList[i].Add(22);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(23))
                                    stateList[i].Add(23);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(24))
                                    stateList[i].Add(24);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(25))
                                    stateList[i].Add(25);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(26))
                                    stateList[i].Add(26);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(29))
                                    stateList[i].Add(29);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(30))
                                    stateList[i].Add(30);
                                find1 = true;
                            }
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.Positivity == TensePositivity.POSITIVE &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(27))
                                    stateList[i].Add(27);
                                find1 = true;
                            }
                            if (verbInflection.VerbRoot.PresentTenseRoot == "هست" &&
                                verbInflection.Positivity == TensePositivity.NEGATIVE &&
                                verbInflection.ZamirPeyvasteh == AttachedPronounType.AttachedPronoun_NONE &&
                                verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(27))
                                    stateList[i].Add(27);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(5))
                                    stateList[i].Add(5);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(-3))
                                    stateList[i].Add(-3);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(51))
                                    stateList[i].Add(51);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(7))
                                    stateList[i].Add(7);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(28))
                                    stateList[i].Add(28);
                                find1 = true;
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "خواست" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(3))
                                    stateList[i].Add(3);
                                find1 = true;
                            }
                            if (!find1)
                            {
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    (verbInflection.VerbRoot.PastTenseRoot == "کرد" ||
                                     verbInflection.VerbRoot.PastTenseRoot == "گشت" ||
                                     verbInflection.VerbRoot.PastTenseRoot == "نمود" ||
                                     verbInflection.VerbRoot.PastTenseRoot == "فرمود" ||
                                     verbInflection.VerbRoot.PastTenseRoot == "ساخت") &&
                                    verbInflection.VerbRoot.Type == VerbType.SADEH)
                                {
                                    if (!stateList[i].Contains(17))
                                        stateList[i].Add(17);
                                    stateList[i - 1].Remove(1);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(0);
                                }
                                else if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                         (verbInflection.VerbRoot.PastTenseRoot == "کرد" ||
                                          verbInflection.VerbRoot.PastTenseRoot == "گشت" ||
                                          verbInflection.VerbRoot.PastTenseRoot == "نمود" ||
                                          verbInflection.VerbRoot.PastTenseRoot == "فرمود" ||
                                          verbInflection.VerbRoot.PastTenseRoot == "ساخت") &&
                                         verbInflection.VerbRoot.Type == VerbType.SADEH)
                                {
                                    if (!stateList[i].Contains(14))
                                        stateList[i].Add(14);
                                    stateList[i - 1].Remove(1);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(0);
                                }
                                else if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                         (verbInflection.VerbRoot.PastTenseRoot == "کرد" ||
                                          verbInflection.VerbRoot.PastTenseRoot == "گشت" ||
                                          verbInflection.VerbRoot.PastTenseRoot == "نمود" ||
                                          verbInflection.VerbRoot.PastTenseRoot == "فرمود" ||
                                          verbInflection.VerbRoot.PastTenseRoot == "ساخت") &&
                                         verbInflection.VerbRoot.Type == VerbType.SADEH)
                                {
                                    if (!stateList[i].Contains(10))
                                        stateList[i].Add(10);
                                    stateList[i - 1].Remove(1);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(0);
                                }
                                else if (counter == valuePair.Count)
                                {
                                    if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد")
                                    {
                                        if (!(verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                              verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                              verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                              verbInflection.Positivity == TensePositivity.POSITIVE))
                                        {
                                            if (!stateList[i].Contains(10))
                                                stateList[i].Add(10);
                                            stateList[i - 1].Remove(1);
                                            stateList[i - 1].Add(27);
                                        }
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد")
                                    {
                                        if (!(verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                              verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                              verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                              verbInflection.Positivity == TensePositivity.POSITIVE))
                                        {
                                            if (!stateList[i].Contains(14))
                                                stateList[i].Add(14);
                                            stateList[i - 1].Remove(1);
                                            stateList[i - 1].Add(27);
                                        }
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد")
                                    {
                                        if (!stateList[i].Contains(15))
                                            stateList[i].Add(15);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد")
                                    {
                                        if (!stateList[i].Contains(17))
                                            stateList[i].Add(17);
                                        if (
                                            !(verbInflection.VerbRoot.PastTenseRoot == "کرد" &&
                                              verbInflection.VerbRoot.Type == VerbType.SADEH))
                                        {

                                            stateList[i - 1].Remove(1);
                                            stateList[i - 1].Add(27);
                                        }
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد")
                                    {
                                        if (!stateList[i].Contains(18))
                                            stateList[i].Add(18);
                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد")
                                    {
                                        if (!stateList[i].Contains(20))
                                            stateList[i].Add(20);
                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm ==
                                        TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                        verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                    {
                                        if (!stateList[i].Contains(21))
                                            stateList[i].Add(21);
                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm ==
                                        TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                        verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                        verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                    {
                                        if (!stateList[i].Contains(6))
                                            stateList[i].Add(6);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }

                                    if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                        verbInflection.VerbRoot.PastTenseRoot == "شد")
                                    {
                                        if (!stateList[i].Contains(16))
                                            stateList[i].Add(16);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                        verbInflection.VerbRoot.PastTenseRoot == "شد")
                                    {
                                        if (!stateList[i].Contains(19))
                                            stateList[i].Add(19);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                        verbInflection.VerbRoot.PastTenseRoot == "خواست")
                                    {
                                        if (!stateList[i].Contains(2))
                                            stateList[i].Add(2);
                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);

                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                        verbInflection.VerbRoot.PastTenseRoot == "شد")
                                    {
                                        if (!stateList[i].Contains(11))
                                            stateList[i].Add(11);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                        verbInflection.VerbRoot.PastTenseRoot == "شد")
                                    {
                                        if (!stateList[i].Contains(13))
                                            stateList[i].Add(13);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);

                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                        verbInflection.Positivity == TensePositivity.POSITIVE)
                                    {
                                        if (!stateList[i].Contains(1))
                                            stateList[i].Add(1);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                        verbInflection.VerbRoot.PastTenseRoot == "شد")
                                    {
                                        if (!stateList[i].Contains(9))
                                            stateList[i].Add(9);
                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);

                                    }
                                    if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                        verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                        verbInflection.Positivity == TensePositivity.NEGATIVE)
                                    {
                                        if (!stateList[i].Contains(4))
                                            stateList[i].Add(4);

                                        stateList[i - 1].Remove(1);
                                        stateList[i - 1].Add(27);
                                    }
                                }
                            }
                            else
                            {
                                stateList[i - 1].Remove(1);
                                stateList[i - 1].Remove(4);
                            }
                        }

                        #endregion

                        #region state 5

                        if (stateList[i - 1].Contains(5))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(35))
                                    stateList[i].Add(35);
                                stateList[i - 1].Remove(5);
                                stateList[i - 1].Remove(9);
                            }
                            if (verbInflection.VerbRoot.PresentTenseRoot == "هست" &&
                                verbInflection.Positivity == TensePositivity.NEGATIVE &&
                                verbInflection.ZamirPeyvasteh == AttachedPronounType.AttachedPronoun_NONE &&
                                verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(35))
                                    stateList[i].Add(35);
                                stateList[i - 1].Remove(5);
                                stateList[i - 1].Remove(9);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(36))
                                    stateList[i].Add(36);
                                stateList[i - 1].Remove(5);
                                stateList[i - 1].Remove(9);
                            }
							//ToCheck
							if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
							    verbInflection.VerbRoot.PastTenseRoot == "بود" &&
							    verbInflection.VerbRoot.Type == VerbType.SADEH &&
							    verbInflection.Positivity == TensePositivity.POSITIVE)
							{
								if (!stateList[i].Contains(52))
									stateList[i].Add(52);
								stateList[i - 1].Remove(5);
								stateList[i - 1].Remove(9);
							}
							if(verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
							    verbInflection.VerbRoot.PastTenseRoot == "بود" &&
							    verbInflection.VerbRoot.Type == VerbType.SADEH)
							   {
								if (!stateList[i].Contains(-3))
									stateList[i].Add(-3);
								stateList[i - 1].Remove(5);
								stateList[i - 1].Remove(9);
							}
							if ((verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH || verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI) &&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(37))
                                    stateList[i].Add(37);
                                stateList[i - 1].Remove(5);
                                stateList[i - 1].Remove(9);
                            }
                        }

                        #endregion

                        #region state 2

                        if (stateList[i - 1].Contains(2))
                        {
                            if (verbInflection.IsPayehFelMasdari() && verbInflection.VerbRoot.PastTenseRoot != "شد" &&
							    verbInflection.VerbRoot.Type == VerbType.SADEH && verbInflection.Positivity==TensePositivity.POSITIVE)
                            {
                                stateList[i].Clear();
                                if (!stateList[i].Contains(31))
                                    stateList[i].Add(31);
                                stateList[i - 1].Clear();
                            }
                            if (verbInflection.IsPayehFelMasdari() && verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                stateList[i].Clear();
                                if (!stateList[i].Contains(32))
                                    stateList[i].Add(32);
                                stateList[i - 1].Clear();
                            }
                        }

                        #endregion

                        #region state -2

                        if (stateList[i - 1].Contains(-2))
                        {
                            if (verbInflection.IsPayehFelMasdari() && verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                VerbList.VerbPishvandiDic[tempPishvand].Contains(verbInflection.VerbRoot.PastTenseRoot +
                                                                                 "|" +
                                                                                 verbInflection.VerbRoot.PresentTenseRoot))
                            {
                                stateList[i].Clear();
                                if (!stateList[i].Contains(31))
                                    stateList[i].Add(31);
                                stateList[i - 1].Clear();
                            }
                            else if (verbInflection.IsPayehFelMasdari() && verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                     verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                     VerbList.VerbPishvandiDic[tempPishvand].Contains(
                                         verbInflection.VerbRoot.PastTenseRoot + "|" +
                                         verbInflection.VerbRoot.PresentTenseRoot))
                            {
                                stateList[i].Clear();
                                if (!stateList[i].Contains(32))
                                    stateList[i].Add(32);
                                stateList[i - 1].Clear();
                            }
                            else
                            {
								//TO Check

                                stateList[i - 1].Clear();
                                stateList[i].Clear();
                                stateList[i - 1].Add(0);
								if (hasGozashtehSadeh)
									stateList[i - 1].Add(-2);
                                stateList[i].Add(0);
                            }
                        }

                        #endregion

                        #region state -3

                        if (stateList[i - 1].Contains(-3))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(52))
                                    stateList[i].Add(52);
                                stateList[i - 1].Remove(-3);
                            }
                        }

                        #endregion

                        #region state 3

                        if (stateList[i - 1].Contains(3))
                        {
                            if (verbInflection.IsPayehFelMasdari() && verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                stateList[i].Clear();
                                if (!stateList[i].Contains(33))
                                    stateList[i].Add(33);
                                stateList[i - 1].Clear();
                            }
                        }

                        #endregion

                        #region state 4

                        if (stateList[i - 1].Contains(4))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(34))
                                    stateList[i].Add(34);
                                stateList[i - 1].Remove(4);
                            }
                            if (verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(41))
                                    stateList[i].Add(41);
                                stateList[i - 1].Remove(4);
                            }
                            if (verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(42))
                                    stateList[i].Add(42);
                                stateList[i - 1].Remove(4);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(-3))
                                    stateList[i].Add(-3);
                                stateList[i - 1].Remove(4);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(51))
                                    stateList[i].Add(51);
                                stateList[i - 1].Remove(4);
                            }
                            if (verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                if (!stateList[i].Contains(8))
                                    stateList[i].Add(8);
                                stateList[i - 1].Remove(4);
                            }
                            if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                verbInflection.VerbRoot.PastTenseRoot != "شد")
                            {
                                if (!(verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                      verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                      verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                      verbInflection.Positivity == TensePositivity.POSITIVE))
                                {
                                    if (!stateList[i].Contains(10))
                                        stateList[i].Add(10);

                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                            }

                            if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                (verbInflection.VerbRoot.PastTenseRoot == "کرد" ||
                                 verbInflection.VerbRoot.PastTenseRoot == "گشت" ||
                                 verbInflection.VerbRoot.PastTenseRoot == "نمود" ||
                                 verbInflection.VerbRoot.PastTenseRoot == "فرمود" ||
                                 verbInflection.VerbRoot.PastTenseRoot == "ساخت") &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(17))
                                    stateList[i].Add(17);
                                stateList[i - 1].Remove(1);
                                stateList[i - 1].Remove(4);
                                stateList[i - 1].Add(0);
                            }
                            else if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                     (verbInflection.VerbRoot.PastTenseRoot == "کرد" ||
                                      verbInflection.VerbRoot.PastTenseRoot == "گشت" ||
                                      verbInflection.VerbRoot.PastTenseRoot == "نمود" ||
                                      verbInflection.VerbRoot.PastTenseRoot == "فرمود" ||
                                      verbInflection.VerbRoot.PastTenseRoot == "ساخت") &&
                                     verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(14))
                                    stateList[i].Add(14);
                                stateList[i - 1].Remove(1);
                                stateList[i - 1].Remove(4);
                                stateList[i - 1].Add(0);
                            }
                            else if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                     (verbInflection.VerbRoot.PastTenseRoot == "کرد" ||
                                      verbInflection.VerbRoot.PastTenseRoot == "گشت" ||
                                      verbInflection.VerbRoot.PastTenseRoot == "نمود" ||
                                      verbInflection.VerbRoot.PastTenseRoot == "فرمود" ||
                                      verbInflection.VerbRoot.PastTenseRoot == "ساخت") &&
                                     verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(10))
                                    stateList[i].Add(10);
                                stateList[i - 1].Remove(1);
                                stateList[i - 1].Remove(4);
                                stateList[i - 1].Add(0);
                            }
                            else if (stateList[i - 1].Contains(4))
                            {
                                if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(15))
                                        stateList[i].Add(15);

                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(14))
                                        stateList[i].Add(14);

                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(17))
                                        stateList[i].Add(17);
                                    if (
                                        !(verbInflection.VerbRoot.PastTenseRoot == "کرد" &&
                                          verbInflection.VerbRoot.Type == VerbType.SADEH))
                                    {
                                        stateList[i - 1].Remove(4);
                                        stateList[i - 1].Add(34);

                                    }
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(18))
                                        stateList[i].Add(18);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(20))
                                        stateList[i].Add(20);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }

                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(21))
                                        stateList[i].Add(21);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(6))
                                        stateList[i].Add(6);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }

                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(47))
                                        stateList[i].Add(47);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(16))
                                        stateList[i].Add(16);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(19))
                                        stateList[i].Add(19);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "خواست")
                                {
                                    if (!stateList[i].Contains(2))
                                        stateList[i].Add(2);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(11))
                                        stateList[i].Add(11);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }

                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(13))
                                        stateList[i].Add(13);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Positivity == TensePositivity.POSITIVE)
                                {
                                    if (!stateList[i].Contains(1))
                                        stateList[i].Add(1);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(9))
                                        stateList[i].Add(9);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Positivity == TensePositivity.NEGATIVE)
                                {
                                    if (!stateList[i].Contains(4))
                                        stateList[i].Add(4);
                                    stateList[i - 1].Remove(4);
                                    stateList[i - 1].Add(34);
                                }

                            }
                        }

                        #endregion

                        #region state 6

                        if (stateList[i - 1].Contains(6))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(39))
                                    stateList[i].Add(39);
                                stateList[i - 1].Remove(6);
                            }
                        }

                        #endregion

                        #region state 7

                        if (stateList[i - 1].Contains(7))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(40))
                                    stateList[i].Add(40);
                                stateList[i - 1].Remove(7);
                            }

                            if (verbInflection.VerbRoot.PresentTenseRoot == "هست" &&
                                verbInflection.Positivity == TensePositivity.NEGATIVE &&
                                verbInflection.ZamirPeyvasteh == AttachedPronounType.AttachedPronoun_NONE &&
                                verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH)
                            {
                                if (!stateList[i].Contains(40))
                                    stateList[i].Add(40);
                                stateList[i - 1].Remove(7);
                            }
                        }

                        #endregion

                        #region state 8

                        if (stateList[i - 1].Contains(8))
                        {
                            if (verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(43))
                                    stateList[i].Add(43);
                                stateList[i - 1].Remove(8);
                            }
                            if (verbInflection.VerbRoot.PastTenseRoot == "بود" &&
                                verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(44))
                                    stateList[i].Add(44);
                                stateList[i - 1].Remove(8);
                            }
                        }

                        #endregion

                        #region state 9

                        if (stateList[i - 1].Contains(9))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(45))
                                    stateList[i].Add(45);
                                stateList[i - 1].Remove(9);
                            }
                            else if (verbInflection.VerbRoot.PresentTenseRoot == "باش" &&
                                     verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                     verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                     verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(49))
                                    stateList[i].Add(49);
                                stateList[i - 1].Remove(9);
                            }
                            else if (verbInflection.VerbRoot.PresentTenseRoot == "باش" &&
                                     verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                     verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                     verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(50))
                                    stateList[i].Add(50);
                                stateList[i - 1].Remove(9);
                            }
                            else
                            {

                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!(verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                          verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                          verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                          verbInflection.Positivity == TensePositivity.POSITIVE))
                                    {
                                        if (!stateList[i].Contains(10))
                                            stateList[i].Add(10);
                                        stateList[i - 1].Remove(9);
                                        stateList[i - 1].Add(45);
                                    }
                                }

                                if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(14))
                                        stateList[i].Add(14);
                                    if (stateList[i - 1].Contains(9))
                                    {
                                        stateList[i - 1].Remove(9);
                                        stateList[i - 1].Add(45);
                                    }

                                }
                                if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(15))
                                        stateList[i].Add(15);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(17))
                                        stateList[i].Add(17);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(18))
                                        stateList[i].Add(18);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(20))
                                        stateList[i].Add(20);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(46))
                                        stateList[i].Add(46);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(21))
                                        stateList[i].Add(21);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }

                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                    verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                    verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(-1))
                                        stateList[i].Add(-1);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }

                                if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(16))
                                        stateList[i].Add(16);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(19))
                                        stateList[i].Add(19);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "خواست")
                                {
                                    if (!stateList[i].Contains(2))
                                        stateList[i].Add(2);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(11))
                                        stateList[i].Add(11);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }

                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(13))
                                        stateList[i].Add(13);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Positivity == TensePositivity.POSITIVE)
                                {
                                    if (!stateList[i].Contains(1))
                                        stateList[i].Add(1);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(9))
                                        stateList[i].Add(9);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Positivity == TensePositivity.NEGATIVE)
                                {
                                    if (!stateList[i].Contains(4))
                                        stateList[i].Add(4);
                                    stateList[i - 1].Remove(9);
                                    stateList[i - 1].Add(45);
                                }
                                if (stateList[i - 1].Contains(9))
                                {
                                    if (!stateList[i - 1].Contains(45))
                                        stateList[i - 1].Add(45);
                                    stateList[i - 1].Remove(9);
                                }
                            }
                        }

                        #endregion

                        #region state -1

                        if (stateList[i - 1].Contains(-1))
                        {
                            if (verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                verbInflection.Positivity == TensePositivity.POSITIVE)
                            {
                                if (!stateList[i].Contains(48))
                                    stateList[i].Add(48);
                                stateList[i - 1].Remove(-1);
                            }
                            else
                            {
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!(verbInflection.VerbRoot.PresentTenseRoot == "است" &&
                                          verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                          verbInflection.VerbRoot.Type == VerbType.SADEH &&
                                          verbInflection.Positivity == TensePositivity.POSITIVE))
                                    {
                                        if (!stateList[i].Contains(10))
                                            stateList[i].Add(10);
                                        stateList[i - 1].Remove(-1);
                                        stateList[i - 1].Add(48);
                                    }
                                }

                                if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(14))
                                        stateList[i].Add(14);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(15))
                                        stateList[i].Add(15);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);

                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(17))
                                        stateList[i].Add(17);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(18))
                                        stateList[i].Add(18);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(20))
                                        stateList[i].Add(20);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(46))
                                        stateList[i].Add(46);

                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(21))
                                        stateList[i].Add(21);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                    verbInflection.Person != PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(6))
                                        stateList[i].Add(6);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد" &&
                                    verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR &&
                                    verbInflection.Person == PersonType.THIRD_PERSON_SINGULAR)
                                {
                                    if (!stateList[i].Contains(-1))
                                        stateList[i].Add(-1);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);

                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد")
                                {
                                    if (!stateList[i].Contains(47))
                                        stateList[i].Add(47);

                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.AMR &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(16))
                                        stateList[i].Add(16);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(19))
                                        stateList[i].Add(19);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "خواست")
                                {
                                    if (!stateList[i].Contains(2))
                                        stateList[i].Add(2);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(11))
                                        stateList[i].Add(11);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.HAAL_ELTEZAMI &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(12))
                                        stateList[i].Add(12);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.GOZASHTEH_SADEH &&
                                    verbInflection.VerbRoot.PastTenseRoot == "شد")
                                {
                                    if (!stateList[i].Contains(13))
                                        stateList[i].Add(13);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Positivity == TensePositivity.POSITIVE)
                                {
                                    if (!stateList[i].Contains(1))
                                        stateList[i].Add(1);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                if (verbInflection.TenseForm == TenseFormationType.PAYEH_MAFOOLI &&
                                    verbInflection.VerbRoot.PastTenseRoot != "شد" &&
                                    verbInflection.Positivity == TensePositivity.NEGATIVE)
                                {
                                    if (!stateList[i].Contains(4))
                                        stateList[i].Add(4);
                                    stateList[i - 1].Remove(-1);
                                    stateList[i - 1].Add(48);
                                }
                                stateList[i - 1].Remove(-1);
                            }
                        }

                        #endregion
                    }
                    if (stateList[i].Count == 0)
                    {
                        stateList[i].Add(0);
                    }
                    if (!stateList[i].Contains(-2))
                        tempPishvand = "";
                }
            }
            return stateList;
        }

        /// <summary>
        /// get initial data for processing verb tokens in the sentence
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="posSentence"></param>
        /// <param name="newPOSTokens"></param>
        /// <param name="lemmas"></param>
        /// <param name="outLemmas"></param>
        /// <param name="verbDicPath"></param>
        /// <returns></returns>
        private static Dictionary<int, KeyValuePair<string, int>> GetOutputResult(string[] sentence, string[] posSentence, out string[] newPOSTokens, string[] lemmas, out string[] outLemmas)
        {
            var posTokens = new List<string>();
            var newLemmas = new List<string>();
            var list = VerbPartTagger.GetGoodResult(sentence, posSentence);
            var newList = new Dictionary<int, KeyValuePair<string, int>>();
            int counter = 0;
            var tempStr = new StringBuilder();
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i].Count() > 0)
                {
                    int value = list[i][0];
                    tempStr.Append(sentence[i]);
                    newList.Add(counter++, new KeyValuePair<string, int>(tempStr.ToString(), value));
                    if (value == 0)
                    {
                        posTokens.Add(posSentence[i]);
                        newLemmas.Add(lemmas[i]);
                    }
                    else
                    {
                        posTokens.Add("V");
                        newLemmas.Add(lemmas[i]);
                    }
                    tempStr = new StringBuilder();
                }
                else
                {
                    tempStr.Append(sentence[i] + " ");
                }
            }
            newPOSTokens = posTokens.ToArray();
            outLemmas = newLemmas.ToArray();
            return newList;
        }

        /// <summary>
        /// get initial data for processing verb tokens in the sentence
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        private static Dictionary<int, KeyValuePair<string, int>> GetOutputResult(string[] sentence)
        {
            var list = GetGoodResult(sentence);
            var newList = new Dictionary<int, KeyValuePair<string, int>>();
            int counter = 0;
            var tempStr = new StringBuilder();
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i].Count() > 0)
                {
                    int value = list[i][0];
                    tempStr.Append(sentence[i]);
                    newList.Add(counter++, new KeyValuePair<string, int>(tempStr.ToString(), value));
                    tempStr = new StringBuilder();
                }
                else
                {
                    tempStr.Append(sentence[i] + " ");
                }
            }
            return newList;
        }

        /// <summary>
        /// Get the verb tokens with their corresponding details
        /// </summary>
        /// <param name="sentence">array of words in the sentence</param>
        /// <param name="posTokens">array of POS tags in the sentence</param>
        /// <param name="newPosTokens">to be converted: new array of POS tags in the sentence</param>
        /// <param name="lemmas">array of lemmas in the sentence</param>
        /// <param name="outLemmas">to be converted: new array of lemmas in the sentence</param>
        /// <param name="verbDicPath">path of dictionary file</param>
        /// <returns></returns>
        public static Dictionary<int, KeyValuePair<string, VerbInflection>> GetVerbTokens(string[] sentence, string[] posTokens, out string[] newPosTokens, string[] lemmas, out string[] outLemmas)
        {
            var outputResults = new Dictionary<int, KeyValuePair<string, VerbInflection>>();
            var output = GetOutputResult(sentence, posTokens, out newPosTokens, lemmas, out outLemmas);
            for (int i = 0; i < output.Count; i++)
            {
                var values = output[i];
                VerbInflection inflection;
                TensePassivity passivity;
                TensePositivity positivity;
                Verb verb;
                PersonType shakhsType;
                TenseFormationType tenseFormationType;
                AttachedPronounType zamirPeyvastehType;
                string zamirString;
                List<VerbInflection> tempInfleclist;
                VerbInflection tempInflec;
                string[] tokens = output[i].Key.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                switch (values.Value)
                {
                    #region 10

                    case 10:
                        tenseFormationType = TenseFormationType.HAAL_SAADEH_EKHBARI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                       positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 53

                    case 53:
                        tenseFormationType = TenseFormationType.HAAL_SAADEH;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH && (inflectionIter.VerbRoot.PastTenseRoot == "بایست" || inflectionIter.VerbRoot.PresentTenseRoot == "توان"))
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                       positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 14

                    case 14:
                        tenseFormationType = TenseFormationType.HAAL_ELTEZAMI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 15

                    case 15:
                        tenseFormationType = TenseFormationType.AMR;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.AMR)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirString = tempInflec.AttachedPronounString;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 17

                    case 17:
                        tenseFormationType = TenseFormationType.GOZASHTEH_SADEH;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                             positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 18

                    case 18:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ESTEMRAARI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                        positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 20

                    case 20:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                        positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 21

                    case 21:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                        positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 11

                    case 11:
                        tenseFormationType = TenseFormationType.HAAL_SAADEH_EKHBARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb.PastTenseRoot = "کرد";
                        verb.PresentTenseRoot = "کن";
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 16

                    case 16:
                        tenseFormationType = TenseFormationType.AMR;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.AMR)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb.PastTenseRoot = "کرد";
                        verb.PresentTenseRoot = "کن";
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 12

                    case 12:
                        tenseFormationType = TenseFormationType.HAAL_ELTEZAMI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb.PastTenseRoot = "کرد";
                        verb.PresentTenseRoot = "کن";
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 13

                    case 13:
                        tenseFormationType = TenseFormationType.GOZASHTEH_SADEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb.PastTenseRoot = "کرد";
                        verb.PresentTenseRoot = "کن";
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 19

                    case 19:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ESTEMRAARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb.PastTenseRoot = "کرد";
                        verb.PresentTenseRoot = "کن";
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 22

                    case 22:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 23

                    case 23:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ESTEMRAARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 24

                    case 24:
                        tenseFormationType = TenseFormationType.HAAL_SAADEH_EKHBARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 25

                    case 25:
                        tenseFormationType = TenseFormationType.HAAL_ELTEZAMI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 26

                    case 26:
                        tenseFormationType = TenseFormationType.GOZASHTEH_SADEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 27

                    case 27:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();

                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                        if (tokens.Length == 2)
                        {
                            if (tokens[1] == "نیست")
                            {
                                positivity = TensePositivity.NEGATIVE;
                            }
                            else
                            {
                                positivity = TensePositivity.POSITIVE;
                            }
                        }
                        else
                        {
                            positivity = TensePositivity.POSITIVE;
                        }
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 28

                    case 28:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 29

                    case 29:
                        tenseFormationType = TenseFormationType.GOZASHTEH_BAEED;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 30

                    case 30:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ELTEZAMI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 31

                    case 31:
                        tenseFormationType = TenseFormationType.AAYANDEH;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        verb = new Verb("", tempInflec.VerbRoot.PastTenseRoot, tempInflec.VerbRoot.PresentTenseRoot,
                                        tempInflec.VerbRoot.Prefix, "", VerbTransitivity.Transitive,
                                        tempInflec.VerbRoot.Type, true,
                                        tempInflec.VerbRoot.PresentRootConsonantVowelEndStem,
                                        tempInflec.VerbRoot.PastRootVowelStart,
                                        tempInflec.VerbRoot.PresentRootVowelStart);
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
						if (inflectionIter.IsPayehFelMasdari() && inflectionIter.Positivity==TensePositivity.POSITIVE)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb.PastTenseRoot = tempInflec.VerbRoot.PastTenseRoot;
                        verb.PresentTenseRoot = tempInflec.VerbRoot.PresentTenseRoot;
                        verb.Transitivity = tempInflec.VerbRoot.Transitivity;
                        verb.CanBeImperative = tempInflec.VerbRoot.CanBeImperative;
                        verb.PresentRootConsonantVowelEndStem = tempInflec.VerbRoot.PresentRootConsonantVowelEndStem;
                        verb.PastRootVowelStart = tempInflec.VerbRoot.PastRootVowelStart;
                        verb.PresentRootVowelStart = tempInflec.VerbRoot.PresentRootVowelStart;
                        if (zamirPeyvastehType == AttachedPronounType.AttachedPronoun_NONE)
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 32

                    case 32:
                        tenseFormationType = TenseFormationType.AAYANDEH;
                        passivity = TensePassivity.PASSIVE; //ToCheck
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        verb = new Verb("", tempInflec.VerbRoot.PastTenseRoot, tempInflec.VerbRoot.PresentTenseRoot,
                                        tempInflec.VerbRoot.Prefix, "", VerbTransitivity.Transitive,
                                        tempInflec.VerbRoot.Type, true,
                                        tempInflec.VerbRoot.PresentRootConsonantVowelEndStem,
                                        tempInflec.VerbRoot.PastRootVowelStart,
                                        tempInflec.VerbRoot.PresentRootVowelStart);
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.IsPayehFelMasdari())
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb.PastTenseRoot = "کرد";
                        verb.PresentTenseRoot = "کن";
                        verb.Transitivity = VerbTransitivity.Transitive;
                        verb.CanBeImperative = tempInflec.VerbRoot.CanBeImperative;
                        verb.PresentRootConsonantVowelEndStem = tempInflec.VerbRoot.PresentRootConsonantVowelEndStem;
                        verb.PastRootVowelStart = tempInflec.VerbRoot.PastRootVowelStart;
                        verb.PresentRootVowelStart = tempInflec.VerbRoot.PresentRootVowelStart;
                        if (zamirPeyvastehType == AttachedPronounType.AttachedPronoun_NONE)
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 34

                    case 34:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 38,39

                    case 38:
                    case 39:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 41

                    case 41:
                        tenseFormationType = TenseFormationType.GOZASHTEH_BAEED;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        positivity = tempInflec.Positivity;
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 42

                    case 42:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ELTEZAMI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        positivity = tempInflec.Positivity;
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 33

                    case 33:
                        tenseFormationType = TenseFormationType.AAYANDEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                        tempInfleclist = VerbList.VerbShapes[tokens[2]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        if (shakhsType == PersonType.PERSON_NONE)
                            shakhsType = tempInflec.Person;

                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 35

                    case 35:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                        if (tokens.Length == 3)
                        {
                            if (tokens[2] == "نیست")
                            {
                                positivity = TensePositivity.NEGATIVE;
                            }
                            else
                            {
                                positivity = TensePositivity.POSITIVE;
                            }
                        }
                        else
                        {
                            positivity = TensePositivity.POSITIVE;
                        }
                        zamirPeyvastehType = AttachedPronounType.AttachedPronoun_NONE; zamirString = "";

                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 36

                    case 36:
                        tenseFormationType = TenseFormationType.GOZASHTEH_BAEED;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[2]];


                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = TensePositivity.POSITIVE;

						tempInfleclist = VerbList.VerbShapes[tokens[2]];
						tempInflec = null;
						foreach (VerbInflection inflectionIter in tempInfleclist)
						{
							if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
							{
								tempInflec = inflectionIter;
								break;
							}
						}
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 37

                    case 37:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ELTEZAMI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[2]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
						if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH  || inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = TensePositivity.POSITIVE;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 40

                    case 40:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                        if (tokens.Length == 3)
                        {
                            if (tokens[2] == "نیست")
                            {
                                positivity = TensePositivity.NEGATIVE;
                            }
                            else
                            {
                                positivity = TensePositivity.POSITIVE;
                            }
                        }
                        else
                        {
                            positivity = TensePositivity.POSITIVE;
                        }
                        zamirPeyvastehType = AttachedPronounType.AttachedPronoun_NONE; zamirString = "";

                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 43

                    case 43:
                        tenseFormationType = TenseFormationType.GOZASHTEH_BAEED;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[2]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = TensePositivity.NEGATIVE;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 44

                    case 44:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ELTEZAMI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[2]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = TensePositivity.NEGATIVE;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 45
                    case 45:
                        tenseFormationType = TenseFormationType.GOZASHTEH_SADEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");

                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;
                    #endregion

                    #region 46

                    case 46:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 47

                    case 47:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 48

                    case 48:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 49

                    case 49:
                        tenseFormationType = TenseFormationType.GOZASHTEH_BAEED;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 50

                    case 50:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ELTEZAMI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 51

                    case 51:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ABAD;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;

                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }

                        shakhsType = tempInflec.Person;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 52

                    case 52:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ABAD;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;

						tempInfleclist = VerbList.VerbShapes[tokens[1]];
						tempInflec = null;
						foreach (VerbInflection inflectionIter in tempInfleclist)
						{
							if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
							{
								tempInflec = inflectionIter;
								break;
							}
						}
					if (tempInflec!=null){
						positivity = tempInflec.Positivity;
						passivity=TensePassivity.PASSIVE;
					}

                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;

                        zamirPeyvastehType = AttachedPronounType.AttachedPronoun_NONE; zamirString = "";

                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region default
                    default:
                        VerbInflection nullinflec = null;
                        if (i == output.Count - 1 && output[i].Value == 1)
                        {
                            tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                            passivity = TensePassivity.ACTIVE;
                            tempInfleclist = VerbList.VerbShapes[tokens[0]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            verb = tempInflec.VerbRoot.Clone();
                            positivity = tempInflec.Positivity;
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                            shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                            inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                             positivity, passivity);
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        }
                        else if (i == output.Count - 1 && output[i].Value == 5)
                        {
                            tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                            passivity = TensePassivity.PASSIVE;
                            tempInfleclist = VerbList.VerbShapes[tokens[0]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            verb = tempInflec.VerbRoot.Clone();
                            positivity = tempInflec.Positivity;
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                            shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                            inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                             positivity, passivity);
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        }
                        else if (i == output.Count - 1 && output[i].Value == 4)
                        {
                            tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                            passivity = TensePassivity.ACTIVE;
                            tempInfleclist = VerbList.VerbShapes[tokens[0]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            verb = tempInflec.VerbRoot.Clone();
                            positivity = tempInflec.Positivity;
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                            shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                            inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                             positivity, passivity);
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        }
                        else if (i == output.Count - 1 && output[i].Value == 7)
                        {
                            tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                            passivity = TensePassivity.PASSIVE;
                            tempInfleclist = VerbList.VerbShapes[tokens[0]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            verb = tempInflec.VerbRoot.Clone();

                            tempInfleclist = VerbList.VerbShapes[tokens[1]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.VerbRoot.PastTenseRoot == "شد")
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            positivity = tempInflec.Positivity;
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                            shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                            inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                             positivity, passivity);
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        }
                        else if (i == output.Count - 1 && output[i].Value == 6)
                        {
                            tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                            passivity = TensePassivity.ACTIVE;
                            tempInfleclist = VerbList.VerbShapes[tokens[0]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI)
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            verb = tempInflec.VerbRoot.Clone();
                            positivity = tempInflec.Positivity;
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                            shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                            inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                             positivity, passivity);
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        }
                        else if (i == output.Count - 1 && output[i].Value == 9)
                        {
                            tenseFormationType = TenseFormationType.GOZASHTEH_SADEH;
                            passivity = TensePassivity.PASSIVE;
                            tempInfleclist = VerbList.VerbShapes[tokens[0]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            verb = tempInflec.VerbRoot.Clone();
                            positivity = tempInflec.Positivity;
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                            verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");

                            shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                            inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                             positivity, passivity);
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        }
                        else
                        {
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, nullinflec));
                        }
                        break;
                    #endregion
                }
            }
            return outputResults;
        }
	
        /// <summary>
        /// Get the verb tokens with their corresponding details
        /// </summary>
        /// <param name="sentence">array of words in the sentence</param>
        /// <param name="verbDicPath">path of dictionary file</param>
        /// <returns></returns>
        public static Dictionary<int, KeyValuePair<string, VerbInflection>> GetVerbTokens(string[] sentence)
        {
            var outputResults = new Dictionary<int, KeyValuePair<string, VerbInflection>>();
            var output = GetOutputResult(sentence);
            for (int i = 0; i < output.Count; i++)
            {
	                var values = output[i];
                string zamirString;
                VerbInflection inflection;
                TensePassivity passivity;
                TensePositivity positivity;
                Verb verb;
                PersonType shakhsType;
                TenseFormationType tenseFormationType;
                AttachedPronounType zamirPeyvastehType;
                List<VerbInflection> tempInfleclist;
                VerbInflection tempInflec;
                string[] tokens = output[i].Key.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                switch (values.Value)
                {
                    #region 10

                    case 10:
                        tenseFormationType = TenseFormationType.HAAL_SAADEH_EKHBARI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh;
                        zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                        positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 53

                    case 53:
                        tenseFormationType = TenseFormationType.HAAL_SAADEH;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH && (inflectionIter.VerbRoot.PastTenseRoot == "بایست" || inflectionIter.VerbRoot.PresentTenseRoot == "توان"))
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                       positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 14

                    case 14:
                        tenseFormationType = TenseFormationType.HAAL_ELTEZAMI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                        positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 15

                    case 15:
                        tenseFormationType = TenseFormationType.AMR;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.AMR)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                        positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 17

                    case 17:
                        tenseFormationType = TenseFormationType.GOZASHTEH_SADEH;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                        positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 18

                    case 18:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ESTEMRAARI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                        positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 20

                    case 20:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                        positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 21

                    case 21:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        shakhsType = tempInflec.Person;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                        positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 11

                    case 11:
                        tenseFormationType = TenseFormationType.HAAL_SAADEH_EKHBARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb.PastTenseRoot = "کرد";
                        verb.PresentTenseRoot = "کن";
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 16

                    case 16:
                        tenseFormationType = TenseFormationType.AMR;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.AMR)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb.PastTenseRoot = "کرد";
                        verb.PresentTenseRoot = "کن";
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 12

                    case 12:
                        tenseFormationType = TenseFormationType.HAAL_ELTEZAMI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb.PastTenseRoot = "کرد";
                        verb.PresentTenseRoot = "کن";
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 13

                    case 13:
                        tenseFormationType = TenseFormationType.GOZASHTEH_SADEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb.PastTenseRoot = "کرد";
                        verb.PresentTenseRoot = "کن";
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 19

                    case 19:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ESTEMRAARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb.PastTenseRoot = "کرد";
                        verb.PresentTenseRoot = "کن";
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 22

                    case 22:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 23

                    case 23:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ESTEMRAARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 24

                    case 24:
                        tenseFormationType = TenseFormationType.HAAL_SAADEH_EKHBARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 25

                    case 25:
                        tenseFormationType = TenseFormationType.HAAL_ELTEZAMI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 26

                    case 26:
                        tenseFormationType = TenseFormationType.GOZASHTEH_SADEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 27

                    case 27:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();

                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                        if (tokens.Length == 2)
                        {
                            if (tokens[1] == "نیست")
                            {
                                positivity = TensePositivity.NEGATIVE;
                            }
                            else
                            {
                                positivity = TensePositivity.POSITIVE;
                            }
                        }
                        else
                        {
                            positivity = TensePositivity.POSITIVE;
                        }
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 28

                    case 28:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 29

                    case 29:
                        tenseFormationType = TenseFormationType.GOZASHTEH_BAEED;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 30

                    case 30:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ELTEZAMI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 31

                    case 31:
                        tenseFormationType = TenseFormationType.AAYANDEH;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        verb = new Verb("", tempInflec.VerbRoot.PastTenseRoot, tempInflec.VerbRoot.PresentTenseRoot,
                                        tempInflec.VerbRoot.Prefix, "", VerbTransitivity.Transitive,
                                        tempInflec.VerbRoot.Type, true,
                                        tempInflec.VerbRoot.PresentRootConsonantVowelEndStem,
                                        tempInflec.VerbRoot.PastRootVowelStart,
                                        tempInflec.VerbRoot.PresentRootVowelStart);
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.IsPayehFelMasdari() && inflectionIter.Positivity==TensePositivity.POSITIVE)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb.PastTenseRoot = tempInflec.VerbRoot.PastTenseRoot;
                        verb.PresentTenseRoot = tempInflec.VerbRoot.PresentTenseRoot;
                        verb.Transitivity = tempInflec.VerbRoot.Transitivity;
                        verb.CanBeImperative = tempInflec.VerbRoot.CanBeImperative;
                        verb.PresentRootConsonantVowelEndStem = tempInflec.VerbRoot.PresentRootConsonantVowelEndStem;
                        verb.PastRootVowelStart = tempInflec.VerbRoot.PastRootVowelStart;
                        verb.PresentRootVowelStart = tempInflec.VerbRoot.PresentRootVowelStart;
                        if (zamirPeyvastehType == AttachedPronounType.AttachedPronoun_NONE)
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 32

                    case 32:
                        tenseFormationType = TenseFormationType.AAYANDEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        verb = new Verb("", tempInflec.VerbRoot.PastTenseRoot, tempInflec.VerbRoot.PresentTenseRoot,
                                        tempInflec.VerbRoot.Prefix, "", VerbTransitivity.Transitive,
                                        tempInflec.VerbRoot.Type, true,
                                        tempInflec.VerbRoot.PresentRootConsonantVowelEndStem,
                                        tempInflec.VerbRoot.PastRootVowelStart,
                                        tempInflec.VerbRoot.PresentRootVowelStart);
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.IsPayehFelMasdari())
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb.PastTenseRoot = "کرد";
                        verb.PresentTenseRoot = "کن";
                        verb.Transitivity = VerbTransitivity.Transitive;
                        verb.CanBeImperative = tempInflec.VerbRoot.CanBeImperative;
                        verb.PresentRootConsonantVowelEndStem = tempInflec.VerbRoot.PresentRootConsonantVowelEndStem;
                        verb.PastRootVowelStart = tempInflec.VerbRoot.PastRootVowelStart;
                        verb.PresentRootVowelStart = tempInflec.VerbRoot.PresentRootVowelStart;
                        if (zamirPeyvastehType == AttachedPronounType.AttachedPronoun_NONE)
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 34

                    case 34:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 38,39

                    case 38:
                    case 39:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 41

                    case 41:
                        tenseFormationType = TenseFormationType.GOZASHTEH_BAEED;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        positivity = tempInflec.Positivity;
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 42

                    case 42:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ELTEZAMI;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        positivity = tempInflec.Positivity;
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 33

                    case 33:
                        tenseFormationType = TenseFormationType.AAYANDEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                        tempInfleclist = VerbList.VerbShapes[tokens[2]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        if (shakhsType == PersonType.PERSON_NONE)
                            shakhsType = tempInflec.Person;

                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 35

                    case 35:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                        if (tokens.Length == 3)
                        {
                            if (tokens[2] == "نیست")
                            {
                                positivity = TensePositivity.NEGATIVE;
                            }
                            else
                            {
                                positivity = TensePositivity.POSITIVE;
                            }
                        }
                        else
                        {
                            positivity = TensePositivity.POSITIVE;
                        }
                        zamirPeyvastehType = AttachedPronounType.AttachedPronoun_NONE; zamirString = "";

                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 36

                    case 36:
                        tenseFormationType = TenseFormationType.GOZASHTEH_BAEED;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
					positivity = TensePositivity.POSITIVE;
					if(tempInflec.Positivity==TensePositivity.NEGATIVE)
						positivity = TensePositivity.NEGATIVE;

					tempInfleclist = VerbList.VerbShapes[tokens[1]];
					tempInflec = null;
					foreach (VerbInflection inflectionIter in tempInfleclist)
					{

							tempInflec = inflectionIter;
							break;

					}
					if(tempInflec.Positivity==TensePositivity.NEGATIVE)
						positivity = TensePositivity.NEGATIVE;

                        tempInfleclist = VerbList.VerbShapes[tokens[2]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
					if(tempInflec.Positivity==TensePositivity.NEGATIVE)
						positivity = TensePositivity.NEGATIVE;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 37

                    case 37:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ELTEZAMI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[2]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
						if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH || inflectionIter.TenseForm == TenseFormationType.HAAL_ELTEZAMI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = TensePositivity.POSITIVE;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 40

                    case 40:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                        if (tokens.Length == 3)
                        {
                            if (tokens[2] == "نیست")
                            {
                                positivity = TensePositivity.NEGATIVE;
                            }
                            else
                            {
                                positivity = TensePositivity.POSITIVE;
                            }
                        }
                        else
                        {
                            positivity = TensePositivity.POSITIVE;
                        }
                        zamirPeyvastehType = AttachedPronounType.AttachedPronoun_NONE; zamirString = "";

                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 43

                    case 43:
                        tenseFormationType = TenseFormationType.GOZASHTEH_BAEED;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[2]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = TensePositivity.NEGATIVE;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 44

                    case 44:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ELTEZAMI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        tempInfleclist = VerbList.VerbShapes[tokens[2]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.HAAL_SAADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        shakhsType = tempInflec.Person;
                        positivity = TensePositivity.NEGATIVE;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 45
                    case 45:
                        tenseFormationType = TenseFormationType.GOZASHTEH_SADEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");

                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;
                    #endregion

                    #region 46

                    case 46:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 47

                    case 47:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 48

                    case 48:
                        tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 49

                    case 49:
                        tenseFormationType = TenseFormationType.GOZASHTEH_BAEED;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 50

                    case 50:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ELTEZAMI;
                        passivity = TensePassivity.PASSIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");
                        shakhsType = tempInflec.Person;
                        positivity = tempInflec.Positivity;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 51

                    case 51:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ABAD;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;

                        tempInfleclist = VerbList.VerbShapes[tokens[1]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }

                        shakhsType = tempInflec.Person;
                        zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region 52

                    case 52:
                        tenseFormationType = TenseFormationType.GOZASHTEH_ABAD;
                        passivity = TensePassivity.ACTIVE;
                        tempInfleclist = VerbList.VerbShapes[tokens[0]];
                        tempInflec = null;
                        foreach (VerbInflection inflectionIter in tempInfleclist)
                        {
                            if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                            {
                                tempInflec = inflectionIter;
                                break;
                            }
                        }
                        verb = tempInflec.VerbRoot.Clone();
                        positivity = tempInflec.Positivity;
						tempInfleclist = VerbList.VerbShapes[tokens[1]];
						tempInflec = null;
						foreach (VerbInflection inflectionIter in tempInfleclist)
						{
							if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
							{
								tempInflec = inflectionIter;
								break;
							}
						}
					if (tempInflec!=null){
						positivity = tempInflec.Positivity;
						passivity=TensePassivity.PASSIVE;
					}
                        shakhsType = PersonType.THIRD_PERSON_SINGULAR;

                        zamirPeyvastehType = AttachedPronounType.AttachedPronoun_NONE; zamirString = "";

                        inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                         positivity, passivity);
                        outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        break;

                    #endregion

                    #region default
                    default:
                        VerbInflection nullinflec = null;
                        if (i == output.Count - 1 && output[i].Value == 1)
                        {
                            tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                            passivity = TensePassivity.ACTIVE;
                            tempInfleclist = VerbList.VerbShapes[tokens[0]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            verb = tempInflec.VerbRoot.Clone();
                            positivity = tempInflec.Positivity;
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                            shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                            inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                             positivity, passivity);
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        }
                        else if (i == output.Count - 1 && output[i].Value == 5)
                        {
                            tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                            passivity = TensePassivity.PASSIVE;
                            tempInfleclist = VerbList.VerbShapes[tokens[0]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            verb = tempInflec.VerbRoot.Clone();
                            positivity = tempInflec.Positivity;
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                            shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                            inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                             positivity, passivity);
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        }
                        else if (i == output.Count - 1 && output[i].Value == 4)
                        {
                            tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
                            passivity = TensePassivity.ACTIVE;
                            tempInfleclist = VerbList.VerbShapes[tokens[0]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            verb = tempInflec.VerbRoot.Clone();
                            positivity = tempInflec.Positivity;
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                            shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                            inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                             positivity, passivity);
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        }
                        else if (i == output.Count - 1 && output[i].Value == 7)
                        {
                            tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                            passivity = TensePassivity.PASSIVE;
                            tempInfleclist = VerbList.VerbShapes[tokens[0]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            verb = tempInflec.VerbRoot.Clone();

                            tempInfleclist = VerbList.VerbShapes[tokens[1]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.VerbRoot.PastTenseRoot == "شد")
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            positivity = tempInflec.Positivity;
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                            shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                            inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                             positivity, passivity);
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        }
                        else if (i == output.Count - 1 && output[i].Value == 6)
                        {
                            tenseFormationType = TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
                            passivity = TensePassivity.ACTIVE;
                            tempInfleclist = VerbList.VerbShapes[tokens[0]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI)
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            verb = tempInflec.VerbRoot.Clone();
                            positivity = tempInflec.Positivity;
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;

                            shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                            inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                             positivity, passivity);
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        }
                        else if (i == output.Count - 1 && output[i].Value == 9)
                        {
                            tenseFormationType = TenseFormationType.GOZASHTEH_SADEH;
                            passivity = TensePassivity.PASSIVE;
                            tempInfleclist = VerbList.VerbShapes[tokens[0]];
                            tempInflec = null;
                            foreach (VerbInflection inflectionIter in tempInfleclist)
                            {
                                if (inflectionIter.TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                                {
                                    tempInflec = inflectionIter;
                                    break;
                                }
                            }
                            verb = tempInflec.VerbRoot.Clone();
                            positivity = tempInflec.Positivity;
                            zamirPeyvastehType = tempInflec.ZamirPeyvasteh; zamirString = tempInflec.AttachedPronounString;
                            verb = new Verb(verb.PrepositionOfVerb, "کرد", "کن", verb.Prefix, verb.NonVerbalElement, VerbTransitivity.Transitive, verb.Type, true, "?", "@", "!");

                            shakhsType = PersonType.THIRD_PERSON_SINGULAR;
                            inflection = new VerbInflection(verb, zamirPeyvastehType, zamirString, shakhsType, tenseFormationType,
                                                             positivity, passivity);
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, inflection));
                        }
                        else
                        {
                            outputResults.Add(i, new KeyValuePair<string, VerbInflection>(output[i].Key, nullinflec));
                        }
                        break;
                    #endregion
                }
            }
            return outputResults;
        }
    }
}
