namespace nodeSys2
{
    //wrapper classes for primitive data types for easy reference passing and serialization

    public class IntData
    {
        public int num;
        public IntData(int num)
        {
            this.num = num;
        }

        public override string ToString()
        {
            return "IntWrapper: " + num;
        }
    }

    public class FloatData
    {
        public float num;
        public FloatData(float num)
        {
            this.num = num;
        }
        public override string ToString()
        {
            return "FloatWrapper: " + num;
        }
    }

    public class StringData
    {
        public string txt;
        public StringData(string num)
        {
            this.txt = num;
        }

        public override string ToString()
        {
            return "StringWrapper: " + txt;
        }
    }
}
