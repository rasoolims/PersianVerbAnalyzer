using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VerbInflector
{
    public class VerbList
    {

        /// <summary>
        /// Saves all possible inflections of a special verb representation
        /// </summary>
        public static Dictionary<string, List<VerbInflection>> VerbShapes { private set; get; }

        public static Dictionary<string, List<string>> VerbPishvandiDic { private set; get; }

        /// <summary>
        /// A dictionay of possible complex predicates in Persian verbs
        /// </summary>
        public static Dictionary<Verb, Dictionary<string, Dictionary<string, bool>>> CompoundVerbDic { private set; get; }

        /// <summary>
        /// Constructs all dictionaries used in the inflection program
        /// </summary>
        /// <param name="verbDicPath"></param>
        public VerbList(string verbDicPath)
        {
            VerbPishvandiDic = new Dictionary<string, List<string>>();
            VerbShapes = new Dictionary<string, List<VerbInflection>>();
            CompoundVerbDic = new Dictionary<Verb, Dictionary<string, Dictionary<string, bool>>>();
            var verbs = new List<Verb>();
            string[] records = File.ReadAllText(verbDicPath).Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var record in records)
            {
                string[] fields = record.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                int vtype = int.Parse(fields[0]);

                if (vtype == 1 || vtype == 2)
                {
                    var verbType = VerbType.SADEH;
                    if (vtype == 2)
                        verbType = VerbType.PISHVANDI;
                    int trans = int.Parse(fields[1]);
                    var transitivity = VerbTransitivity.Transitive;
                    if (trans == 0)
                        transitivity = VerbTransitivity.InTransitive;
                    else if (trans == 2)
                        transitivity = VerbTransitivity.BiTransitive;
                    string pishvand = "";
                    if (fields[5] != "-")
                    {
                        pishvand = fields[5];
                    }
                    Verb verb;
                    bool amrShodani = true;
                    if (fields[7] == "*")
                        amrShodani = false;
                    string vowelEnd = fields[8];
                    string maziVowel = fields[9];
                    string mozarehVowel = fields[10];
                    if (fields[3] == "-")
                        verb = new Verb("", fields[2], "", pishvand, "", transitivity, verbType, amrShodani,
                                        vowelEnd, maziVowel, mozarehVowel);
                    else if (fields[2] == "-")
                        verb = new Verb("", "", fields[3], pishvand, "", transitivity, verbType, amrShodani,
                                        vowelEnd, maziVowel, mozarehVowel);
                    else
                        verb = new Verb("", fields[2], fields[3], pishvand, "", transitivity, verbType, amrShodani,
                                        vowelEnd, maziVowel, mozarehVowel);

                    verbs.Add(verb);
                    if (verb.Type == VerbType.PISHVANDI)
                    {
                        verbs.Add(new Verb("", "", "خواه", pishvand, "", VerbTransitivity.InTransitive,
                                           VerbType.AYANDEH_PISHVANDI, false, "?", "@", "!"));
                        if (VerbPishvandiDic.ContainsKey(pishvand))
                        {
                            VerbPishvandiDic[pishvand].Add(verb.PastTenseRoot + "|" + verb.PresentTenseRoot);
                        }
                        else
                        {
                            var lst = new List<string>();
                            lst.Add(verb.PastTenseRoot + "|" + verb.PresentTenseRoot);
                            VerbPishvandiDic.Add(pishvand, lst);
                        }
                    }
                }
                else if (vtype == 3)
                {
                    var verbType = VerbType.SADEH;
                    int trans = int.Parse(fields[1]);
                    VerbTransitivity transitivity = VerbTransitivity.Transitive;
                    if (trans == 0)
                        transitivity = VerbTransitivity.InTransitive;
                    else if (trans == 2)
                        transitivity = VerbTransitivity.BiTransitive;
                    Verb verb;
                    bool amrShodani = true;
                    string vowelEnd = fields[8];
                    string maziVowel = fields[9];
                    string mozarehVowel = fields[10];
                    string nonVerbalElemant = fields[4];
                    if (fields[3] == "-")
                        verb = new Verb("", fields[2], "", "", "", transitivity, verbType, amrShodani,
                                        vowelEnd, maziVowel, mozarehVowel);
                    else if (fields[2] == "-")
                        verb = new Verb("", "", fields[3], "", "", transitivity, verbType, amrShodani,
                                        vowelEnd, maziVowel, mozarehVowel);
                    else
                        verb = new Verb("", fields[2], fields[3], "", "", transitivity, verbType, amrShodani,
                                        vowelEnd, maziVowel, mozarehVowel);
                    if (fields[7] == "*")
                        amrShodani = false;
					var istran=true;
					if(transitivity==VerbTransitivity.InTransitive)
						istran=false;

                    if (!CompoundVerbDic.ContainsKey(verb))
                        CompoundVerbDic.Add(verb, new Dictionary<string, Dictionary<string, bool>>());
                    if (!CompoundVerbDic[verb].ContainsKey(nonVerbalElemant))
                    {
                        CompoundVerbDic[verb].Add(nonVerbalElemant, new Dictionary<string, bool>());
                    }
                    if (!CompoundVerbDic[verb][nonVerbalElemant].ContainsKey(""))
                        CompoundVerbDic[verb][nonVerbalElemant].Add("", istran);

                }
                else if (vtype == 4)
                {
                    var verbType = VerbType.PISHVANDI;
                    int trans = int.Parse(fields[1]);
                    VerbTransitivity transitivity = VerbTransitivity.Transitive;
                    if (trans == 0)
                        transitivity = VerbTransitivity.InTransitive;
                    else if (trans == 2)
                        transitivity = VerbTransitivity.BiTransitive;
                    Verb verb;
                    string pishvand = "";
                    if (fields[5] != "-")
                    {
                        pishvand = fields[5];
                    }
                    bool amrShodani = true;
                    string vowelEnd = fields[8];
                    string maziVowel = fields[9];
                    string mozarehVowel = fields[10];
                    string nonVerbalElemant = fields[4];
                    if (fields[3] == "-")
                        verb = new Verb("", fields[2], "", pishvand, "", transitivity, verbType, amrShodani,
                                        vowelEnd, maziVowel, mozarehVowel);
                    else if (fields[2] == "-")
                        verb = new Verb("", "", fields[3], pishvand, "", transitivity, verbType, amrShodani,
                                        vowelEnd, maziVowel, mozarehVowel);
                    else
                        verb = new Verb("", fields[2], fields[3], pishvand, "", transitivity, verbType, amrShodani,
                                        vowelEnd, maziVowel, mozarehVowel);
                    if (fields[7] == "*")
                        amrShodani = false;
					var istrans=true;
					if(transitivity==VerbTransitivity.InTransitive)
						istrans=false;
                    if (!CompoundVerbDic.ContainsKey(verb))
                        CompoundVerbDic.Add(verb, new Dictionary<string, Dictionary<string, bool>>());
                    if (!CompoundVerbDic[verb].ContainsKey(nonVerbalElemant))
                    {
                        CompoundVerbDic[verb].Add(nonVerbalElemant, new Dictionary<string, bool>());
                    }
                    if (!CompoundVerbDic[verb][nonVerbalElemant].ContainsKey(""))
						CompoundVerbDic[verb][nonVerbalElemant].Add("", istrans);
                }
                else if (vtype == 5 || vtype == 7)
                {
                    var verbType = VerbType.SADEH;
                    int trans = int.Parse(fields[1]);
                    VerbTransitivity transitivity = VerbTransitivity.Transitive;
                    if (trans == 0)
                        transitivity = VerbTransitivity.InTransitive;
                    else if (trans == 2)
                        transitivity = VerbTransitivity.BiTransitive;
                    Verb verb;
                    string pishvand = "";
                    if (fields[5] != "-")
                    {
                        pishvand = fields[5];
                    }
                    if (pishvand != "")
                    {
                        verbType = VerbType.PISHVANDI;
                    }
                    bool amrShodani = true;
                    string vowelEnd = fields[8];
                    string maziVowel = fields[9];
                    string mozarehVowel = fields[10];
                    string nonVerbalElemant = fields[4];
                    string harfeEazafeh = fields[6];
                    if (fields[3] == "-")
                        verb = new Verb("", fields[2], "", pishvand, "", transitivity, verbType, amrShodani,
                                        vowelEnd, maziVowel, mozarehVowel);
                    else if (fields[2] == "-")
                        verb = new Verb("", "", fields[3], pishvand, "", transitivity, verbType, amrShodani,
                                        vowelEnd, maziVowel, mozarehVowel);
                    else
                        verb = new Verb("", fields[2], fields[3], pishvand, "", transitivity, verbType, amrShodani,
                                        vowelEnd, maziVowel, mozarehVowel);
                    if (fields[7] == "*")
                        amrShodani = false;
					var istrans=true;
					if(transitivity==VerbTransitivity.InTransitive)
						istrans=false;
                    if (!CompoundVerbDic.ContainsKey(verb))
                        CompoundVerbDic.Add(verb, new Dictionary<string, Dictionary<string, bool>>());
                    if (!CompoundVerbDic[verb].ContainsKey(nonVerbalElemant))
                    {
                        CompoundVerbDic[verb].Add(nonVerbalElemant, new Dictionary<string, bool>());
                    }
                    if (!CompoundVerbDic[verb][nonVerbalElemant].ContainsKey(harfeEazafeh))

                        CompoundVerbDic[verb][nonVerbalElemant].Add(harfeEazafeh, istrans);
                }
            }
            var verbtext = new StringBuilder();
            var mitavanInflection = new VerbInflection(new Verb("", "", "توان", "", "", VerbTransitivity.InTransitive, VerbType.SADEH, false, "?", "@", "!"), AttachedPronounType.AttachedPronoun_NONE, "",
                                                                     PersonType.PERSON_NONE,
                                                                     TenseFormationType.HAAL_SAADEH, TensePositivity.POSITIVE);
            VerbShapes.Add("می‌توان", new List<VerbInflection>());
            VerbShapes["می‌توان"].Add(mitavanInflection);
            var nemitavanInflection = new VerbInflection(new Verb("", "", "توان", "", "", VerbTransitivity.InTransitive, VerbType.SADEH, false, "?", "@", "!"), AttachedPronounType.AttachedPronoun_NONE, "",
                                                                     PersonType.PERSON_NONE,
                                                                     TenseFormationType.HAAL_SAADEH, TensePositivity.POSITIVE);
            VerbShapes.Add("نمی‌توان", new List<VerbInflection>());
            VerbShapes["نمی‌توان"].Add(nemitavanInflection);

			VerbShapes.Add("میتوان", new List<VerbInflection>());
			VerbShapes["میتوان"].Add(mitavanInflection);
			VerbShapes.Add("نمیتوان", new List<VerbInflection>());
			VerbShapes["نمیتوان"].Add(nemitavanInflection);

            var betavanInflection = new VerbInflection(new Verb("", "", "توان", "", "", VerbTransitivity.InTransitive, VerbType.SADEH, false, "?", "@", "!"), AttachedPronounType.AttachedPronoun_NONE, "",
                                                                   PersonType.PERSON_NONE,
                                                                   TenseFormationType.HAAL_ELTEZAMI, TensePositivity.POSITIVE);
            VerbShapes.Add("بتوان", new List<VerbInflection>());
            VerbShapes["بتوان"].Add(betavanInflection);

            var naitavanInflection = new VerbInflection(new Verb("", "", "توان", "", "", VerbTransitivity.InTransitive, VerbType.SADEH, false, "?", "@", "!"), AttachedPronounType.AttachedPronoun_NONE, "",
                                                                     PersonType.PERSON_NONE,
                                                                     TenseFormationType.HAAL_ELTEZAMI, TensePositivity.POSITIVE);
            VerbShapes.Add("نتوان", new List<VerbInflection>());
            VerbShapes["نتوان"].Add(naitavanInflection);


            foreach (Verb verb in verbs)
            {
                if (verb.Type == VerbType.SADEH || verb.Type == VerbType.PISHVANDI || verb.Type == VerbType.AYANDEH_PISHVANDI)
                {
                    foreach (TensePositivity positivity in Enum.GetValues(typeof(TensePositivity)))
                    {
                        foreach (PersonType shakhsType in Enum.GetValues(typeof(PersonType)))
                        {
                            foreach (
                                TenseFormationType tenseFormationType in
                                    Enum.GetValues(typeof(TenseFormationType)))
                            {
                                foreach (
                                    AttachedPronounType zamirPeyvastehType in
                                        Enum.GetValues(typeof(AttachedPronounType)))
                                {

                                    var inflection = new VerbInflection(verb, zamirPeyvastehType, "",
                                                                        shakhsType,
                                                                        tenseFormationType, positivity);
                                    if (inflection.IsValid())
                                    {
                                        var output = InflectorAnalyzeSentencer.GetInflections(inflection);
                                        if (inflection.VerbRoot.PastTenseRoot == "بایست")
                                        {
                                            if (output[0].Contains("بایست"))
                                            {
                                                inflection.Person = PersonType.PERSON_NONE;
                                                inflection.TenseForm = TenseFormationType.GOZASHTEH_SADEH;
                                            }
                                            else
                                            {
                                                inflection.Person = PersonType.PERSON_NONE;
                                                inflection.TenseForm = TenseFormationType.HAAL_SAADEH;
                                            }
                                        }
										var output2=new List<string>();
										foreach (string list in output)
										{
											output2.Add (list);
											//Console.WriteLine(list);
											if (list.Contains("می‌")){
												var newshape=list.Replace("می‌","می");
												output2.Add (newshape);
												//Console.WriteLine(newshape);
											}
												
										}
										foreach (string list in output2)
                                        {
                                            if (!(VerbShapes.ContainsKey(list)))
                                            {
                                                var verbInflections = new List<VerbInflection> { inflection };
                                                VerbShapes.Add(list, verbInflections);
                                            }
                                            else
                                            {
                                                bool contains = false;
                                                foreach (VerbInflection inf in VerbShapes[list])
                                                {
                                                    if (inflection.Equals(inf))
                                                    {
                                                        contains = true;
                                                        break;
                                                    }
                                                }
                                                if (!contains)
                                                {
                                                    //This for zamir_peyvaste rule based disambiguation in which the inflections
                                                    //without zamir_peyvaste are rathered to remain
                                                    for (int i = 0; i < VerbShapes[list].Count; i++)
                                                    {
                                                        var verbInflection = VerbShapes[list][i];
                                                        if (verbInflection.ZamirPeyvasteh !=
                                                            AttachedPronounType.AttachedPronoun_NONE)
                                                        {
                                                            VerbShapes[list].Remove(verbInflection);
                                                        }

                                                    }
                                                    VerbShapes[list].Add(inflection);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
