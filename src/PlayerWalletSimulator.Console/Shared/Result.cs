namespace PlayerWalletSimulator.Console.Shared
{
    public class Result
    {
        public bool Succeeded { get; }
        public string ErrorMessage { get; }

        protected Result(bool succeeded, string errorMessage)
        {
            Succeeded = succeeded;
            ErrorMessage = succeeded ? string.Empty : errorMessage;
        }

        public static Result Success
        {
            get { return new Result(true, string.Empty); }
        }

        public static Result Failure(string errorMessage)
        {
            return new Result(false, errorMessage);
        }

        public static implicit operator Result(string errorMessage)
        {
            return Failure(errorMessage);
        }

        public static implicit operator Result(bool success)
        {
            return success ? Success : Failure("Unsuccessful operation.");
        }

        public static implicit operator bool(Result result)
        {
            return result.Succeeded;
        }
    }

    public class Result<TData> : Result
    {
        private readonly TData _data;

        public TData Data
        {
            get
            {
                if (Succeeded)
                {
                    return _data;
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(Data)} is not available with a failed result. Use {ErrorMessage} instead.");
                }
            }
        }

        protected Result(bool succeeded, TData data, string errorMessage) : base(succeeded, errorMessage)
        {
            _data = data;
        }

        public static Result<TData> SuccessWith(TData data)
        {
            return new Result<TData>(true, data, string.Empty);
        }

        public static new Result<TData> Failure(string errorMessage)
        {
            return new Result<TData>(false, default!, errorMessage);
        }

        public static implicit operator Result<TData>(string errorMessage)
        {
            return Failure(errorMessage);
        }

        public static implicit operator Result<TData>(TData data)
        {
            return SuccessWith(data);
        }

        public static implicit operator bool(Result<TData> result)
        {
            return result.Succeeded;
        }
    }
}
