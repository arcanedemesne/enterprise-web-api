namespace Enterprise.Solution.Shared.Exceptions
{
    public class ExceptionHelper
    {
        public class RequestType
        {
            private RequestType(string value) { Value = value; }

            public string Value { get; private set; }

            public static RequestType ListAll { get { return new RequestType("List All"); } }
            public static RequestType ListPaged { get { return new RequestType("List Paged"); } }
            public static RequestType GetById { get { return new RequestType("Get By Id"); } }
            public static RequestType Add { get { return new RequestType("Add"); } }
            public static RequestType Update { get { return new RequestType("Update"); } }
            public static RequestType Patch { get { return new RequestType("Patch"); } }
            public static RequestType Delete { get { return new RequestType("Delete"); } }

            public override string ToString()
            {
                return Value;
            }
        }
    }
}
