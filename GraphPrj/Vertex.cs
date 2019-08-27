namespace GraphPrj
{
    public class Vertex
    {
        public int ID;

        public Vertex(int id)
        {
            ID = id;
        }

        public override bool Equals(object obj)
        {
            Vertex tmp = (Vertex)obj;
            return tmp.ID == ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}