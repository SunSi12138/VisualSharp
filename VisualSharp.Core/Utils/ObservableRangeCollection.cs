
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace VisualSharp;

/// <summary>
/// 表示一个可观察的范围集合，继承自ObservableCollection<T>。
/// </summary>
/// <typeparam name="T">集合中元素的类型。</typeparam>
public class ObservableRangeCollection<T> : ObservableCollection<T>
{
    /// <summary>
    /// 将一系列项添加到集合中。
    /// </summary>
    /// <param name="list">要添加的项的范围。</param>
    public void AddRange(IEnumerable<T> list)
    {
        foreach (var item in list)
        {
            Items.Add(item);
        }
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    /// <summary>
    /// 从集合中移除一系列项。
    /// </summary>
    /// <param name="list">要移除的项的范围。</param>
    public void RemoveRange(IEnumerable<T> list)
    {
        foreach (var item in list)
        {
            Items.Remove(item);
        }
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    
    /// <summary>
    /// 替换集合中的一系列项。
    /// </summary>
    /// <param name="collection">要替换的项的范围。</param>
    public void ReplaceRange(IEnumerable<T> collection)
    {
        Items.Clear();

        foreach (var item in collection)
        {
            Items.Add(item);
        }

        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public ObservableRangeCollection(): base() {}
    public ObservableRangeCollection(IEnumerable<T> collection): base(collection) {}
}