var input = File.ReadAllLines("./input.txt");

var instructions = input[0].ToCharArray();

var map = new List<(string, (string, string))>();

for (int i = 2; i < input.Length; i++)
{
    map.Add((input[i][0..3], (input[i][7..10], input[i][12..15])));
}

var instructionIndex = 0;
var steps = 0;
var mapId = "AAA";
while (true)
{
    var nextMapId = map.First(x => x.Item1 == mapId);
    mapId = GetNextMapId(instructionIndex, nextMapId);
    steps++;
    if (mapId == "ZZZ")
    {
        break;
    }
    instructionIndex++;
    if (instructionIndex >= instructions.Length)
    {
        instructionIndex = 0;
    }
}

Console.WriteLine(steps);


string GetNextMapId(int index, (string, (string, string)) nextMapId) => instructions[index] switch
{
    'L' => nextMapId.Item2.Item1,
    'R' => nextMapId.Item2.Item2
};