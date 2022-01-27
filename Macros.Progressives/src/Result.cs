namespace Macros.Progressives;

public sealed class Result<T> {
    private readonly Func<T> function;
    private T? result;
    private Func<Exception, T>? handleException;
    private Action<T>? thenAction;
    public Result(Func<T> function) {
        this.function = function;
    }
    public Result<T> Error(Func<Exception, T> handleException) {
        this.handleException = handleException;
        return this;
    }
    public Result<T> Then(Action<T> thenAction) {
        this.thenAction = thenAction;
        return this;
    }
    public T Unwrap() {
        try {
            result = function();
            if(thenAction != null) {
                thenAction(result);
            }
            return result;
        }
        catch(Exception ex) {
            if(handleException != null) {
                result = handleException(ex);
            }
            return result!;
        }
    }
}