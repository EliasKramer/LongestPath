using Assets;

Solver_3 solver = new Solver_3();
/*
List<int> numbers = new List<int>() {
2,
3,
5,
7,
9,
11,
13,
17,
19,
21,
23,
29,
31,
33,
37,
41,
43,
47,
53,
59,
61,
67,
71,
73,
79,
83,
89,
97,
101,
103,
107,
109,
113,
127,
131,
137,
139,
149,
151,
157,
163,
167,
173,
179,
181,
191,
193,
197,
199,
211,
223,
227,
229,
233,
239,
241,
251,
257,
263,
269,
271,
277,
281,
283,
293,
307,
311,
313,
317,
331,
337,
347,
349,
353,
359,
367,
373,
379,
383,
389,
397,
401,
409,
419,
421,
431,
433,
439,
443,
449,
457,
461,
463,
467,
479,
487,
491,
499,
503,
509,
521,
523,
541,
547,
557,
563,
569,
571,
577,
587,
593,
599,
601,
607,
613,
617,
619,
631,
641,
643,
647,
653,
659,
661,
673,
677,
683,
691,
701,
709,
719,
727,
733,
739,
743,
751,
757,
761,
769,
773,
787,
797,
809,
811,
821,
823,
827,
829,
839,
853,
857,
859,
863,
877,
881,
883,
887,
907,
911,
919,
929,
937,
941,
947,
953,
967,
971,
977,
983,
991,
997
};
List<int> oldNumbers = new List<int>()
{
2,
3,
5,
7,
9,
15,
19,
21,
23,
27,
29,
35,
37,
71,
83,
105,
111,
115,
117,
121,
125,
131,
133,
169,
189,
191,
199,
201,
231,
237,
281,
293,
309,
311,
319,
333,
339,
379,
435,
459,
501,
515,
537,
551,
553,
579,
607,
623,
625,
647,
671,
675,
697,
713,
755,
759,
773,
777,
781,
785,
817,
819,
829,
839,
859,
861,
937
};

for (int i = 2; i < 100000; i++)
{
    if (i % 100 == 0)
    {
        Console.WriteLine("progress: " + i);
    }
    /*
    if (IsPrime(i))
    {
        continue;
    }
    var path = solver.CalculatePath(i);
    if (path.ConnectionCount == PathHelper.OptimalConnectionCount(i))
    {
        bool numbersContain = numbers.Contains(i);
        bool oldNumbersContain = oldNumbers.Contains(i);
        bool prime = IsPrime(i);

        Console.WriteLine($"{i}|\t new numbers contain({numbersContain})\t|old numbers contain({oldNumbersContain})\t|prime({prime})");
    }
}

//bool IsPrime(int number)
{
    if (number <= 1) return false;
    if (number == 2) return true;
    if (number % 2 == 0) return false;

    var boundary = (int)Math.Floor(Math.Sqrt(number));

    for (int i = 3; i <= boundary; i += 2)
        if (number % i == 0)
            return false;

    return true;
}
*/

for (int i = 3; ; i += 2)
{
    var res = solver.CalculatePath(i).ConnectionCount;
    if (res != PathHelper.OptimalConnectionCount(i))
    {
        Console.WriteLine(i);
    }
    if (i % 99 == 0)
    {
        Console.WriteLine("progress: " + i);
    }
}
