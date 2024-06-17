using Microsoft.VisualBasic;
using System.Diagnostics;

namespace VisualSharp;


public abstract partial class Node
{
    double positionX;
    double positionY;
    public event NodePositionChangedDelegate OnPositionChanged;
    public string Name { get; set; }
    public double PositionX
    {
        get => positionX;
        set
        {
            positionX = value;
            OnPositionChanged?.Invoke(this, value, positionY);
        }
    }
    public double PositionY
    {
        get => positionY;
        set
        {
            positionY = value;
            OnPositionChanged?.Invoke(this, positionX, value);
        }
    }

    public bool IsPure
    {
        get=>InputExecPins.Count==0 && OutputExecPins.Count==0;
        set
        {
            if(!CanSetPure) throw new InvalidOperationException("Cannot set pure");

            if(IsPure!=value) SetPurity(value);

            Debug.Assert(value == IsPure, "Purity could not be set correctly.");
        }
    }
    public virtual bool CanSetPure => false;

    //TODO: 好像是断开和其他节点的连接
    protected virtual void SetPurity(bool value){}
    
    // TODO:Data和Tpye合并
    public ObservableRangeCollection<InputDataPin> InputDataPins { get; private set; } = [];

    public ObservableRangeCollection<OutputDataPin> OutputDataPins { get; private set; } = [];

    public ObservableRangeCollection<InputExecPin> InputExecPins { get; private set; } = [];

    public ObservableRangeCollection<OutputExecPin> OutputExecPins { get; private set; } = [];

    public ObservableRangeCollection<InputTypePin> InputTypePins { get; private set; } = [];

    public ObservableRangeCollection<OutputTypePin> OutputTypePins { get; private set; } = [];

    public Graph Graph {get; private set;}
    public MethodGraph  MethodGraph=>Graph as MethodGraph;

    protected Node(Graph graph)
    {
        Graph = graph;
        Graph.Nodes.Add(this);
        Name = NameUtil.GetUniqueName(GetType().Name, Graph.Nodes.Select(n => n.Name).ToList());
    }

    public override string ToString()=>NameUtil.SplitCamelCase(GetType().Name);

}