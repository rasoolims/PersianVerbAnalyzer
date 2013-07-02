using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VerbInflector
{
    [Flags]
    public enum AttachedPronounType
    {
        AttachedPronoun_NONE = 1,
        FIRST_PERSON_SINGULAR = 2,
        SECOND_PERSON_SINGULAR = 4,
        THIRD_PERSON_SINGULAR = 8,
        FIRST_PERSON_PLURAL = 16,
        SECOND_PERSON_PLURAL = 32,
        THIRD_PERSON_PLURAL = 64
    }
  
    [Flags]
    public enum PersonType
    {
        PERSON_NONE = 1,
        FIRST_PERSON_SINGULAR = 2,
        SECOND_PERSON_SINGULAR = 4,
        THIRD_PERSON_SINGULAR = 8,
        FIRST_PERSON_PLURAL = 16,
        SECOND_PERSON_PLURAL = 32,
        THIRD_PERSON_PLURAL = 64
    }
  
    [Flags]
    public enum TenseFormationType
    {
        TenseFormationType_NONE = 0,
        HAAL_SAADEH_EKHBARI = 1,
        HAAL_ELTEZAMI = 2,
        HAAL_SAADEH = 4,
        AMR = 8,
        GOZASHTEH_SADEH = 32,
        GOZASHTEH_ESTEMRAARI = 64,
        GOZASHTEH_NAGHLI_SADEH = 128,
        GOZASHTEH_NAGHLI_ESTEMRAARI = 256,
        GOZASHTEH_BAEED = 512,
        GOZASHTEH_ELTEZAMI = 1024,
        PAYEH_MAFOOLI = 2048,
        AAYANDEH = 4096,
        GOZASHTEH_ABAD = 8192
    }
   
    [Flags]
    public enum TensePositivity
    {
        POSITIVE = 1,
        NEGATIVE = 2
    }
  
    [Flags]
    public enum TensePassivity
    {
        ACTIVE = 1,
        PASSIVE = 2
    }

    [Flags]
    public enum VerbType
    {
        SADEH = 1,
        PISHVANDI = 2,
        MORAKKAB = 4,
        MORAKKABPISHVANDI = 8,
        MORAKKABHARFE_EZAFEH = 16,
        EBAARATFELI = 32,
        LAZEM_TAKFELI = 64,
        AYANDEH_PISHVANDI = 128
    }

    [Flags]
    public enum VerbTransitivity
    {
        Transitive = 1,
        InTransitive = 2,
        BiTransitive = 4
    }


}
