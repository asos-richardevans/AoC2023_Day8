var input = File.ReadAllLines("./input.txt");

var instructions = input[0].ToCharArray();

var map = new List<(string, (string, string))>();

for (int i = 2; i < input.Length; i++)
{
    map.Add((input[i][0..3], (input[i][7..10], input[i][12..15])));
}

var mapEntries = new List<string>();
mapEntries.AddRange(map.Where(x => x.Item1.EndsWith("A")).Select(x => x.Item1));

//Find number of steps for each starting location to reach an end location
//Find each numbers Prime numbers, then find the least common multiplier

var mapSteps = new List<int>();
for (int i = 0; i < mapEntries.Count; i++)
{
    var instructionIndex = 0;
    var steps = 0;
    while (true)
    {
        var nextMapId = map.First(x => x.Item1 == mapEntries[i]);
        mapEntries[i] = GetNextMapId(instructionIndex, nextMapId);
        steps++;
        if (mapEntries[i].EndsWith("Z"))
        {
            mapSteps.Add(steps);
            break;
        }
        instructionIndex++;
        if (instructionIndex >= instructions.Length)
        {
            instructionIndex = 0;
        }
    }
}

var finalPrimeFactors = new List<(int, int)>();
foreach (var mapStep in mapSteps)
{
    var pf = GetPrimeFactors(mapStep);
    foreach (var primeFactor in pf)
    {
        var existing = finalPrimeFactors.FirstOrDefault(x => x.Item1 == primeFactor.Item1);
        if (existing.Item1 == 0)
        {
            finalPrimeFactors.Add(primeFactor);
        }
        else
        {
            if (existing.Item2 < primeFactor.Item2)
            {
                finalPrimeFactors.Remove(existing);
                finalPrimeFactors.Add(primeFactor);
            }
        }
    }
}

//NOTE the cast of the seed to long, this is because the result of the multiplication is too large for an int
var finalSteps = finalPrimeFactors.Aggregate((long)1, (acc, x) => acc * (int)Math.Pow(x.Item1, x.Item2));


//Brute Force, takes too long
//var instructionIndex = 0;
//var steps = 0;

//while (true)
//{
//    for (int i = 0; i < mapEntries.Count; i++)
//    {
//        var nextMapId = map.First(x => x.Item1 == mapEntries[i]);
//        mapEntries[i] = GetNextMapId(instructionIndex, nextMapId);
//    }
//    steps++;
//    if (mapEntries.All(x=>x.EndsWith("Z")))
//    {
//        break;
//    }
//    instructionIndex++;
//    if (instructionIndex >= instructions.Length)
//    {
//        instructionIndex = 0;
//    }
//}

Console.WriteLine(finalSteps);

string GetNextMapId(int index, (string, (string, string)) nextMapId) => instructions[index] switch
{
    'L' => nextMapId.Item2.Item1,
    'R' => nextMapId.Item2.Item2
};

List<(int, int)> GetPrimeFactors(int number)
{
    //Console.WriteLine($"Finding prime factors for {number}");
    var primeFactors = new List<(int, int)>();
    for (int b = 2; number > 1; b++)
    {
        if (number % b == 0)
        {
            int x = 0;
            while (number % b == 0)
            {
                number /= b;
                x++;
            }

            primeFactors.Add((b, x));
            //Console.WriteLine($"{b} is a prime factor {x} times!");
        }
    }
    //Console.WriteLine($"-------------------");
    return primeFactors;
}