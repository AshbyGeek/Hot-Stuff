using System;
using System.Collections.Generic;

public class ObservableArgs { }

public interface IObservable<T> where T : ObservableArgs
{
    void Register(Action<object, T> callback);

    void UnRegister(Action<object, T> callback);
}

public class ObservableEvent : ObservableEvent<ObservableArgs> 
{
    public void SendMessage(object sender)
    {
        base.SendMessage(sender, new ObservableArgs());
    }
}

// TODO: use weak references and open-instance delegates to prevent garbage collection issues
public class ObservableEvent<T> : IObservable<T> where T : ObservableArgs
{
    private readonly HashSet<Action<object, T>> callbackCollection = new HashSet<Action<object, T>>();

    public void Register(Action<object, T> callback)
    {
        if (callback != null && !callbackCollection.Contains(callback))
        {
            callbackCollection.Add(callback);
        }
    }

    public void UnRegister(Action<object, T> callback)
    {
        if (callbackCollection.Contains(callback))
        {
            callbackCollection.Remove(callback);
        }
    }

    public void SendMessage(object sender, T args)
    {
        foreach (Action<object, T> callback in callbackCollection)
        {
            callback.Invoke(sender, args);
        }
    }
}