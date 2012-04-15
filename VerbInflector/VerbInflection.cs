using System;

namespace VerbInflector
{
    /// <summary>
    /// Represents an instance of an inflected verb
    /// </summary>
    public class VerbInflection:IComparable
    {
        /// <summary>
        /// verb root
        /// </summary>
        public Verb VerbRoot;

        /// <summary>
        /// Type of the attached pronoun
        /// </summary>
        public AttachedPronounType ZamirPeyvasteh;

        /// <summary>
        /// String representation of the attached pronoun
        /// </summary>
        public string AttachedPronounString;

        /// <summary>
        /// Person type of the verb inflection
        /// </summary>
        public PersonType Person;

        /// <summary>
        /// Represents the formation of the tense
        /// </summary>
        public TenseFormationType TenseForm;

        /// <summary>
        /// Positive/Negative tense
        /// </summary>
        public TensePositivity Positivity;

        /// <summary>
        /// Active/Passive tense
        /// </summary>
        public TensePassivity Passivity;

        public VerbInflection(Verb vrb, AttachedPronounType zamirType,string zamirString, PersonType shakhstype, TenseFormationType tenseFormationType, TensePositivity positivity)
        {
            VerbRoot = vrb;
            ZamirPeyvasteh = zamirType;
            AttachedPronounString = zamirString;
            Person = shakhstype;
            TenseForm = tenseFormationType;
            Positivity = positivity;
            Passivity = TensePassivity.ACTIVE;
        }
    
        public VerbInflection(Verb vrb, AttachedPronounType zamir,string zamirPeyvastehString, PersonType shakhstype,TenseFormationType tenseFormationType,TensePositivity positivity, TensePassivity passivity)
        {
            VerbRoot = vrb;
            ZamirPeyvasteh = zamir;
            AttachedPronounString = zamirPeyvastehString;
            Person = shakhstype;
            TenseForm = tenseFormationType;
            Positivity = positivity;
            Passivity = passivity;
        }
     
        public bool IsPayehFelMasdari()
        {
            if(Person==PersonType.THIRD_PERSON_SINGULAR && TenseForm==TenseFormationType.GOZASHTEH_SADEH)//TODO 
                return true;
            return false;
        }
       
        /// <summary>
        /// Shows whether negative form is valid or not
        /// </summary>
        /// <returns></returns>
        private bool IsNegativeFormValid()
        {
            return true;
        }

        /// <summary>
        /// Shows whether the mentioned person is valid or not
        /// </summary>
        /// <returns></returns>
        private bool IsPersonValid()
        {
            if (TenseForm==TenseFormationType.HAAL_SAADEH_EKHBARI &&  VerbRoot.PresentTenseRoot == "است" && Person!=PersonType.THIRD_PERSON_SINGULAR)
                return false; 
            if (TenseForm==TenseFormationType.PAYEH_MAFOOLI && Person!=PersonType.PERSON_NONE)
                return false;
            if (TenseForm!=TenseFormationType.PAYEH_MAFOOLI && Person==PersonType.PERSON_NONE)
                return false;
            if (TenseForm==TenseFormationType.GOZASHTEH_NAGHLI_SADEH && Person==PersonType.THIRD_PERSON_SINGULAR)
                return false;
            if (TenseForm==TenseFormationType.AMR &&
                !(Person==PersonType.SECOND_PERSON_PLURAL || Person==PersonType.SECOND_PERSON_SINGULAR))
                return false;
            return true;
        }

        /// <summary>
        /// checks whether the attached pronoun is valid or not
        /// </summary>
        /// <returns></returns>
        private bool IsAttachedPronounValid()
        {
          if (TenseForm==TenseFormationType.AMR && (ZamirPeyvasteh==AttachedPronounType.SECOND_PERSON_SINGULAR || ZamirPeyvasteh==AttachedPronounType.SECOND_PERSON_PLURAL || ZamirPeyvasteh==AttachedPronounType.FIRST_PERSON_PLURAL || ZamirPeyvasteh==AttachedPronounType.FIRST_PERSON_SINGULAR))
                return false;
          if(IsEqualPersonPronoun(ZamirPeyvasteh,Person))
              return false;
            if (ZamirPeyvasteh==AttachedPronounType.AttachedPronoun_NONE && TenseForm==TenseFormationType.PAYEH_MAFOOLI)
                return true;
            if(ZamirPeyvasteh==AttachedPronounType.AttachedPronoun_NONE)
                return true;
            return VerbRoot.IsZamirPeyvastehValid() && TenseForm!=TenseFormationType.PAYEH_MAFOOLI;
        }

        /// <summary>
        /// Checks whether two parameter persons are equal or not (it is common in Persian language
        /// that the verb with a special person type can not have a same attached pronoun person type)
        /// </summary>
        /// <param name="zamirPeyvastehType">Attached pronoun type</param>
        /// <param name="shakhsType">Person type</param>
        /// <returns>true if persons are equal</returns>
        private static bool IsEqualPersonPronoun(AttachedPronounType zamirPeyvastehType, PersonType shakhsType)
        {
            if (zamirPeyvastehType==AttachedPronounType.SECOND_PERSON_SINGULAR && shakhsType==PersonType.SECOND_PERSON_SINGULAR)
                return true;
            if (zamirPeyvastehType==AttachedPronounType.SECOND_PERSON_PLURAL && shakhsType==PersonType.SECOND_PERSON_PLURAL)
                return true;
            if (zamirPeyvastehType==AttachedPronounType.FIRST_PERSON_SINGULAR && shakhsType==PersonType.FIRST_PERSON_SINGULAR)
                return true;
            if (zamirPeyvastehType==AttachedPronounType.FIRST_PERSON_PLURAL && shakhsType==PersonType.FIRST_PERSON_PLURAL)
                return true;
            if (zamirPeyvastehType==AttachedPronounType.SECOND_PERSON_PLURAL && shakhsType==PersonType.SECOND_PERSON_SINGULAR)
                return true;
            if (zamirPeyvastehType==AttachedPronounType.SECOND_PERSON_SINGULAR && shakhsType==PersonType.SECOND_PERSON_PLURAL)
                return true;
            if (zamirPeyvastehType==AttachedPronounType.FIRST_PERSON_PLURAL && shakhsType==PersonType.FIRST_PERSON_SINGULAR)
                return true;
            if (zamirPeyvastehType==AttachedPronounType.FIRST_PERSON_SINGULAR && shakhsType==PersonType.FIRST_PERSON_PLURAL)
                return true;
            return false;
        }
      
        /// <summary>
        /// overrided method of ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return VerbRoot + "\t" + TenseForm + "\t" + Person + "\t" + ZamirPeyvasteh + "\t" + Positivity;
        }

        /// <summary>
        /// represent an abstract string representation of the verb discarding its root details
        /// </summary>
        /// <returns></returns>
        public string AbstarctString()
        {
            return TenseForm + "\t" + ZamirPeyvasteh;
        }

        /// <summary>
        /// Checks whether the inflection if valid in Persian or not
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if ((TenseForm==TenseFormationType.HAAL_ELTEZAMI || TenseForm==TenseFormationType.AMR || TenseForm==TenseFormationType.PAYEH_MAFOOLI) && (VerbRoot.PresentTenseRoot == "هست" || VerbRoot.PresentTenseRoot == "است"))
                return false;
            if (Positivity==TensePositivity.NEGATIVE && VerbRoot.PresentTenseRoot == "است")
                return false;
            if (TenseForm==TenseFormationType.HAAL_SAADEH_EKHBARI || TenseForm==TenseFormationType.HAAL_ELTEZAMI || TenseForm==TenseFormationType.AMR)
                if(string.IsNullOrEmpty(VerbRoot.PresentTenseRoot))
                    return false;
            if (TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI || TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI || TenseForm == TenseFormationType.GOZASHTEH_NAGHLI_SADEH || TenseForm == TenseFormationType.GOZASHTEH_SADEH || TenseForm == TenseFormationType.PAYEH_MAFOOLI)
                if (string.IsNullOrEmpty(VerbRoot.PastTenseRoot))
                    return false;
            if(TenseForm==TenseFormationType.AMR && VerbRoot.CanBeImperative==false)
                return false;
            if (TenseForm!=TenseFormationType.HAAL_SAADEH && VerbRoot.Type==VerbType.AYANDEH_PISHVANDI)
                return false;

            if(VerbRoot.PastTenseRoot=="بایست")
            {
                if (TenseForm == TenseFormationType.HAAL_SAADEH && Person == PersonType.THIRD_PERSON_SINGULAR)
                    return true;
                if (TenseForm == TenseFormationType.HAAL_SAADEH_EKHBARI && Person == PersonType.THIRD_PERSON_SINGULAR)
                    return true;
                if (TenseForm == TenseFormationType.GOZASHTEH_SADEH && (Person == PersonType.THIRD_PERSON_SINGULAR || Person == PersonType.SECOND_PERSON_SINGULAR))
                    return true;
                if (TenseForm == TenseFormationType.GOZASHTEH_ESTEMRAARI && (Person == PersonType.THIRD_PERSON_SINGULAR || Person == PersonType.SECOND_PERSON_SINGULAR))
                    return true;
                return false;
            }
            return (IsAttachedPronounValid() && IsPersonValid() && IsNegativeFormValid());
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            if (this.Equals(obj))
                return 0;
            else
                return this.GetHashCode().CompareTo(obj.GetHashCode());
        }
        public override bool Equals(object obj)
        {
            if (!(obj is VerbInflection))
                return false;
            var inflection = (VerbInflection) obj;
            if (inflection.VerbRoot.Equals(VerbRoot) && inflection.ZamirPeyvasteh == ZamirPeyvasteh && inflection.Person == Person && inflection.TenseForm == TenseForm && inflection.Positivity == Positivity)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return VerbRoot.GetHashCode() + ZamirPeyvasteh.GetHashCode() + Person.GetHashCode() +
                   TenseForm.GetHashCode() + Positivity.GetHashCode();
        }
        #endregion
    }
 

   
}
