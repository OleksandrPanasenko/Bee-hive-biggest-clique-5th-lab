namespace BeeColony{
    public class Field{
        public static int TotalVertices;
        public static int EdgesMin;
        public static int EdgesMax;
        public int Size{
            get{
                return Graph.GetLength(0);
            }
        }
        public int SourcesStarting=150;
        public int NumberBees=50;
        public float StartingScoutRatio=(float)0.8;
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
            bees=new Bee[NumberBees];
            for(int i=0;i<Size;i++){
                for(int j=0;j<Size;j++){
                    Graph[i,j]=false;
                }
            }
        }
        public Field(int Size):this(new bool[Size,Size]){
        }
        public static Field GenerateAsInTask(int Size){
            Random random=new Random();
            Size=TotalVertices;
            Field toReturn = new Field(Size);

            //edges with required number of connections
            List<int[]> VerticesNumberEdges;
            int TotalEdgeSum=0;
            do{
                TotalEdgeSum=0;
                VerticesNumberEdges=new List<int[]>();
                for(int VerticeNumber=0;VerticeNumber<Size;VerticeNumber++){
                    int[] ToAdd=[VerticeNumber, 0, random.Next(EdgesMin, EdgesMax+1)];
                    VerticesNumberEdges.Add(ToAdd);
                    TotalEdgeSum+=ToAdd[2];
                }
            }while (TotalEdgeSum%2==1);

            //shuffle list
            /*for(int i=0;i<Size;i++){
                int NewPlace=random.Next(Size);
                int[]buffer=VerticesNumberEdges[i];
                VerticesNumberEdges[i]=VerticesNumberEdges[NewPlace];
                VerticesNumberEdges[NewPlace]=buffer;
            }*/

            for(int i=0;i<TotalEdgeSum/2;i++){
                int currentVertice=GetBiggestIndex(VerticesNumberEdges);
                int secondVertice=-1;
                do{
                    secondVertice=random.Next(Size);
                        int difference=VerticesNumberEdges[secondVertice][2]-VerticesNumberEdges[secondVertice][1];
                        bool CanBeAdded=secondVertice!=currentVertice&&toReturn.Graph[secondVertice,currentVertice]!=true&&difference>0;
                        if(!CanBeAdded){
                            secondVertice=-1;
                        }
                    
                }while (secondVertice==-1);
                //int TotalPowerSum=0;
                /*foreach(int[]vertice in VerticesNumberEdges){
                    if(vertice[0]!=currentVertice&&toReturn.Graph[vertice[0],currentVertice]!=true){
                        TotalPowerSum+=vertice[2]-vertice[1];
                    }
                }
                int secondVertice=-1;
                TotalPowerSum=random.Next(TotalPowerSum);
                for(int j=0;j<Size&&TotalPowerSum>=0;j++){
                    if(currentVertice!=j&&toReturn.Graph[currentVertice,j]!=true)
                    TotalPowerSum-=VerticesNumberEdges[j][2]-VerticesNumberEdges[j][1];
                    if(TotalPowerSum<0) secondVertice=j;
                }*/
                if(secondVertice==-1) throw new Exception("Not accomodated all edges");
                toReturn.Graph[secondVertice,currentVertice]=true;
                toReturn.Graph[currentVertice,secondVertice]=true;
                VerticesNumberEdges[currentVertice][1]++;
                VerticesNumberEdges[secondVertice][1]++;
            }
            JoinClique(SelectNRandoms(8,Size),toReturn.Graph);
            return toReturn;
        }
        public static int GetBiggestIndex(List<int[]> array){
            int toReturn=0;
            int biggestLeft=array[0][2]-array[0][1];
            for(int i=0;i<array.Count;i++){
                if(array[i][2]-array[i][1]>biggestLeft){
                    biggestLeft=array[i][2]-array[i][1];
                    toReturn=i;
                }
            }
            return toReturn;
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
            for (int i=0;i<array.GetLength(0);i++){
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
                sw.WriteLine(Graph.GetLength(0));
                for(int i = 0; i < Graph.GetLength(0); i++){
                    for(int j = 0;j<Graph.GetLength(0);j++){
                        if(Graph[i,j]==true){
                            sw.Write("1 ");
                        }
                        else
                        {
                            sw.Write("0 ");
                        }
                    }
                    sw.WriteLine("");
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
        public void BeeSetup(){
            Bee starting=new Bee();
            starting.field=this;
            for(int i=0;i<NumberBees;i++){
                bees[i]=new Bee();
                bees[i].field=this;
                if(i<=NumberBees*StartingScoutRatio){
                    bees[i].State=Bee.SCOUT;
                }
                else{
                    bees[i].State=Bee.WORKER;
                }
            }
            for(int j=0;j<SourcesStarting;j++){
                starting.RandomSearch();
                if(starting.Location.Value>=2) foodSorces.Add(starting.Location);
            }
            
        }
        public void BeeIteration(){
            Random random=new Random();
            //scouts search
            foreach(Bee bee in bees){
                if (bee.State==Bee.SCOUT) {
                    bee.RandomSearch();
                    if(bee.Location.Value<2) {
                        bee.Location=null;
                    }
                }
            }
            //workers work and get fired
            foreach(Bee bee in bees){
                if(bee.State==Bee.WORKER){
                    
                    do{
                    int ValueSum=0;
                    foreach(FoodSource source in foodSorces){
                        ValueSum+=source.Value;
                    }
                    ValueSum=random.Next(ValueSum);
                    foreach(FoodSource source in foodSorces){
                        ValueSum-=source.Value;
                        if(ValueSum<0){
                            bee.Location=source;
                            break;
                        }
                        
                    }
                    }while(bee.Location is null);
                    bee.WorkOnExisting();
                    if(bee.Location.StepsNotChanged>NonChangeTreshold){
                        foodSorces.Remove(bee.Location);
                        bee.Location=null;
                        bee.State=Bee.SCOUT;
                    }
                }
            }
            //scouts settle
            foreach(Bee bee in bees){
                if (bee.State==Bee.SCOUT&&bee.Location is not null) {
                    bee.State=Bee.WORKER;
                    foodSorces.Add(bee.Location);
                }
            }
        }
    }
}