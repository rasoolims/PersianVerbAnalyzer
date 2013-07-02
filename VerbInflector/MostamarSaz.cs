using System;

namespace VerbInflector
{
	public class MostamarSaz
	{
		public string Type;
		public int Head;
		public VerbInflection Inflection;
		public MostamarSaz (VerbInflection inflec, int head, string type)
		{
			Type = type;
			Inflection = inflec;
			Head = head;
		}
	}
}

