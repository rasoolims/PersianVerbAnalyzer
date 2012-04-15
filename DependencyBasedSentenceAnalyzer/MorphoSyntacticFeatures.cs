using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerbInflector;

namespace DependencyBasedSentenceAnalyzer
{
    public class MorphoSyntacticFeatures
    {
        public MorphoSyntacticFeatures(NumberType num, PersonType pers, TenseFormationType tma)
        {
            Number = num;
            Person = pers;
            TenseMoodAspect = tma;
        }

		public MorphoSyntacticFeatures(String num, String pers, String tma)
		{
			Number = this.StringToNumber(num);
			Person = this.StringToPerson(pers);
			TenseMoodAspect = this.StringToTMA(tma);
		}

		private TenseFormationType StringToTMA(string tma)
		{
			switch (tma)
			{
				case "_":
					return TenseFormationType.TenseFormationType_NONE;
				case "HAAL_SAADEH_EKHBARI":
					return TenseFormationType.HAAL_SAADEH_EKHBARI;
				case "HAAL_ELTEZAMI":
					return TenseFormationType.HAAL_ELTEZAMI;
				case "HAAL_SAADEH":
					return TenseFormationType.HAAL_SAADEH;
				case "AMR":
					return TenseFormationType.AMR;
				case "GOZASHTEH_SADEH":
					return TenseFormationType.GOZASHTEH_SADEH;
				case "GOZASHTEH_ESTEMRAARI":
					return TenseFormationType.GOZASHTEH_ESTEMRAARI;
				case "GOZASHTEH_NAGHLI_SADEH":
					return TenseFormationType.GOZASHTEH_NAGHLI_SADEH;
				case "GOZASHTEH_NAGHLI_ESTEMRAARI":
					return TenseFormationType.GOZASHTEH_NAGHLI_ESTEMRAARI;
				case "GOZASHTEH_BAEED":
					return TenseFormationType.GOZASHTEH_BAEED;
				case "GOZASHTEH_ELTEZAMI":
					return TenseFormationType.GOZASHTEH_ELTEZAMI;
				case "PAYEH_MAFOOLI":
					return TenseFormationType.PAYEH_MAFOOLI;
				case "AAYANDEH":
					return TenseFormationType.AAYANDEH;
				case "GOZASHTEH_ABAD":
					return TenseFormationType.GOZASHTEH_ABAD;
				default:
					return TenseFormationType.TenseFormationType_NONE;
			}
		}

		private PersonType StringToPerson(string pers)
		{
			switch (pers)
			{
				case "_":
					return PersonType.PERSON_NONE;
				case "AVALSHAKHS_MOFRAD":
					return PersonType.FIRST_PERSON_SINGULAR;
				case "DOVVOMSHAKHS_MOFRAD":
					return PersonType.SECOND_PERSON_SINGULAR;
				case "SEVVOMSHAKHS_MOFRAD":
					return PersonType.THIRD_PERSON_SINGULAR;
				case "AVALSHAKHS_JAM":
					return PersonType.FIRST_PERSON_PLURAL;
				case "DOVVOMSHAKHS_JAM":
					return PersonType.SECOND_PERSON_PLURAL;
				case "SEVVOMSHAKHS_JAM":
					return PersonType.THIRD_PERSON_PLURAL;
				default:
					return PersonType.PERSON_NONE;
			}
		}

		private NumberType StringToNumber(string num)
		{
			switch (num)
			{
				case "_":
					return NumberType.INVALID;
				case "SINGULAR":
					return NumberType.SINGULAR;
				case "PLURAL":
					return NumberType.PLURAL;
				default:
					return NumberType.INVALID;
			}
		}

        public NumberType Number { set; get; }
        public PersonType Person { set; get; }
        public TenseFormationType TenseMoodAspect { set; get; }

        public MorphoSyntacticFeatures Clone()
        {
            return new MorphoSyntacticFeatures(Number, Person, TenseMoodAspect);
        }
    }
}
