
namespace BeeColony{
    
    public class Bee{
        const bool WORKER = true;
        const bool SCOUT = false;
        public bool State;

        public FoodSource Location;

        public void WorkOnExisting(){
            Random random = new Random();
            int Position=random.Next(Location.Vertices.Count);
            int old=Location.Vertices[Position];
            int OldValue=Location.Evaluate();
            List<int> avaliable=new List<int>();
            for(int i = 0;i<Location.field.Size;i++){
                if(!Location.Vertices.Contains(i))avaliable.Add(i);
            }
            Location.Vertices[Position]=avaliable[random.Next(avaliable.Count)];
            if(Location.Evaluate()<OldValue){
                Location.Vertices[Position]=OldValue;
            }
            else{
                GreedyIteration();
            }
            if(Location.Value>OldValue){
                Location.StepsNotChanged=0;
            }
            else{
                Location.StepsNotChanged++;
            }
            if(Location.Vertices.Count>Location.field.RecordLength){
                Location.field.RecordLength=Location.Vertices.Count;
                Location.field.record=Location;
            }

        }

        /*public FoodSource SearchForNew(){
            
        }*/
        public void EvaluateFoodSource(FoodSource food){
            Location.Evaluate();
        }
        public void GreedyIteration(){
            Random random = new Random();
            List<int> avaliable=new List<int>();
            for(int i = 0;i<Location.field.Size;i++){
                if(!Location.Vertices.Contains(i))avaliable.Add(i);
            }
            for(int i = 0;i<avaliable.Count;i++){
                int NewPosition=random.Next(avaliable.Count);
                int buffer=avaliable[i];
                avaliable[i]=avaliable[NewPosition];
                avaliable[NewPosition]=buffer;
            }
            foreach(int i in avaliable){
                if(VerticeCliqueAddition(i)){
                    GreedyIteration();
                    return;
                }
            }
        }
        public bool VerticeCliqueAddition(int i){
            foreach (int j in Location.Vertices){
                if(i==j) return false;
                if(Location.field.Graph[i,j]==false) return false;
            }
            return true;
        }
        public void RandomSearch(){
            Random random = new Random();
            Location=new FoodSource(Location.field);
            Location.Vertices.Add(random.Next(Location.field.Size));
            GreedyIteration();
        }
        public void ReplaceRandom(){
            
        }
    }
}