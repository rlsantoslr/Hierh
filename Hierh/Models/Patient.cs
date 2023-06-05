namespace Hierh.Models
{
    public class Patient
    {
        public Patient()
        {
        }

        public Patient(string identifier)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        public string Identifier { get; set; }

        public string FamiliarRelation { get; set; }

        public List<Disease> Diseases { get; set; } = new List<Disease>();

        public void AddDisease(string name, bool ishereditary)
        {
            Diseases.Add(new Disease() { Name = name, IsHereditary = ishereditary });
        }
    }
}
