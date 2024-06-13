using org.mariuszgromada.math.mxparser;

namespace Thumper_Custom_Level_Editor
{
    public class CellFunction
    {
        public string function { get; set; }
        public int rowindex { get; set; }
        public int columnindex { get; set; }

        public double Evaluate()
        {
            string func = Substitutions(this.function);
            Expression exp = new(func);
            return exp.calculate();
        }

        private string Substitutions(string func)
        {
            return func;
        }
    }
}
