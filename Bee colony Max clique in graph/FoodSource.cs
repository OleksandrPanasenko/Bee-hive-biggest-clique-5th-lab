namespace BeeColony{
    public class FoodSource{
        public Field field;
        public List<int> Vertices;
        public int Value;
        public int StepsNotChanged;
        public FoodSource(Field field){
            this.field = field;
            this.Vertices = new List<int>();
            StepsNotChanged=0;
        }
        public int Evaluate(){
            if(Vertices.Count==1){
                Value=1;
                return 1;
            }
            for (int i = 0;i<Vertices.Count;i++){
                for(int j = 0;j < Vertices.Count;j++){
                    if(i!=j&&field.Graph[Vertices[i],Vertices[j]]==false){
                        Value = 0;
                        return Value;
                    }
                }
            }
            Value=Vertices.Count;
            return Value;
        }
    }
}