namespace BeeColony{
    public class Field{
        public int Size{
            get{
                return Graph.Length;
            }
        }
        public int NumberBees=50;
        public Bee[] bees;
        public int NonChangeTreshold;
        public const string SaveFile="Graph.txt";
        public bool[,] Graph;
        public List<FoodSource> foodSorces;
        public FoodSource record;
        public int RecordLength;
        public Field(bool[,] graph){
            Graph = graph;
            foodSorces=new List<FoodSource>();
            bees=new Bee[50];
        }
        public Field(int Size):this(new bool[Size,Size]){
        }
        public static Field Generate(int Size){
            Field toReturn = new Field(Size);
            GenerateNoise((float)0.3, toReturn.Graph);
            JoinClique(SelectNRandoms(8, Size-1), toReturn.Graph);
            for(int i=0; i<3;i++){
                JoinClique(SelectNRandoms(4, Size-1), toReturn.Graph);
            }
            //setting connections
            return toReturn;
        }
        public static void JoinClique(List<int> Vertices, bool[,] array){
            foreach(int i in Vertices){
                foreach(int j in Vertices){
                    array[i,j]=true;
                }
            }
        }
        public static void GenerateNoise(float probability, bool[,] array){
            Random random=new Random();
            for (int i=0;i<array.Length;i++){
                for (int j=0;j<i+1;j++){
                    if(i==j)array[i,j]=true;
                    else{
                        if(random.NextSingle()<probability){
                            array[i,j]=true;
                            array[j,i]=true;
                        }
                        else{
                            array[j,i]=false;
                            array[i,j]=false;
                        }
                    }
                }
            }
        }
        public static List<int> SelectNRandoms(int n, int Roof){
            if(n>Roof+1) throw new Exception("Trying to select more numbers than possible");
            Random random=new Random();
            List<int> selection=new List<int>();
            for(int i = 0;i<n;i++){
                int toAdd;
                do{
                    toAdd=random.Next(Roof+1);
                }while (selection.Contains(toAdd));
                selection.Add(toAdd);
            }
            return selection;
        }
        public void SaveToFile(){
            using(StreamWriter sw = new StreamWriter(File.Open(SaveFile, FileMode.Create))){
                sw.WriteLine(Graph.Length);
                for(int i = 0; i < Graph.Length; i++){
                    for(int j = 0;j<Graph.Length;j++){
                        if(Graph[i,j]==true){
                            sw.Write("1 ");
                        }
                        else
                        {
                            sw.Write("0 ");
                        }
                    }
                }
            }
        }
        public void FromFile(){
            
            using(StreamReader sr = new StreamReader(File.Open(SaveFile, FileMode.Open))){
                int Size=int.Parse(sr.ReadLine());
                Graph=new bool[Size,Size];
                for(int i = 0;i<Size;i++){
                    string[]Row=sr.ReadLine().Split(" ");
                    for(int j = 0;j<Size;j++){
                        if(Row[j]=="1") Graph[i,j]=true;
                        else if(Row[j]=="0") Graph[i,j]=true;
                    }
                }
            }
        }
        public void BeeIteration(){

        }
    }
}