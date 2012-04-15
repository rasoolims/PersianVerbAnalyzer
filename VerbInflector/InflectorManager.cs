using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VerbInflector
{
    /// <summary>
    /// Controls ways of building a verb string of a specified inflection
    /// </summary>
    public class InflectorAnalyzeSentencer
    {
        /// <summary>
        /// return the possible string representation of a specified verb inflection
        /// </summary>
        /// <param name="inflection">inflection</param>
        /// <returns>a list of string representations</returns>
        public static List<string> GetInflections(VerbInflection inflection)
        {
            var lstInflections = new List<string>();
            switch (inflection.TenseForm)
            {
                case TenseFormationType.AMR:
                    lstInflections = GetAmrInflections(inflection);
                    break;
                case TenseFormationType.GOZASHTEH_ESTEMRAARI:
                    lstInflections = GetGozashtehEstemrariInflections(inflection);
                    break;
                case TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI:
                    lstInflections = GetGozashtehNaghliEstemraiSadehInflections(inflection);
                    break;
                case TenseFormationType.GOZASHTEH_NAGHLI_SADEH:
                    lstInflections = GetGozashtehNaghliSadehInflections(inflection);
                    break;
                case TenseFormationType.GOZASHTEH_SADEH:
                    lstInflections = GetGozashtehSadehInflections(inflection);
                    break;
                case TenseFormationType.HAAL_ELTEZAMI:
                    lstInflections = GetHaalEltezamiInflections(inflection);
                    break;
                case TenseFormationType.HAAL_SAADEH:
                    lstInflections = GetHaalSaadehInflections(inflection);
                    break;
                case TenseFormationType.HAAL_SAADEH_EKHBARI:
                    lstInflections = GetHaalSaadehEkhbaariInflections(inflection);
                    break;
                case TenseFormationType.PAYEH_MAFOOLI:
                    lstInflections = GetPayehFelInflections(inflection);
                    break;
            }
            return lstInflections;
        }

        private static List<string> GetGozashtehNaghliSadehInflections(VerbInflection inflection)
        {
            var lstInflections = new List<string>();
            var verbInflection = new VerbInflection(inflection.VerbRoot, AttachedPronounType.AttachedPronoun_NONE,"",
                                                    PersonType.PERSON_NONE, TenseFormationType.PAYEH_MAFOOLI,
                                                    inflection.Positivity);
            var tempLst = GetPayehFelInflections(verbInflection);
            string fel = tempLst[0];
            switch (inflection.Person)
            {
                case PersonType.THIRD_PERSON_PLURAL:
                    fel += "‌اند";
                    break;
                case PersonType.SECOND_PERSON_SINGULAR:
                    fel += "‌ای";
                    break;
                case PersonType.SECOND_PERSON_PLURAL:
                    fel += "‌اید";
                    break;
                case PersonType.FIRST_PERSON_SINGULAR:
                    fel += "‌ام";
                    break;
                case PersonType.FIRST_PERSON_PLURAL:
                    fel += "‌ایم";
                    break;
            }
            lstInflections.Add(AddAttachedPronoun(fel, inflection));
            return lstInflections;
        }
        private static List<string> GetGozashtehNaghliEstemraiSadehInflections(VerbInflection inflection)
        {
            var lstInflections = new List<string>();
            var verbBuilder = new StringBuilder();
            verbBuilder.Append(inflection.VerbRoot.Prefix);
            switch (inflection.Positivity)
            {
                case TensePositivity.POSITIVE:
                    verbBuilder.Append("می‌" );
                    break;
                case TensePositivity.NEGATIVE:
                    verbBuilder.Append("نمی‌" );
                    break;
            }
            var verb = new Verb("", inflection.VerbRoot.PastTenseRoot,
                                 inflection.VerbRoot.PresentTenseRoot, "",
                                 "", inflection.VerbRoot.Transitivity, VerbType.SADEH,
                                 inflection.VerbRoot.CanBeImperative, inflection.VerbRoot.PresentRootConsonantVowelEndStem,inflection.VerbRoot.PastRootVowelStart,inflection.VerbRoot.PresentRootVowelStart);
            var verbInflection = new VerbInflection(verb, AttachedPronounType.AttachedPronoun_NONE,"",
                                                    PersonType.PERSON_NONE, TenseFormationType.PAYEH_MAFOOLI,
                                                    TensePositivity.POSITIVE);
            var tempLst = GetPayehFelInflections(verbInflection);
           verbBuilder.Append(tempLst[0]);
            switch (inflection.Person)
            {
                case PersonType.THIRD_PERSON_PLURAL:
                     verbBuilder.Append( "‌اند");
                    break;
                case PersonType.SECOND_PERSON_SINGULAR:
                    verbBuilder.Append("‌ای");
                    break;
                case PersonType.SECOND_PERSON_PLURAL:
                     verbBuilder.Append( "‌اید");
                    break;
                case PersonType.FIRST_PERSON_SINGULAR:
                     verbBuilder.Append("‌ام");
                    break;
                case PersonType.FIRST_PERSON_PLURAL:
                     verbBuilder.Append( "‌ایم");
                    break;
            }
            lstInflections.Add(AddAttachedPronoun(verbBuilder.ToString(), inflection));
            return lstInflections;
        }
        private static List<string> GetGozashtehEstemrariInflections(VerbInflection inflection)
        {
            var lstInflections = new List<string>();
            var verbBuilder = new StringBuilder();
            verbBuilder.Append(inflection.VerbRoot.Prefix); 
            switch (inflection.Positivity)
            {
                case TensePositivity.POSITIVE:
                    verbBuilder.Append("می‌" + inflection.VerbRoot.PastTenseRoot);
                    break;
                case TensePositivity.NEGATIVE:
                    verbBuilder.Append("نمی‌" + inflection.VerbRoot.PastTenseRoot);
                    break;
            }
            if (inflection.VerbRoot.PastTenseRoot.EndsWith("آ"))
            {
                verbBuilder.Remove(verbBuilder.Length - 1, 1);
                verbBuilder.Append("ی");
            }
            else if (inflection.VerbRoot.PastTenseRoot.EndsWith("ا") || inflection.VerbRoot.PastTenseRoot.EndsWith("و"))
            {
                verbBuilder.Append("ی");
            }
            switch (inflection.Person)
            {
                case PersonType.FIRST_PERSON_PLURAL:
                    verbBuilder.Append("یم");
                    break;
                case PersonType.FIRST_PERSON_SINGULAR:
                    verbBuilder.Append("م");
                    break;
                case PersonType.SECOND_PERSON_PLURAL:
                    verbBuilder.Append("ید");
                    break;
                case PersonType.SECOND_PERSON_SINGULAR:
                    verbBuilder.Append("ی");
                    break;
                case PersonType.THIRD_PERSON_PLURAL:
                    verbBuilder.Append("ند");
                    break;
            }
            lstInflections.Add(AddAttachedPronoun(verbBuilder.ToString(), inflection));
            return lstInflections;
        }
        private static List<string> GetGozashtehSadehInflections(VerbInflection inflection)
           {
               var lstInflections = new List<string>();
               var verbBuilder = new StringBuilder();
               verbBuilder.Append(inflection.VerbRoot.Prefix); 
            if (inflection.Positivity == TensePositivity.NEGATIVE)
               {
                   verbBuilder.Append("ن");
               }

               if (inflection.VerbRoot.PastRootVowelStart=="A" && inflection.Positivity == TensePositivity.NEGATIVE)
               {
                   if (!inflection.VerbRoot.PastTenseRoot.StartsWith("آ"))
                   verbBuilder.Append("ی");
                   else
                   verbBuilder.Append("یا");
                       verbBuilder.Append(inflection.VerbRoot.PastTenseRoot.Remove(0, 1));
               }
            
               else
               {
                   verbBuilder.Append(inflection.VerbRoot.PastTenseRoot);
               }

               if (inflection.VerbRoot.PastTenseRoot.EndsWith("آ"))
               {
                   verbBuilder.Remove(verbBuilder.Length - 1, 1);
                   verbBuilder.Append("ی");
               }
               else if (inflection.VerbRoot.PastTenseRoot.EndsWith("ا") || inflection.VerbRoot.PastTenseRoot.EndsWith("و"))
               {
                   verbBuilder.Append("ی");
               }
               switch (inflection.Person)
               {
                   case PersonType.FIRST_PERSON_PLURAL:
                       verbBuilder.Append("یم");
                       break;
                   case PersonType.FIRST_PERSON_SINGULAR:
                       verbBuilder.Append("م");
                       break;
                   case PersonType.SECOND_PERSON_PLURAL:
                       verbBuilder.Append("ید");
                       break;
                   case PersonType.SECOND_PERSON_SINGULAR:
                       verbBuilder.Append("ی");
                       break;
                   case PersonType.THIRD_PERSON_PLURAL:
                       verbBuilder.Append("ند");
                       break;
               }
               lstInflections.Add(AddAttachedPronoun(verbBuilder.ToString(), inflection));
               return lstInflections;
           }
        private static List<string> GetHaalSaadehEkhbaariInflections(VerbInflection inflection)
        {
            var lstInflections = new List<string>();
            var verbBuilder = new StringBuilder();
            verbBuilder.Append(inflection.VerbRoot.Prefix);
            if(inflection.VerbRoot.PresentTenseRoot == "است")
            {
                verbBuilder.Append(inflection.VerbRoot.PresentTenseRoot);
                lstInflections.Add(verbBuilder.ToString());
                return lstInflections;
            }
            if (inflection.VerbRoot.PresentTenseRoot == "هست")
            {
                switch (inflection.Positivity)
                {
                    case TensePositivity.POSITIVE:
                        verbBuilder.Append(inflection.VerbRoot.PresentTenseRoot);
                        break;
                    case TensePositivity.NEGATIVE:
                            verbBuilder.Append("نیست");
                        break;
                }
            }
            else
            {
                switch (inflection.Positivity)
                {
                    case TensePositivity.POSITIVE:
                        verbBuilder.Append("می‌" + inflection.VerbRoot.PresentTenseRoot);
                        break;
                    case TensePositivity.NEGATIVE:
                        verbBuilder.Append("نمی‌" + inflection.VerbRoot.PresentTenseRoot);
                        break;
                }
            }
            if (inflection.VerbRoot.PresentRootConsonantVowelEndStem == "A")
            {
                if (inflection.VerbRoot.PresentTenseRoot.Length>1)
                {
                    verbBuilder.Remove(verbBuilder.Length - 1, 1);
                    verbBuilder.Append("ای");
                }
                else
                    verbBuilder.Append("ی");
            }
            else if (inflection.VerbRoot.PresentRootConsonantVowelEndStem != "?")
            {
                verbBuilder.Append("ی");
            }
            switch (inflection.Person)
            {
                case PersonType.FIRST_PERSON_PLURAL:
                    verbBuilder.Append("یم");
                    break;
                case PersonType.FIRST_PERSON_SINGULAR:
                    verbBuilder.Append("م");
                    break;
                case PersonType.SECOND_PERSON_PLURAL:
                    verbBuilder.Append("ید");
                    break;
                case PersonType.SECOND_PERSON_SINGULAR:
                    verbBuilder.Append("ی");
                    break;
                case PersonType.THIRD_PERSON_PLURAL:
                    verbBuilder.Append("ند");
                    break;
                case PersonType.THIRD_PERSON_SINGULAR:
                    if (inflection.VerbRoot.PresentTenseRoot != "باید" && inflection.VerbRoot.PresentTenseRoot != "هست")
                    verbBuilder.Append("د");
                    break;
            }
            lstInflections.Add(AddAttachedPronoun(verbBuilder.ToString(), inflection));
            return lstInflections;
        }
        private static List<string> GetHaalEltezamiInflections(VerbInflection inflection)
        {
            var lstInflections = new List<string>();
            var verbBuilder = new StringBuilder();
            var verbBuilder2 = new StringBuilder();
            var verbBuilder3 = new StringBuilder();
            bool thirdInflec = false;
            verbBuilder.Append(inflection.VerbRoot.Prefix);
            verbBuilder2.Append(inflection.VerbRoot.Prefix); 
            
                switch (inflection.Positivity)
                {
                    case TensePositivity.POSITIVE:
                        if (!(inflection.VerbRoot.PresentTenseRoot == "باشد" || inflection.VerbRoot.PresentTenseRoot == "باید"))
                            verbBuilder.Append("ب");
                        break;
                    case TensePositivity.NEGATIVE:
                        verbBuilder.Append("ن");
                        break;
                }
                if (inflection.VerbRoot.PresentRootVowelStart=="A")
                {
                    if (inflection.VerbRoot.PresentTenseRoot.StartsWith("آ"))
                    {
                        verbBuilder.Append("یا");
                        verbBuilder.Append(inflection.VerbRoot.PresentTenseRoot.Remove(0, 1));
                        verbBuilder2.Append(inflection.VerbRoot.PresentTenseRoot);
                    }
                    else
                    {
                        thirdInflec = true;
                        verbBuilder3.Append(verbBuilder.ToString());
                        verbBuilder.Append("ی");
                        verbBuilder3.Append("یا");
                       
                        verbBuilder.Append(inflection.VerbRoot.PresentTenseRoot.Remove(0, 1));
                        verbBuilder3.Append(inflection.VerbRoot.PresentTenseRoot.Remove(0, 1));
                        verbBuilder2.Append(inflection.VerbRoot.PresentTenseRoot);
                    }
                }
                else
                {
                    verbBuilder.Append(inflection.VerbRoot.PresentTenseRoot);
                    verbBuilder2.Append(inflection.VerbRoot.PresentTenseRoot);
                }

            if (inflection.VerbRoot.PresentRootConsonantVowelEndStem=="A")
            {
                if (verbBuilder.Length > 1)
                {
                    verbBuilder.Remove(verbBuilder.Length - 1, 1);
                    verbBuilder.Append("ای");
                    if (thirdInflec)
                    {
                        verbBuilder3.Remove(verbBuilder3.Length - 1, 1);
                        verbBuilder3.Append("ای");
                    }
                    if (inflection.VerbRoot.PresentTenseRoot.Length > 1)
                    {
                        verbBuilder2.Remove(verbBuilder2.Length - 1, 1);
                        verbBuilder2.Append("ای");
                    }
                    else
                    {
                        verbBuilder2.Append("ی");
                    }
                }
                else
                {
                    verbBuilder.Append("ی");
                    verbBuilder3.Append("ی");
                    verbBuilder2.Append("ی");
                }
            }
            else if (inflection.VerbRoot.PresentRootConsonantVowelEndStem != "?")
            {
                if (inflection.VerbRoot.PastTenseRoot != "رفت" && inflection.VerbRoot.PastTenseRoot != "شد")
                {
                   verbBuilder.Append("ی");
                   verbBuilder3.Append("ی");
                   verbBuilder2.Append("ی");
                }
            }
            switch (inflection.Person)
            {
                case PersonType.FIRST_PERSON_PLURAL:
                    verbBuilder.Append("یم");
                    verbBuilder3.Append("یم");
                    verbBuilder2.Append("یم");
                    break;
                case PersonType.FIRST_PERSON_SINGULAR:
                    verbBuilder.Append("م");
                    verbBuilder3.Append("م");
                    verbBuilder2.Append("م");
                    break;
                case PersonType.SECOND_PERSON_PLURAL:
                    verbBuilder.Append("ید");
             verbBuilder3.Append("ید");
             verbBuilder2.Append("ید");
                    break;
                case PersonType.SECOND_PERSON_SINGULAR:
                    verbBuilder.Append("ی");
                    verbBuilder3.Append("ی");
                    verbBuilder2.Append("ی");
                    break;
                case PersonType.THIRD_PERSON_PLURAL:
                    verbBuilder.Append("ند");
                    verbBuilder3.Append("ند");
                    verbBuilder2.Append("ند");
                    break;
                case PersonType.THIRD_PERSON_SINGULAR:
                    verbBuilder.Append("د");
                    verbBuilder3.Append("د");
                    verbBuilder2.Append("د");
                    break;
            }
            lstInflections.Add(AddAttachedPronoun(verbBuilder.ToString(), inflection));
            if(thirdInflec)
            lstInflections.Add(AddAttachedPronoun(verbBuilder3.ToString(), inflection));
            if (inflection.Positivity == TensePositivity.POSITIVE && (inflection.VerbRoot.PresentTenseRoot.Length >= 2 || inflection.VerbRoot.Type == VerbType.PISHVANDI))
                lstInflections.Add(AddAttachedPronoun(verbBuilder2.ToString(), inflection));
            return lstInflections;
        }
        private static List<string> GetHaalSaadehInflections(VerbInflection inflection)
        {
            var lstInflections = new List<string>();
            if (inflection.VerbRoot.PastTenseRoot == "خواست"  || inflection.VerbRoot.PastTenseRoot == "خواست" || inflection.VerbRoot.PastTenseRoot == "داشت" || inflection.VerbRoot.PastTenseRoot == "بایست"|| inflection.VerbRoot.Type==VerbType.AYANDEH_PISHVANDI)
            {
                var verbBuilder = new StringBuilder();
                verbBuilder.Append(inflection.VerbRoot.Prefix);
                if (inflection.Positivity==TensePositivity.NEGATIVE)
                {
                        verbBuilder.Append("ن");
                }
                verbBuilder.Append(inflection.VerbRoot.PresentTenseRoot);
                
                if (inflection.VerbRoot.PresentRootConsonantVowelEndStem == "A")
                {
                    if (verbBuilder.Length > 1)
                    {
                        verbBuilder.Remove(verbBuilder.Length - 1, 1);
                        verbBuilder.Append("ای");
                    }
                    else
                        verbBuilder.Append("ی");
                }
                else if (inflection.VerbRoot.PresentRootConsonantVowelEndStem != "?")
                {
                    verbBuilder.Append("ی");
                }
                switch (inflection.Person)
                {
                    case PersonType.FIRST_PERSON_PLURAL:
                        verbBuilder.Append("یم");
                        break;
                    case PersonType.FIRST_PERSON_SINGULAR:
                        verbBuilder.Append("م");
                        break;
                    case PersonType.SECOND_PERSON_PLURAL:
                        verbBuilder.Append("ید");
                        break;
                    case PersonType.SECOND_PERSON_SINGULAR:
                        verbBuilder.Append("ی");
                        break;
                    case PersonType.THIRD_PERSON_PLURAL:
                        verbBuilder.Append("ند");
                        break;
                    case PersonType.THIRD_PERSON_SINGULAR:
                        if (inflection.VerbRoot.PastTenseRoot != "بایست")
                        verbBuilder.Append("د");
                        break;
                }
                lstInflections.Add(AddAttachedPronoun(verbBuilder.ToString(), inflection));
            }
            return lstInflections;
        }
        private static List<string> GetAmrInflections(VerbInflection inflection)
        {
            var lstInflections = new List<string>();
            var verbBuilder1 = new StringBuilder();
            var verbBuilder2 = new StringBuilder();
            var verbBuilder3 = new StringBuilder();
            var verbBuilder4 = new StringBuilder();
            bool fourthInflec = false;
            if (inflection.VerbRoot.Prefix != "")
            {
                verbBuilder1.Append(inflection.VerbRoot.Prefix);
                if (inflection.Positivity == TensePositivity.NEGATIVE)
                {
                    verbBuilder3.Append(inflection.VerbRoot.Prefix);
                }
                if (inflection.Positivity == TensePositivity.POSITIVE)
                {
                    verbBuilder2.Append(inflection.VerbRoot.Prefix);                    
                }
            }
            switch (inflection.Positivity)
            {
                case TensePositivity.POSITIVE:
                    if (!(inflection.VerbRoot.PresentTenseRoot == "باش" || inflection.VerbRoot.PresentTenseRoot == "باید"))
                        verbBuilder1.Append("ب");
                    break;
                case TensePositivity.NEGATIVE:
                    verbBuilder1.Append("ن");
                    verbBuilder3.Append("م");
                    break;
            }
            if (inflection.VerbRoot.PresentRootVowelStart == "A")
            {
                if (inflection.VerbRoot.PresentTenseRoot.StartsWith("آ"))
                {
                    verbBuilder1.Append("یا");
                    verbBuilder1.Append(inflection.VerbRoot.PresentTenseRoot.Remove(0, 1));
                    if (inflection.Positivity == TensePositivity.NEGATIVE)
                    {
                        verbBuilder3.Append("یا");
                        verbBuilder3.Append(inflection.VerbRoot.PresentTenseRoot.Remove(0, 1));
                    }
                    if (inflection.Positivity == TensePositivity.POSITIVE)
                    {
                        verbBuilder2.Append(inflection.VerbRoot.PresentTenseRoot);
                    }
                }
                else
                {
                    fourthInflec = true;
                    verbBuilder4.Append(verbBuilder1.ToString());
                    verbBuilder1.Append("ی");
                    verbBuilder4.Append("یا");
                    verbBuilder1.Append(inflection.VerbRoot.PresentTenseRoot.Remove(0, 1));
                    verbBuilder4.Append(inflection.VerbRoot.PresentTenseRoot.Remove(0, 1));
                    if (inflection.Positivity == TensePositivity.NEGATIVE)
                    {
                        verbBuilder3.Append("ی");
                        verbBuilder3.Append(inflection.VerbRoot.PresentTenseRoot.Remove(0, 1));
                    }
                    if (inflection.Positivity == TensePositivity.POSITIVE)
                    {
                        verbBuilder2.Append(inflection.VerbRoot.PresentTenseRoot);
                    }
                }
            }
            else
            {
                verbBuilder1.Append(inflection.VerbRoot.PresentTenseRoot);
                if (inflection.Positivity == TensePositivity.POSITIVE/* && inflection.VerbStem.Type==VerbType.PISHVANDI*/)
                {
                    verbBuilder2.Append(inflection.VerbRoot.PresentTenseRoot);
                }
                if (inflection.Positivity == TensePositivity.NEGATIVE)
                {
                    verbBuilder3.Append(inflection.VerbRoot.PresentTenseRoot);
                }
            }

            switch (inflection.Person)
            {
                case PersonType.SECOND_PERSON_PLURAL:
                    if (inflection.VerbRoot.PresentRootConsonantVowelEndStem!="?")
                    {
                        verbBuilder1.Append("یید");
                        verbBuilder4.Append("یید");
                        if (inflection.Positivity == TensePositivity.NEGATIVE)
                        {
                            verbBuilder3.Append("یید");
                        }
                        if (inflection.Positivity == TensePositivity.POSITIVE && inflection.VerbRoot.Type==VerbType.PISHVANDI)
                        {
                            verbBuilder2.Append("یید");
                        }
                    }
                    else
                    {
                        verbBuilder1.Append("ید");
                        verbBuilder4.Append("ید");
                        if (inflection.Positivity == TensePositivity.NEGATIVE)
                        {
                            verbBuilder3.Append("ید");
                        }
                        if (inflection.Positivity == TensePositivity.POSITIVE && inflection.VerbRoot.Type == VerbType.PISHVANDI)
                        {
                            verbBuilder2.Append("ید");
                        }
                    }
                    break;
            }
            if (inflection.ZamirPeyvasteh == AttachedPronounType.AttachedPronoun_NONE)
            {
                if (!(inflection.VerbRoot.PresentTenseRoot == "نه" && inflection.Positivity == TensePositivity.NEGATIVE))
                    lstInflections.Add(verbBuilder1.ToString());
                if(fourthInflec)
                    lstInflections.Add(verbBuilder4.ToString());
                if (inflection.Positivity == TensePositivity.NEGATIVE)
                    lstInflections.Add(verbBuilder3.ToString());
                if (inflection.Positivity == TensePositivity.POSITIVE && (inflection.VerbRoot.Type == VerbType.PISHVANDI || inflection.VerbRoot.PastTenseRoot=="کرد"  || inflection.VerbRoot.PastTenseRoot=="نمود" || inflection.VerbRoot.PastTenseRoot=="فرمود")) 
                    lstInflections.Add(verbBuilder2.ToString());
                if (inflection.VerbRoot.Type == VerbType.PISHVANDI && inflection.Person == PersonType.SECOND_PERSON_SINGULAR && inflection.Positivity == TensePositivity.POSITIVE &&
                    inflection.VerbRoot.PresentRootConsonantVowelEndStem != "?")
                {
                    lstInflections.Add(verbBuilder2.Append("ی").ToString());
                }
            }
            else
            {
                if (!(inflection.VerbRoot.PresentTenseRoot == "نه" && inflection.Positivity == TensePositivity.NEGATIVE))
                    lstInflections.Add(AddAttachedPronoun(verbBuilder1.ToString(), inflection));
                if (fourthInflec)
                    lstInflections.Add(AddAttachedPronoun(verbBuilder4.ToString(), inflection)); 
                if (inflection.Positivity == TensePositivity.NEGATIVE)
                    lstInflections.Add(AddAttachedPronoun(verbBuilder3.ToString(),inflection));
                if (inflection.VerbRoot.Type == VerbType.PISHVANDI && inflection.Positivity == TensePositivity.POSITIVE)
                    lstInflections.Add(AddAttachedPronoun(verbBuilder2.ToString(), inflection));
                if (inflection.VerbRoot.Type == VerbType.PISHVANDI && inflection.Person==PersonType.SECOND_PERSON_SINGULAR && inflection.Positivity == TensePositivity.POSITIVE &&
                    inflection.VerbRoot.PresentRootConsonantVowelEndStem != "?")
                {
                    lstInflections.Add(AddAttachedPronoun(verbBuilder2.Append("ی").ToString(), inflection));
                }
            }
            return lstInflections;
        }
        private static List<string> GetPayehFelInflections(VerbInflection inflection)
        {
            var lstInflections = new List<string>();
            switch (inflection.Positivity)
            {
                case TensePositivity.POSITIVE:
                    lstInflections.Add(inflection.VerbRoot.Prefix+ inflection.VerbRoot.PastTenseRoot + "ه");
                    break;
                case TensePositivity.NEGATIVE:
                    if (inflection.VerbRoot.PastRootVowelStart == "A" && inflection.Positivity == TensePositivity.NEGATIVE)
                    {
                        var verbBuilder = new StringBuilder();
                        verbBuilder.Append(inflection.VerbRoot.Prefix + "ن");
                        if (!inflection.VerbRoot.PastTenseRoot.StartsWith("آ"))
                            verbBuilder.Append("ی");
                        else
                            verbBuilder.Append("یا");
                        verbBuilder.Append(inflection.VerbRoot.PastTenseRoot.Remove(0, 1));
                        verbBuilder.Append("ه");
                        lstInflections.Add(verbBuilder.ToString());                        

                    }
                    else
                    {
                        lstInflections.Add(inflection.VerbRoot.Prefix + "ن" + inflection.VerbRoot.PastTenseRoot + "ه");                        
                    }
                    break;
            }
            return lstInflections;
        }
        private static string AddAttachedPronoun(string verb, VerbInflection inflection)
        {
            string inflectedVerb = verb;
            switch (inflection.ZamirPeyvasteh)
            {
                case AttachedPronounType.THIRD_PERSON_SINGULAR:
                    if (verb.EndsWith("آ") || verb.EndsWith("ا") || verb.EndsWith("و"))
                    {
                        inflection.AttachedPronounString = "یش";
                        inflectedVerb += "یش";
                    }
                    else if (verb.EndsWith("ه") && !verb.EndsWith("اه") && !verb.EndsWith("وه"))
                    {
                        inflection.AttachedPronounString = "‌اش";
                        inflectedVerb += "‌اش";
                    }
                    else if (verb.EndsWith("ی") && !verb.EndsWith("ای") && !verb.EndsWith("وی"))
                    {
                        inflection.AttachedPronounString = "‌اش";
                        inflectedVerb += "‌اش";
                    }
                    else if (verb.EndsWith("‌ای"))
                    {
                        inflection.AttachedPronounString = "‌اش";
                        inflectedVerb += "‌اش";
                    }
                    else
                    {
                        inflection.AttachedPronounString = "ش";
                        inflectedVerb += "ش";
                    }
                    break;
                case AttachedPronounType.THIRD_PERSON_PLURAL:
                    if (verb.EndsWith("آ") || verb.EndsWith("ا") || verb.EndsWith("و"))
                    {
                        inflection.AttachedPronounString = "یشان";
                            inflectedVerb += "یشان";
                    }
                    else if (verb.EndsWith("ه") && !verb.EndsWith("اه") && !verb.EndsWith("وه"))
                    {
                        inflection.AttachedPronounString = "‌شان";
                        inflectedVerb += "‌شان";
                    }
                    else
                    {
                        inflection.AttachedPronounString = "شان";
                        inflectedVerb += "شان";
                    }
                    break;
                case AttachedPronounType.SECOND_PERSON_PLURAL:
                    if (verb.EndsWith("آ") || verb.EndsWith("ا") || verb.EndsWith("و"))
                    {
                        inflection.AttachedPronounString = "یتان";
                        inflectedVerb += "یتان";
                    }
                    else if (verb.EndsWith("ه") && !verb.EndsWith("اه") && !verb.EndsWith("وه"))
                    {
                        inflection.AttachedPronounString = "‌تان";
                        inflectedVerb += "‌تان";
                    }
                   else
                    {
                        inflection.AttachedPronounString = "تان";
                        inflectedVerb += "تان";
                    } break;
                case AttachedPronounType.SECOND_PERSON_SINGULAR:
                    if (verb.EndsWith("آ") || verb.EndsWith("ا") || verb.EndsWith("و"))
                    {
                        inflection.AttachedPronounString = "یت";
                        inflectedVerb += "یت";
                    }
                    else if (verb.EndsWith("ه") && !verb.EndsWith("اه") && !verb.EndsWith("وه"))
                    {
                        inflection.AttachedPronounString = "‌ات";
                        inflectedVerb += "‌ات";
                    }
                    else if (verb.EndsWith("ی") && !verb.EndsWith("ای") && !verb.EndsWith("وی"))
                    {
                        inflection.AttachedPronounString = "‌ات";
                        inflectedVerb += "‌ات";
                    }
                    else if (verb.EndsWith("‌ای"))
                    {
                        inflection.AttachedPronounString = "‌ات";
                        inflectedVerb += "‌ات";
                    }
                    else
                    {
                        inflection.AttachedPronounString = "ت";
                        inflectedVerb += "ت";
                    }
                    break;
                case AttachedPronounType.FIRST_PERSON_PLURAL:
                    if (verb.EndsWith("آ") || verb.EndsWith("ا") || verb.EndsWith("و"))
                    {
                        inflection.AttachedPronounString = "یمان";
                        inflectedVerb += "یمان";
                    }
                    else if (verb.EndsWith("ه") && !verb.EndsWith("اه") && !verb.EndsWith("وه"))
                    {
                        inflection.AttachedPronounString = "‌مان";
                        inflectedVerb += "‌مان";
                    }
                    else
                    {
                        inflection.AttachedPronounString = "مان";
                        inflectedVerb += "مان";
                    }
                    break;
                case AttachedPronounType.FIRST_PERSON_SINGULAR:
                    if (verb.EndsWith("آ") || verb.EndsWith("ا") || verb.EndsWith("و"))
                    {
                        inflection.AttachedPronounString = "یم";
                        inflectedVerb += "یم";
                    }
                    else if (verb.EndsWith("ه") && !verb.EndsWith("اه") && !verb.EndsWith("وه"))
                    {
                        inflection.AttachedPronounString = "‌ام";
                        inflectedVerb += "‌ام";
                    }
                    else if (verb.EndsWith("ی") && !verb.EndsWith("ای") && !verb.EndsWith("وی"))
                    {
                        inflection.AttachedPronounString = "‌ام";
                        inflectedVerb += "‌ام";
                    }
                    else if (verb.EndsWith("‌ای"))
                    {
                        inflection.AttachedPronounString = "‌ام";
                        inflectedVerb += "‌ام";
                    }
                    else
                    {
                        inflection.AttachedPronounString = "م";
                        inflectedVerb += "م";
                    } 
                    break;
            }
            return inflectedVerb;
        }
    }
}
