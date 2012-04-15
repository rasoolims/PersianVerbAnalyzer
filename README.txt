In the name of Allah
-----------------------------------------------------------------------------------------------
The following package consists of a rule-based verb inflector in Persian developed by Mohammad Sadegh Rasooli (https://sites.google.com/site/rasoolims). The code was mainly used for preprocessing the Persian dependency treebank (http://dadegan.ir/en). 

<!> If you use this software in your research work, please cite to the following paper:
- Mohammad Rasooli, Heshaam Faili, and Behrouz Minaei-Bidgoli. "Unsupervised Identification of Persian Compound Verbs", in Proceedings of the 10th Mexican international conference on Advances in Artificial Intelligence - Volume Part I (LNCS 7094), Pages 394-406, Puebla, Mexico, 2011. 
In ACM library: http://dl.acm.org/citation.cfm?id=2178197.2178234&coll=DL&dl=GUIDE&CFID=97422849&CFTOKEN=39528935
In Springer library: http://www.springerlink.com/content/n3r0181wu2h6p337/

--------------------------------------------------------------------------------------
Please send the bugs and your questions to rasooli.ms{A#T}gmail.com

-----------------------------------------------------------------------------------------------------
* How to use the code

The code is compatible with C# 3.5 or upper versions.

There are two options for getting a verb analyzed sentence:
1)	Without part of speech tags (without disambiguation, considering all the words as potential verbs).
In SentenceAnalyzer.cs:
public static VerbBasedSentence MakeVerbBasedSentence(string sentence)

or

public static VerbBasedSentence MakeVerbBasedSentence(string[] sentence)

2)	With part of speech and morphosyntactic tags (with a good accuracy): the pos tags are the same as Bijankhan corpus (http://ece.ut.ac.ir/dbrg/bijankhan/)
public static VerbBasedSentence MakeVerbBasedSentence(string[] sentence, string[] posSentence, string[] lemmas, MorphoSyntacticFeatures[] morphoSyntacticFeatureses)

---------------------------------------------------------------------------------------------------
* A sample code
In the program.cs file there is a test output of a Persian sentence that can be used as a starting point.

var analyzer = new SentenceAnalyzer("../../../Data/VerbList.txt");
            var sentence =
                "من دارم به شما می‌گویم که این صحبت‌ها به راحتی گفته نخواهد شد و من با شما صحبت زیاد خواهم کرد.";

            var result = SentenceAnalyzer.MakeVerbBasedSentence(sentence);
            var output = new StringBuilder();
            foreach (var dependencyBasedToken in result.SentenceTokens)
            {
                output.AppendLine(dependencyBasedToken.WordForm + "\t" + dependencyBasedToken.Lemma + "\t" +
                                  dependencyBasedToken.CPOSTag
                                  + "\t" + dependencyBasedToken.HeadNumber + "\t" +
                                  dependencyBasedToken.DependencyRelation);
            }
            File.WriteAllText("../../../testOutPut.txt",output.ToString());


Output in "testOutPut.txt ":
من	_	_	-1	_
دارم	داشت#دار	V	4	PROG
به	_	_	-1	_
شما	_	_	-1	_
می‌گویم	گفت#گو	V	-1	_
که	_	_	-1	_
این	_	_	-1	_
صحبت‌ها	_	_	-1	_
به	_	_	-1	_
راحتی	_	_	-1	_
گفته نخواهد شد	گفت#گو	V	-1	_
و	_	_	-1	_
من	_	_	-1	_
با	_	_	-1	_
شما	_	_	-1	_
صحبت	_	_	17	NVE
زیاد	_	_	-1	_
خواهم کرد	کرد#کن	V	-1	_


---------------------------------------------------------------------------------------
* Verb Dictionary Format
The file is tab-separated with the following fields:
- verbType: integer
	1: simple, 2: prefix verb, 3: compound verb, 4: compound prefix verb , 5: prepositional compound prefix verb, 6: enclitic verb, 7: prepositional verb
- transitivity: integer
	0: intransitive, 1: transitive, x 2: bitransitive
- past tense root: string
	"-" if not present
- present tense root: string
	"-" if not present
- Non-verbal element: string
	"-" if not present
-Prefix: string
	"-" if not present
-Preposition:
	"-" if not present
-amrShodani: string:
	"-" =true,  *: false
- vowelEnd: string
	End of present root vowel: 
	U: ends with u, I: ends with ei, A: ends with a, ?: else

- maziVowel: string
	Start vowel type of past tense root
	A: starts with "a" or "\ae", @: else
- mozarehVowel: string
	Start vowel type of present tense root
	bU: starts with "bu", ba: start with "b\ae", bA: starts with "ba", A: starts with "a" or "\ae", !: else

----------------------------------------------------------------------------------------
* Some Points
	!!! I assumed the character set is being refined when you pass array argument to the methods. As shown in the follwoing code, I used Virastyar library (http://sourceforge.net/projects/virastyar/files/Virastyar/) for refining characters and tokenizing strings. 
	public static VerbBasedSentence MakeVerbBasedSentence(string sentence)
        {
            sentence = StringUtil.RefineAndFilterPersianWord(sentence); // using the refiner of Virastyar software
            var tokenized = PersianWordTokenizer.Tokenize(sentence,true); // using the tokenizer of Virastyar software
            return MakeVerbBasedSentence(tokenized.ToArray());
        }
		You can go to Virastyar official site in order to know more about its options (http://virastyar.ir). 
	
	!!! if you do not want to use it for your purposes you can clean the mentioned lines from the code
	
	!!! You can find a morphological-based POS tagger that can be used in your code. You can also use the tagger to help improve learner POS taggers such as HMM tagger.
	
	!!! I assumed that the writers use semi-space for verb inflections. In Bijankhan corpus, you can replace space with semi-space in words with verb tag.

	