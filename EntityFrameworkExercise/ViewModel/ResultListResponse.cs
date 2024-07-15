namespace EntityFrameworkExercise.ViewModel
{
    public class ResultListResponse <T>
    {
        public List<T> Data { get; set; } = [];
        public int PageCount { get; set; }
    }
}
