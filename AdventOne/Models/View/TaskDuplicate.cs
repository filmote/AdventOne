namespace AdventOne.Models.View {

    public enum TaskFrequency {
        Monthly,
        Quarterly,
        Yearly
    }

    public class TaskDuplicate {

        public int ID { get; set; }
        public int Quantity { get; set; }
        public TaskFrequency TaskFrequency { get; set; }

    }
}