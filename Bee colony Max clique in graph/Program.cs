using BeeColony;

Field.TotalVertices=300;
Field.EdgesMin=2;
Field.EdgesMax=30;
Field field=Field.GenerateAsInTask(3000);
field.NonChangeTreshold=4000;
field.SaveToFile();
field.BeeSetup();
field.SaveToFile();
for (int i=0; i<100000;i++){
    field.BeeIteration();
    Console.WriteLine($"{i} iterations - largest is {field.record.Value}");
}

// See https://aka.ms/new-console-template for more information
Console.WriteLine($"Biggest clique found has {field.record.Value} vertices");
